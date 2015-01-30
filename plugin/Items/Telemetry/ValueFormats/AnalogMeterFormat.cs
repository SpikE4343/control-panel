using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlPanelPlugin.Items.Telemetry.ValueFormats
{
  public class AnalogMeterFormat : FloatValueFormat<byte>
  {
    public override byte Format(float input)
    {
      return (byte)((input / 100.0f) * 255.0);
    }
  }
}
