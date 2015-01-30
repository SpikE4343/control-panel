using ControlPanelPlugin.Telemetry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlPanelPlugin.telemetry.display
{
    public class DigitalDisplay : TelemetryDisplay
    {
      public int TelemetryId;
      public int Display;
      public int StartDigit;
      public int MaxDigits;
      public int Precision;

      public List<PrecisionValue> PrecisionValues = new List<PrecisionValue>();

      public class PrecisionValue
      {
        public float Value;
        public int Precision;
      }

      public DigitalDisplay()
      { 
        
      }

      public void Add(float value, int precision)
      {
        PrecisionValues.Add(new PrecisionValue { Value = value, Precision = precision });
      }


      public DigitalDisplay(int id, int display, int startDigit, int maxDigits)
      {
        TelemetryId = id;
        Display = display;
        StartDigit = startDigit;
        MaxDigits = maxDigits;
      }

      public override bool Update(float next)
      {
        foreach (var p in PrecisionValues)
        {
          if (next > p.Value)
          {
            Precision = p.Precision;
            break;
          }
        }

        if (next != Value)
        {
          Value = next;
          Send();
          return true;
        }

        return false;
      }

      public override void Send()
      {
        ConnectionManager.Instance.Connection.SendTelemetryMessage(TelemetryId, Display, StartDigit, MaxDigits, Precision, Value);
      }
    }
}
