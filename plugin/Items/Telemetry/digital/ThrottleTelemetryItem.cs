using ControlPanelPlugin;
using ControlPanelPlugin.Telemetry;
using ControlPanelPlugin.telemetry.digital;

namespace ControlPanelPlugin.telemetry
{
  public class ThrottleTelemetryItem : DigitalTelemetryItem
  {
    public ThrottleTelemetryItem()
    {
      Add(0, 0);
    }

    public override float GetLatestValue()
    {
      return Panel.CurrentVessel.mainThrottle * 100;
    }
  }
}
