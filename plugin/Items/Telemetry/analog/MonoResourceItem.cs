using ControlPanelPlugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlPanelPlugin.telemetry.analog
{
    public class MonoResourceItem : AnalogTelemetryPercentItem
    {
      public MonoResourceItem()
        {

        }

        public MonoResourceItem(int meter)
            : base(meter)
        {
        }


        public override bool Update()
        {
            if (!UpdateValue(Panel.CurrentVessel.monoResourcePercent))
            {
                Send();
                return true;
            }

            return false;
        }
    }
}
