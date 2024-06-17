using Microsoft.EntityFrameworkCore;
using SolarManagement.Data;
using SolarManagement.Models;
using SolarManagement.Helpers;
using SolarManagement.ViewModel;

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
                await UpdateEsp(await IsBatteryOk()); //Update ESP based on battery status

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
        public async Task<List<LoadData>> GetLoadData()
        {
            List<LoadData> batteryData = new List<LoadData>();

            using (var scope = _serviceProvider.CreateScope())
            {
                var _context = scope.ServiceProvider.GetRequiredService<SolarManagementContext>();  //Add this to use the SolarManagement data context

                //PUT CODE HERE;

                string sqlQuery = "SELECT * FROM [db3861].[dbo].[vPower]";
                var data = _context.powertbl.FromSqlRaw(sqlQuery);
                var loadData = from power in data
                               select new LoadData
                               {
                                   id = power.id,
                                   volt = power.volt,
                                   power = power.power,
                                   Ampere = power.Ampere,
                                   EspNum = power.EspNum,
                                   TimeData = power.datetimecreated.Value.ToString("HH:mm"),
                                   DateData = power.datetimecreated.Value.ToString("MMM-dd-yyyy"),
                               };

                batteryData = await loadData.ToListAsync();
            }

            return batteryData;
        }

        //CHECK BATTERY AVERAGE VOLTAGE
        public async Task<bool> IsBatteryOk()
        {
            List<LoadData> batteryData = await GetLoadData();
            decimal totalVoltage = 0;
            decimal averageVoltage = 0;

            foreach(var data in batteryData)
            {
                if(data.volt.HasValue)
                {
                    totalVoltage += data.volt.Value;
                }
            }

            averageVoltage = totalVoltage / batteryData.Count;

            if(averageVoltage >= (decimal) 2.8)
            {
                return true;
            }
            else if(averageVoltage <= (decimal) 2.5)
            { 
                return false; 
            }
            else
            {
                return false;
            }
        }

        //FOR UPDATING THE LOAD ESP TO TURN OFF ON BASED ON BATTERY VOLTAGE
        public async Task UpdateEsp(bool isOk)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var _context = scope.ServiceProvider.GetRequiredService<SolarManagementContext>();  //Add this to use the SolarManagement data context

                //PUT CODE HERE;
                if (isOk)
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
                else
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
    }
}
