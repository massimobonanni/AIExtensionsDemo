using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIExtensionsDemo.TrafficAnalysis
{
    internal class TrafficImageResult
    {
        [Description("The status of the traffic in the image.")]
        public TrafficStatus Status { get; set; }

        [Description("The number of cars in the image.")]
        public int NumCars { get; set; }

        [Description("The number of buses in the image.")]
        public int NumBikes { get; set; }

        [Description("The number of trucks in the image.")]
        public int NumTrucks { get; set; }

        public enum TrafficStatus
        {
            Good,
            Moderate,
            Heavy
        }

    }
}
