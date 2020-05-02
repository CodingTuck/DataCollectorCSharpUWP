using System;
using System.Threading;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;

namespace DataCollector
{
    class Device
    {
        //device for gathering measurements
        //declaring the fields
        private Timer timer;
        private int data = 0;
        Random rnd = new Random();

        public Device()
        {
            //timer to capture the data every 15 seconds
            timer = new Timer(Timer_Tick, null, (int)TimeSpan.FromSeconds(1).TotalMilliseconds, (int)TimeSpan.FromSeconds(15).TotalMilliseconds);
        }

        private async void Timer_Tick(object state)
        {
            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                () =>
                {
                    //get a measurement between 1 and 10
                    data = rnd.Next(1, 11);
                });
        }
        //make it accessible 
        public int GetMeasurement => data;
    }
}