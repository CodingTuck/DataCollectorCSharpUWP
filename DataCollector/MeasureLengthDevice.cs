using System;
using System.Text;
using System.Threading;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;

namespace DataCollector 
{
    public class MeasureLengthDevice : IMeasuringDevice
    {
        //declare fields for MeasureLengthDevice.
        //This class uses the interface to start/stop the device and decide which unit of measurement will be converted to inches/centimeters.
        Device measureDevice = new Device();
        FixedQueue<int> myQueue = null;
        private Timer timer;
        private int mostRecentMeasure = 0;      
        private int[] dataCaptured;
        private Units unitsToUse;
       
        
        //construct the device
        public MeasureLengthDevice(Units unitsToUse)
        {
            this.unitsToUse = unitsToUse;
            myQueue = new FixedQueue<int>
            {
                Limit = 10
            };
            //holds 10 ints that will be displayed in the history portion of the user interface and limits the display to 10
            dataCaptured = new int[10];
            
        }

        public string History => PrintValues(myQueue);

        //calls FixedQueue class and builds what the output will be to the history box in the user interface
        private string PrintValues(FixedQueue<int> myCollection)
        {
            StringBuilder myString = new StringBuilder();
            foreach (var i in myCollection.q)
                myString.AppendLine(i.ToString());
            return myString.ToString();
        }
        //executes the timer in a different thread and sets most recent measure on the new timer tick.
        private async void Timer_Tick(object state)
        {
            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    mostRecentMeasure = measureDevice.GetMeasurement;
                    myQueue.Enqueue(mostRecentMeasure);
                });
        }
        //returns a decimal representation of the metric unit that was captured. Raw data displays 8 Inches, textbox displays number of inches to centimeters
        public decimal MetricValue()
        {
            //calls the enum Unit that is Metric
            if (unitsToUse == Units.Metric)
            {
                return mostRecentMeasure;
            }
            return (decimal)(mostRecentMeasure * 2.54);
        }
        //returns a decimal representation of the Imperial unit that was captured. Raw data displays 8 Centimeters, textbox displays number of Centimeters to inches
        public decimal ImperialValue()
        {
            //calls the enum Unit that is Imperial
            if (unitsToUse == Units.Imperial)
            {
                return (decimal)(mostRecentMeasure * 0.393701);
            }
            return mostRecentMeasure;
        }

        
        //starts the device so it can take measurements at an interval of 15 seconds
        public void StartCollecting()
        {
            timer = new Timer(Timer_Tick, null, (int)TimeSpan.FromSeconds(1).TotalMilliseconds, (int)TimeSpan.FromSeconds(15).TotalMilliseconds);
            
        }
        //stops the device
        public void StopCollecting()
        {
            this.timer.Dispose();
        }
    }
}
