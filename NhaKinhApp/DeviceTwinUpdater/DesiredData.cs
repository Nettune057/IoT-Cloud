using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp
{
    public class DesiredData
    {
        public string? DeviceId { get; set; }
        public int? Temperature {get; set;}
        public int? MinHumidity { get; set;}
        public int? MaxHumidity { get; set; }
        public int? MinCO2 { get; set; }
        public int? MaxCO2 { get; set; }
        public int Disable { get; set; }
        public int LedState { get; set; }
    }
}
