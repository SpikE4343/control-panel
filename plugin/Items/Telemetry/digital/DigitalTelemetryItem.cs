using ControlPanelPlugin.Telemetry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace ControlPanelPlugin.telemetry.digital
{
    public class DigitalTelemetryItem : TelemetryItem
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

      public DigitalTelemetryItem()
      { 
        
      }

      public void Add(float value, int precision)
      {
        PrecisionValues.Add(new PrecisionValue { Value = value, Precision = precision });
      }
        

      public DigitalTelemetryItem( int id, int display, int startDigit, int maxDigits, int precision )
      {
        TelemetryId = id;
        Display = display;
        StartDigit = startDigit;
        MaxDigits = maxDigits;
        Precision = precision;

        SetupFormatting();
      }

      public override bool Update()
      {
        float next = GetLatestValue();

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
          // Value = next;
          Send();
          return true;
        }

        return false;
      }

      public void Send()
      {
        PanelManager.Instance.Connection.SendTelemetryMessage(Id, Display, StartDigit, MaxDigits, Precision, Value);
      }

      public virtual void SetupFormatting()
      {

      }

      public virtual float GetLatestValue()
      {
        return 0.0f;
      }
    }
}
