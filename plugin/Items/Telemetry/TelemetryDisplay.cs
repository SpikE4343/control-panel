using ControlPanelPlugin;
using ControlPanelPlugin.Telemetry;
using Newtonsoft.Json;

namespace ControlPanelPlugin.telemetry.display
{
  public abstract class TelemetryDisplay
  {
    [JsonIgnore]
    public float Value { get; set; }

    [JsonIgnore]
    public ControlPanel Panel { get; set; }

    [JsonIgnore]
    public PanelItem Item { get; set; }

    public abstract bool Update(float value);
    public abstract void Send();
  }
}
