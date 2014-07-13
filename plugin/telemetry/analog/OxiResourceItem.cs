using ControlPanelPlugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace plugin.telemetry.analog
{
    public class OxiResourceItem : AnalogTelemetryPercentItem
    {
        public OxiResourceItem(int meter)
            : base(meter)
        {
        }

        public override bool Update(IVessel vessel)
        {
            if (!UpdateValue(vessel.oxiResourcePercent))
            {
                Send();
                return true;
            }

            return false;
        }
    }
}
