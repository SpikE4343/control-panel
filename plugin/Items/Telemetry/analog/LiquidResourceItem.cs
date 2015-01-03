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
      public LiquidResourceItem()
        {

        }

        public LiquidResourceItem(int meter) 
            : base(meter)
        {

        }

        public override bool Update()
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
