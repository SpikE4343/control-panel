using System.Collections.Generic;
using Boomlagoon.JSON;
using ControlPanelPlugin;
using ControlPanelPlugin.Items;
using ControlPanelPlugin.Telemetry;
using ControlPanelPlugin.Utils;

namespace ControlPanelPlugin.Telemetry.Display
{
  [ClassSerializer("TelemetryDisplay")]
  public abstract class TelemetryDisplay : IJsonConvertable
  {
    public float Value { get; set; }

    public ControlPanel Panel { get; set; }

    public PanelItem Item { get; set; }

    public abstract bool Update(float value);
    public abstract void Send();

    #region IJsonConvertable Members

    public virtual JSONObject ToJson()
    {
      return new JSONObject();
    }

    public virtual void FromJson(JSONObject json)
    {

    }

    #endregion
  }
}
