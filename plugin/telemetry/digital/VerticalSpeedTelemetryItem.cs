///
///
using ControlPanelPlugin.telemetry.digital;

namespace ControlPanelPlugin.telemetry
{
  public class VerticalSpeedTelemetryItem : DigitalTelemetryItem
  {
    public VerticalSpeedTelemetryItem(int id,
                                 int display,
                                 int startDigit,
                                 int maxDigits,
                                 int precision)
      : base(id, display, startDigit, maxDigits, precision)
    {

    }

    public override bool Update(IVessel vessel)
    {
      float speed = vessel.verticalSpeed;
      Precision = 3;

      if (speed >= 1000)
      {
        Precision = 0;
      }
      else if (speed >= 100)
      {
        Precision = 1;
      }
      else if (speed >= 10)
      {
        Precision = 2;
      }

      if (speed != Value)
      {
        Value = speed;
        Send();
        return true;
      }

      return false;
    }

  }
}
