///
///
using ControlPanelPlugin.telemetry.digital;

namespace ControlPanelPlugin.telemetry
{
  public class TerrainHeightTelemetryItem : DigitalTelemetryItem
  {
    public TerrainHeightTelemetryItem()
    {
      
    }

    public TerrainHeightTelemetryItem(int id,
                                 int display,
                                 int startDigit,
                                 int maxDigits,
                                 int precision)
      : base(id, display, startDigit, maxDigits, precision)
    {

    }

    public override void SetupFormatting()
    {
      Add(1000, 0);
      Add(100, 1);
      Add(10, 2);
      Add(0, 3);
    }

    public override float GetLatestValue()
    {
      return Panel.CurrentVessel.heightFromTerrain;      
    }
  }
}
