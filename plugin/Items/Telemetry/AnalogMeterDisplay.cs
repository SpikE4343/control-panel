using ControlPanelPlugin;
using ControlPanelPlugin.Telemetry;

namespace ControlPanelPlugin.telemetry.display
{
  public class AnalogMeterDisplay : TelemetryDisplay
  {
    public byte meter = 255;
    public byte meterValue = 0;

    public AnalogMeterDisplay()
    {

    }

    public AnalogMeterDisplay(int meter)
    {
      meter = (byte)meter;
    }

    public override bool Update(float value)
    {
      Value = value;

      byte val = (byte)((Value / 100.0f) * 255.0);
      bool same = val == meterValue;
      meterValue = val;

      if (!same)
      {
        Send();
      }

      return same;
    }

    public override void Send()
    {
      ConnectionManager.Instance.Connection.SendAnalogMeterMsg(meter, meterValue);
    }

  }
}
