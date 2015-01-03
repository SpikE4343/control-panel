using ControlPanelPlugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlPanelPlugin.telemetry.analog
{
  public class OxiResourceItem : AnalogTelemetryPercentItem
  {
    public OxiResourceItem()
    {

    }
    public OxiResourceItem(int meter)
      : base(meter)
    {
    }

    public override bool Update()
    {
      return UpdateValue(Panel.CurrentVessel.oxiResourcePercent);
    }
  }
}
