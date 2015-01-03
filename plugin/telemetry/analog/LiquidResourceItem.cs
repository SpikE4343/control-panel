using ControlPanelPlugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlPanelPlugin.telemetry.analog
{
    public class LiquidResourceItem : AnalogTelemetryPercentItem
    {
        public LiquidResourceItem(int meter) 
            : base(meter)
        {

        }

        public override bool Update(IVessel vessel)
        {
            if (!UpdateValue(vessel.liquidResourcePercent))
            {
                Send();
                return true;
            }

            return false;
        }
    }
}
