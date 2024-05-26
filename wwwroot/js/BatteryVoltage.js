//Declaring global variables
//FOR ALERT
var Toast = Swal.mixin({
    toast: true,
    position: 'top-end',
    showConfirmButton: false,
    timer: 2000
});

//FOR BATTERY VOLATAGES AND CURRENT MONITORING ---------------------------------------------------------------------------------
let batteryVoltages = [];
let batteryTemperature = [];
let batterNumber = [];
let batteryDate = [];
let currentTime = []; //convert to current date and get time
let updateTime = 1 * 1000; //1seconds
let isRealTime = false // realtime data of voltage or temperature

async function GetBatteryVoltage(apiUrl, voltageBarChart)
{
    const Data = await fetch(apiUrl);
    if (Data.status === 200) //status success
    {
        const result = await Data.json();
        //iterate to get the result
        for (i = 0; i < result.length; i++) {
            batteryVoltages.push(result[i].voltage);
            currentTime.push(result[i].timeData.toString());
            batterNumber.push(result[i].batterNumber)
        };
    }
    else {
        toastr.error("Error fetching data");
    }


    //-------------
    //- BAR CHART - VOLTAGES DATA
    //-------------


    // Get context with jQuery - using jQuery's .get() method.
    const ChartData_Voltage = {
        labels: batterNumber,
        datasets: [{
            label: 'Battery Voltages',
            data: batteryVoltages,
            backgroundColor: 'rgba(51, 51, 225, 0.5)',
        }]
    };

    var ChartOptions_Voltage = {
        responsive: true,
        maintainAspectRatio: false,
        datasetFill: false,
        scales: {
            xAxes: [{
                ticks: {
                    beginAtZero: true,
                    max: 16
                },
                display: true,
                scaleLabel: {
                    display: true,
                    labelString: 'Battery Number',
                },
            }],
            yAxes: [{
                ticks: {
                    beginAtZero: true,
                    max: 4
                },
                display: true,
                scaleLabel:
                {
                    display: true,
                    labelString: 'Volts',
                },
            }]
        },
    }

    const myChart_Voltage = new Chart(voltageBarChart,
        {
            type: 'bar',
            data: ChartData_Voltage,
            options: ChartOptions_Voltage
        })

    return myChart_Voltage;
}

async function GetBatteryTemperature(apiUrl, temperatureBarChart)
{
    const Data = await fetch(apiUrl);
    if (Data.status === 200) //status success
    {
        const result = await Data.json();
        //iterate to get the result
        for (i = 0; i < result.length; i++) {
            batteryTemperature.push(result[i].temperature);
            currentTime.push(result[i].timeData.toString());
            batterNumber.push(result[i].batterNumber)
        };
    }
    else {
        toastr.error("Error fetching data");
    }


    //-------------
    //- BAR CHART - TEMPERATURE DATA
    //-------------

    const ChartData_Temperature = {
        labels: batterNumber,
        datasets: [{
            label: 'Battery Temperature',
            data: batteryTemperature,
            backgroundColor: 'rgba(0, 204, 0, 0.5)',
        }]
    };

    var ChartOptions_Temperature = {
        responsive: true,
        maintainAspectRatio: false,
        datasetFill: false,
        scales: {
            xAxes: [{
                ticks: {
                    beginAtZero: true,
                    max: 16
                },
                display: true,
                scaleLabel: {
                    display: true,
                    labelString: 'Battery Number',
                },
            }],
            yAxes: [{
                ticks: {
                    beginAtZero: true,
                    max: 50
                },
                display: true,
                scaleLabel:
                {
                    display: true,
                    labelString: 'Celcius',
                },
            }]
        },
    }

    const myChart_Temperature = new Chart(temperatureBarChart,
        {
            type: 'bar',
            data: ChartData_Temperature,
            options: ChartOptions_Temperature
        })

    return myChart_Temperature;
}

//Get Live Data when voltage barchart is clicked
async function GetLiveVoltageData(apiUrl, lineChartCanvas)
{
    const Data = await fetch(apiUrl);
    if (Data.status === 200) //status success
    {
        const result = await Data.json();
        //iterate to get the result
        for (i = 0; i < result.length; i++) {
            batteryVoltage.push(result[i].voltage);
            time.push(result[i].timeData.toString());
        };
    }
    else {
        toastr.error("Fetch data error");
    }


    //-------------
    //- LINE CHART - VOLTAGES DATA
    //-------------

    const ChartData = {
        labels: time,
        datasets: [{
            label: 'Live Battery Voltage Data',
            data: batteryVoltage,
            borderColor: 'rgb(75, 192, 192)',
            tension: 0.1
        }]
    };

    var ChartOptions = {
        responsive: true,
        maintainAspectRatio: false,
        datasetFill: false,
        legend: {
            display: true
        },
        scales: {
            xAxes: [{
                gridLines: {
                    display: true,
                },
                max: 60,
            }],
            yAxes: [{
                gridLines: {
                    display: true,
                }
            }]
        },
        fill: false
    }

    const myChart = new Chart(lineChartCanvas,
        {
            type: 'line',
            data: ChartData,
            options: ChartOptions
        })

    return myChart;
}