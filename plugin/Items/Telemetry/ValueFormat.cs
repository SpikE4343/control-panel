using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlPanelPlugin.Items.Telemetry
{
  public class PrecisionValue<T>
  {
    public T Value;
    public int Precision;
  }

  public abstract class ValueFormat<T>
  {
    public List<PrecisionValue<T>> PrecisionValues = new List<PrecisionValue<T>>();


    public abstract int GetPrecision(T value);
  }
}
