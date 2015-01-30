using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlPanelPlugin.Items.Telemetry.ValueFormats
{
  public class FloatValueFormat<TOutput> : ValueFormat<float, TOutput>
  {
    public override bool Greater(float a, float b)
    {
      return a > b;
    }
  }
}
