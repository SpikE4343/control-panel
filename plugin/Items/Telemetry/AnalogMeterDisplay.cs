using System.Collections.Generic;
using Boomlagoon.JSON;
using ControlPanelPlugin;
using ControlPanelPlugin.Messages;
using ControlPanelPlugin.Telemetry;
using ControlPanelPlugin.Utils;

namespace ControlPanelPlugin.Telemetry.Display
{
  [ClassSerializer("AnalogMeterDisplay")]
  public class AnalogMeterDisplay : TelemetryDisplay
  {
    public byte meter = 255;
    public byte meterValue = 0;

    public AnalogMeterDisplay()
    {

    }

    public AnalogMeterDisplay(int meter)
    {
      this.meter = (byte)meter;
    }

    public override bool Update(float value)
    {
      Value = value;

      byte val = (byte)((Value / 100.0f) * 255.0);
      bool same = val == meterValue;
      meterValue = val;

      if (!same || Panel.ResendAll)
      {
        Send();
      }

      return same;
    }

    public override void Send()
    {
      var msg = Singleton.Get<ObjectPool>().Grab<AnalogMeterMsg>();
      msg.meter = meter;
      msg.value = meterValue;
      Singleton.Get<MessageManager>().WriteMsg(msg);
    }

    public override JSONObject ToJson()
    {
      var json = base.ToJson();
      json.Add("meter", meter);
      return json;
    }

    public override void FromJson(JSONObject json)
    {
      meter = json["meter"];
    }

  }
}
