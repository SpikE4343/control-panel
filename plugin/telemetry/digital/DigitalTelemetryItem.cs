using ControlPanelPlugin.Telemetry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace plugin.telemetry.digital
{
    public class DigitalTelemetryItem : TelemetryItem
    {
        public int TelemetryId;
        public int Display;
        public int StartDigit;
        public int MaxDigits;
        public int Precision;

        public DigitalTelemetryItem( int id,
                                     int display,
                                     int startDigit,
                                     int maxDigits,
                                     int precision)
        {
            TelemetryId = id;
            Display = display;
            StartDigit = startDigit;
            MaxDigits = maxDigits;
            Precision = precision;
        }

        public override bool Update(ControlPanelPlugin.IVessel vessel)
        {
             throw new NotImplementedException();
        }
    }
}
