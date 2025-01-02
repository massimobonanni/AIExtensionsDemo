using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIExtensionsDemo.TrafficAnalysis
{
    internal class TrafficImageResult
    {
        public TrafficStatus Status { get; set; }
        public int NumCars { get; set; }
        public int NumBikes { get; set; }
        public int NumTrucks { get; set; }

        public enum TrafficStatus
        {
            Good,
            Moderate,
            Heavy
        }

    }
}
