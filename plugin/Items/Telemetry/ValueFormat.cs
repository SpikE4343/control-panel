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

  public interface IValueFormat
  {
    int Precision(object value);
    T Format<T>(object value);
  }
  public abstract class ValueFormat<TInput, TOutput> : IValueFormat
  {
    public List<PrecisionValue<TInput>> PrecisionValues;

    public override int GetPrecision(TInput value)
    {
      if (PrecisionValues == null || PrecisionValues.Count <= 0)
      {
        return 0;
      }

      int last = 0;
      foreach (var p in PrecisionValues)
      {
        if( Greater( value, p.Value) )
        {
          return p.Precision;
        }

        last = p.Precision;
      }

      return last;
    }

    private abstract bool Greater(TInput a, TInput b);
    public abstract TOutput Format(TInput input);

    int IValueFormat.Precision(object value)
    {
      return GetPrecision((TInput)value);
    }

    object IValueFormat.Format<T>(object value)
    {
      return Format((TInput)value);
    }
  }
}
