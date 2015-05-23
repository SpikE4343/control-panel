using ControlPanelPlugin.Telemetry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ControlPanelPlugin.Utils;
using ControlPanelPlugin.Messages;


namespace ControlPanelPlugin.Telemetry.display
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
      Value = -1;
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

      if (next != Value || Panel.ResendAll)
      {
        Value = next;
        Send();
        return true;
      }

      return false;
    }

    public override void Send()
    {
      var msg = Singleton.Get<ObjectPool>().Grab<TelemetryMsg>();
      msg.display = (byte)Display;
      msg.id = (byte)TelemetryId;
      msg.maxDigits = (byte)MaxDigits;
      msg.startDigit = (byte)StartDigit;
      msg.precision = (byte)Precision;
      msg.value = (int)(Value * Math.Pow(10, Precision));

      Singleton.Get<MessageManager>().WriteMsg(msg);
    }

    public override Dictionary<string, object> ToJson()
    {
      var json = base.ToJson();
      json.Add("id", TelemetryId);
      json.Add("display", Display);
      json.Add("start", StartDigit);
      json.Add("max", MaxDigits);
      json.Add("precision", Precision);
      return json;
    }

    public override void FromJson(Dictionary<string, object> json)
    {
      TelemetryId = (int)json["id"];
      Display = (int)json["display"];
      StartDigit = (int)json["start"];
      MaxDigits = (int)json["max"];
      Precision = (int)json["precision"];
    }
  }
}
