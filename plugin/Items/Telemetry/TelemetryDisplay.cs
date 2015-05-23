using System.Collections.Generic;
using ControlPanelPlugin;
using ControlPanelPlugin.Items;
using ControlPanelPlugin.Telemetry;
using ControlPanelPlugin.Utils;
using Newtonsoft.Json;

namespace ControlPanelPlugin.Telemetry.display
{
  [ClassSerializer("TelemetryDisplay")]
  public abstract class TelemetryDisplay : IJsonConvertable
  {
    [JsonIgnore]
    public float Value { get; set; }

    [JsonIgnore]
    public ControlPanel Panel { get; set; }

    [JsonIgnore]
    public PanelItem Item { get; set; }

    public abstract bool Update(float value);
    public abstract void Send();

    #region IJsonConvertable Members

    public virtual Dictionary<string, object> ToJson()
    {
      return new Dictionary<string, object>();
    }

    public virtual void FromJson(Dictionary<string, object> json)
    {

    }

    #endregion
  }
}
