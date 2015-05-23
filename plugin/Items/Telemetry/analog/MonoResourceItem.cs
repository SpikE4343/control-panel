using ControlPanelPlugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace ControlPanelPlugin.Telemetry.analog
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
