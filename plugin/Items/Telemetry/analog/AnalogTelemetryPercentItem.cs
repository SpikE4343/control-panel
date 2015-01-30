using ControlPanelPlugin;
using ControlPanelPlugin.Telemetry;

namespace ControlPanelPlugin.telemetry.analog
{
  public abstract class AnalogTelemetryPercentItem : TelemetryItem
  {

    private byte mMeter = 255;
    private byte mValue = 0;

    public AnalogTelemetryPercentItem()
    {

    }

    public AnalogTelemetryPercentItem(int meter)
    {
      mMeter = (byte)meter;
    }

    protected bool UpdateValue(double value)
    {
      byte val = (byte)((value / 100.0f) * 255.0);
      bool same = val == mValue;
      mValue = val;

      if (!same)
      {
        Send();
      }

      return same;
    }

    public void Send()
    {
      ConnectionManager.Instance.Connection.SendAnalogMeterMsg(mMeter, mValue);
    }
  }
}
