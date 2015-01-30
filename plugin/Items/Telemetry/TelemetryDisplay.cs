using ControlPanelPlugin;
using ControlPanelPlugin.Telemetry;
using Newtonsoft.Json;

namespace ControlPanelPlugin.telemetry.display
{
  public abstract class TelemetryDisplay
  {
    [JsonIgnore]
    public float Value { get; set; }
    public abstract bool Update(float value);
    public abstract void Send();
  }
}
