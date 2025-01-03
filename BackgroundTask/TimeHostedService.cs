using Microsoft.EntityFrameworkCore;
using SolarManagement.Data;
using SolarManagement.Models;
using SolarManagement.Helpers;
using SolarManagement.ViewModel;
using System.Reflection.Metadata;

namespace SolarManagement.BackgroundTask
{
    public class TimedHostedService : IHostedService, IDisposable
    {
        private int executionCount = 0;
        private readonly ILogger<TimedHostedService> _logger;
        private readonly IConfiguration _configuration;
        private readonly IServiceProvider _serviceProvider; //Add this to use the IMRB data context
        private Timer? _timer = null;

        public TimedHostedService(ILogger<TimedHostedService> logger, IConfiguration configuration, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _configuration = configuration;
            _serviceProvider = serviceProvider;
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Timed Hosted Service running.");

            _timer = new Timer(DoWork, null, TimeSpan.Zero,
                TimeSpan.FromMinutes(1));

            return Task.CompletedTask;
        }

        private async void DoWork(object? state)
        {
            try
            {
                var count = Interlocked.Increment(ref executionCount);

                _logger.LogInformation(
                    "Timed Hosted Service is working. Count: {Count}", count);

                //PUT CODE HERE --------------------------------------------------------------------------------------
                await UpdateEsp(await BatteryState()); //Update ESP based on battery state
                await UpdateMpptEsp(); //Update the mppt relay based on battery voltage reading

            }

            catch (Exception ex)
            {
                ErrorLogs.CreateErrorLogFile(ex.ToString(), "TimedHostedService", "DoWork");
            }
        }

        public Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Timed Hosted Service is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }

        //FOR CHECKING THE BATTERY DATA
        public async Task<List<BatteryVoltages>> GetLoadData()
        {
            List<BatteryVoltages> batteryDataList = new List<BatteryVoltages>();

            using (var scope = _serviceProvider.CreateScope())
            {
                var _context = scope.ServiceProvider.GetRequiredService<SolarManagementContext>();  //Add this to use the SolarManagement data context

                //PUT CODE HERE;

                string sqlQuery = "SELECT * FROM [db3861].[dbo].[vBatteries]";
                var query = _context.batterytbl.FromSqlRaw(sqlQuery);
                var batteryData = from battery in query
                               select new BatteryVoltages
                               {
                                   Id = battery.id,
                                   batterNumber = battery.batt,
                                   voltage = battery.volt,
                                   temperature = battery.temp,
                                   current = battery.Ampere,
                                   TimeData = battery.dttmcreated.Value.ToString("HH:mm"),
                                   DateData = battery.dttmcreated.Value.ToString("MMM-dd-yyyy"),
                               };

                batteryDataList = await batteryData.ToListAsync();
            }

            return batteryDataList;
        }

        //CHECK BATTERY AVERAGE VOLTAGE
        public async Task<string> BatteryState()
        {
            List<BatteryVoltages> batteryData = await GetLoadData();
            int[] batteriesA = { 1, 2, 3, 4, 5, 6, 7, 8 };
            int[] batteriesB = { 9, 10, 11, 12, 13, 14, 15, 16 };

            decimal totalVoltageA = 0; //for battery set A. 
            decimal averageVoltageA = 0; //for battery set A. 

            decimal totalVoltageB = 0; //for battery set B. 
            decimal averageVoltageB = 0; //for battery set B. 

            //Get battery set A voltages
            var setA = (from data in batteryData where batteriesA.Contains(data.batterNumber.Value) select data).ToList();
            foreach(var battery in setA)
            {
                if(battery.voltage.HasValue)
                {
                    totalVoltageA += (decimal) battery.voltage.Value;
                }
            }

            //Get battery set B voltages
            var setB = (from data in batteryData where batteriesB.Contains(data.batterNumber.Value) select data).ToList();
            foreach (var battery in setB)
            {
                if (battery.voltage.HasValue)
                {
                    totalVoltageB += (decimal) battery.voltage.Value;
                }
            }

            averageVoltageA = totalVoltageA / setA.Count;
            averageVoltageB = totalVoltageB / setB.Count;

            decimal TotalAverageVoltage = (averageVoltageA + averageVoltageB) / 2;

            if (TotalAverageVoltage >= (decimal) 2.7)
            {
                return "on";
            }
            else
            { 
                if(TotalAverageVoltage < (decimal) 2.5)
                {
                    return "off";
                }
                else
                {
                    return "KeepState";
                }
            }
        }

        //FOR UPDATING THE LOAD ESP TO TURN OFF ON BASED ON BATTERY VOLTAGE
        public async Task UpdateEsp(string state)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var _context = scope.ServiceProvider.GetRequiredService<SolarManagementContext>();  //Add this to use the SolarManagement data context

                //PUT CODE HERE;
                if (state.ToLower() == "on")
                {
                    //Turn on esp 2
                    var esp2 = await (from esp in _context.loadtbl where esp.id == 2 select esp).FirstOrDefaultAsync();
                    if(esp2.state == 0)
                    {
                        esp2.state = 1;
                        await _context.SaveChangesAsync();
                    }

                    //Turn on esp 3
                    var esp3 = await (from esp in _context.loadtbl where esp.id == 3 select esp).FirstOrDefaultAsync();
                    if (esp3.state == 0)
                    {
                        esp3.state = 1;
                        await _context.SaveChangesAsync();
                    }
                }
                else if(state.ToLower() == "off")
                {
                    //Turn off esp 2
                    var esp2 = await (from esp in _context.loadtbl where esp.id == 2 select esp).FirstOrDefaultAsync();
                    if (esp2.state == 1)
                    {
                        esp2.state = 0;
                        await _context.SaveChangesAsync();
                    }

                    //Turn off esp 3
                    var esp3 = await (from esp in _context.loadtbl where esp.id == 3 select esp).FirstOrDefaultAsync();
                    if (esp3.state == 1)
                    {
                        esp3.state = 0;
                        await _context.SaveChangesAsync();
                    }
                }
            }
        }

        //UPDATE THE MPPT ESP TO TURN OFF BASED ON BATTERY VOLTAGE(Voltage is more than 4)
        public async Task UpdateMpptEsp()
        {
            List<BatteryVoltages> batteryData = await GetLoadData();
            var volt = from v in batteryData where v.voltage > (decimal)3.5 select v; // 1 or more battery has a voltage greater than 4 volts
            if(volt.Count() > 0)
            {
                //UPDATE THE RELAY STATE TO 0 TO CUT OFF MPPT BATTERY CHARGING
                using (var scope = _serviceProvider.CreateScope())
                {
                    var _context = scope.ServiceProvider.GetRequiredService<SolarManagementContext>();  //Add this to use the SolarManagement data context
                    var esp4 = await (from esp in _context.loadtbl where esp.id == 4 select esp).FirstOrDefaultAsync();
                    if (esp4.state == 1)
                    {
                        esp4.state = 0;
                        await _context.SaveChangesAsync();
                    }
                }
            }
            else
            {
                //UPDATE THE RELAY STATE TO 1 TO TURN ON MPPT BATTERY CHARGING
                using (var scope = _serviceProvider.CreateScope())
                {
                    var _context = scope.ServiceProvider.GetRequiredService<SolarManagementContext>();  //Add this to use the SolarManagement data context
                    var esp4 = await (from esp in _context.loadtbl where esp.id == 4 select esp).FirstOrDefaultAsync();
                    if (esp4.state == 0)
                    {
                        esp4.state = 1;
                        await _context.SaveChangesAsync();
                    }
                }
            }
        }

    }
}
