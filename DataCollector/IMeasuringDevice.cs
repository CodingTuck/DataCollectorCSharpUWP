using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCollector
{
    //define an interface for accessing measurements
    public interface IMeasuringDevice
    {
        //returns a decimal representation of the metric value that was captured
        decimal MetricValue();
        //returns a decimal representation of the imperial value that was captured
        decimal ImperialValue();
        //this method will get the device to start running
        void StartCollecting();
        //this method will stop the device
        void StopCollecting();
    }
}
