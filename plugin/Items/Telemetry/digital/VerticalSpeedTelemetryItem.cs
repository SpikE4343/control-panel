///
///
using ControlPanelPlugin.Telemetry.digital;

namespace ControlPanelPlugin.Telemetry
{
  public class VerticalSpeedTelemetryItem : DigitalTelemetryItem
  {
    public VerticalSpeedTelemetryItem()
      {
        Add(1000, 0);
        Add( 100, 1);
        Add(  10, 2);
        Add(   0, 3);
      }           

    public virtual float GetLatestValue()
    {
      return Panel.CurrentVessel.verticalSpeed;
    }
  }
}
