using ControlPanelPlugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlPanelPlugin.telemetry.analog
{
    public class EvResourceItem : AnalogTelemetryPercentItem
    {
        public EvResourceItem(int meter)
            : base(meter)
        {
        }

        public override bool Update(IVessel vessel)
        {
            if (!UpdateValue(vessel.electricResourcePercent))
            {
                Send();
                return true;
            }

            return false;
        }
    }
}
