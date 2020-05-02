using System;
using System.Globalization;
using System.Threading;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;


// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace DataCollector
{
    //creates the interactions from the mainpage.xaml
    public sealed partial class MainPage : Page
    {
        //declare fields and Objects
        private Timer timer;
        private Units measuringDeviceUnit;        
        Frame frame = null;
        MainPage page = null;
        MeasureLengthDevice measurementDevice = null;        
        MainViewData displayData = new MainViewData();
        
       
        //initialize the user interface

        public MainPage()
        {
            this.InitializeComponent();
            
            measuringDeviceUnit = Units.Imperial;
            measurementDevice = new MeasureLengthDevice(measuringDeviceUnit);
                        
            displayData = new MainViewData
            {   
                //get and set the History from MainViewData.History to the measurement device
                History = measurementDevice.History                
            };
            
        }

        //executest timer in a different thread and checks for what unit of measurement to convert to and display
        private async void Timer_Tick(object state)
        {
            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                if (page != null)
                {
                    if (inchesButton.IsChecked == true)
                    {
                        //calls the MeasureLengthDevice.Imperial() method and displays it to the textbox with a timestamp
                        page.measure.Text = measurementDevice.ImperialValue().ToString("#.##") + "in." + " " + DateTime.Now;                       
                    }
                    else if (inchesButton.IsChecked != true && centButton.IsChecked == true)
                    {
                        //calls the MeasureLengthDevice.Metric() method and displays it to the textbox with a timestamp
                        page.measure.Text = measurementDevice.MetricValue().ToString("#.##") + "cm." + " " + DateTime.Now;                       
                    }                                       
                }               
            });
            
        }

        
        //stop the device when user clicks the button
        private void StopCollecting_clicked(object sender, RoutedEventArgs e)
        {
            measurementDevice.StopCollecting();
            this.timer.Dispose();

        }
        //starts the device when user clicks start collecting
        private void StartCollecting_Click(object sender, RoutedEventArgs e)
        {
            timer = new Timer(Timer_Tick, null, (int)TimeSpan.FromSeconds(1).TotalMilliseconds, (int)TimeSpan.FromSeconds(15).TotalMilliseconds);
            
            measurementDevice.StartCollecting();
        }

        private void recentMeasurementButton_Click(object sender, RoutedEventArgs e)
        {
            if (inchesButton.IsChecked == true)
            {
                //calls the MeasureLengthDevice.Imperial() method and displays it to the textbox with a timestamp
                measure.Text = measurementDevice.ImperialValue().ToString("#.##") + "in." + " " + DateTime.Now;
            }
            else if (inchesButton.IsChecked != true && centButton.IsChecked == true)
            {
                //calls the MeasureLengthDevice.Metric() method and displays it to the textbox with a timestamp
                measure.Text = measurementDevice.MetricValue().ToString("#.##") + "cm." + " " + DateTime.Now;
            }
        }

        //displays the raw data, or the opposite of what measurement the user wants converted into the history box under Raw data button. 
        //if user has Inches checked, then raw data displays the centimeters and the text box with the timestamp displays the inches conversion and vise versa
        private void RawDataClick(object sender, RoutedEventArgs e)
        {
            frame = (Frame)Window.Current.Content;
            page = (MainPage)frame.Content;
            measureHistory.Text = measurementDevice.History.ToString();
        }
    }
}
