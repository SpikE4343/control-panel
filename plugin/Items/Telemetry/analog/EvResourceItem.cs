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
    public EvResourceItem()
    {

    }

    public EvResourceItem(int meter)
      : base(meter)
    {
    }

    public override bool Update()
    {
      if (!UpdateValue(Panel.CurrentVessel.electricResourcePercent))
      {
        Send();
        return true;
      }

      return false;
    }
  }
}
