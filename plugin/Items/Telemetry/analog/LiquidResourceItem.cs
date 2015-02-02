﻿using ControlPanelPlugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


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

            if (!UpdateValue(Panel.CurrentVessel.liquidResourcePercent))
            {
                Send();
                return true;
            }

            return false;
        }
    }
}
