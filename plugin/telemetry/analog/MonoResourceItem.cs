using ControlPanelPlugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace plugin.telemetry.analog
{
    public class MonoResourceItem : AnalogTelemetryPercentItem
    {
        public MonoResourceItem(int meter)
            : base(meter)
        {
        }

        public override bool Update(IVessel vessel)
        {
            if (!UpdateValue(vessel.monoResourcePercent))
            {
                Send();
                return true;
            }

            return false;
        }
    }
}
