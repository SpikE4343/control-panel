using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ControlPanelPlugin.Utils;

namespace ControlPanelPlugin
{
  public class Config : IJsonConvertable
  {
    public IntervalsConfig Intervals = new IntervalsConfig();
    public class IntervalsConfig : IJsonConvertable
    {
      public float InputUpdate = 0.01f;
      public float PanelUpdate = 0.0625f;
      public float VesselUpdate = 1.0f;

      #region IJsonConvertable Members

      public Dictionary<string, object> ToJson()
      {
        var json = new Dictionary<string, object>();
        json.Add("input", InputUpdate);
        json.Add("panel", PanelUpdate);
        json.Add("vessel", VesselUpdate);
        return json;
      }

      public void FromJson(Dictionary<string, object> json)
      {
        InputUpdate = (float)json["input"];
        PanelUpdate = (float)json["panel"];
        VesselUpdate = (float)json["vessel"];
      }

      #endregion
    }
    #region IJsonConvertable Members

    public Dictionary<string, object> ToJson()
    {
      var json = new Dictionary<string, object>();
      json.Add("intervals", Intervals.ToJson());
      return json;
    }

    public void FromJson(Dictionary<string, object> json)
    {
      Intervals.FromJson(json["intervals"] as Dictionary<string, object>);
    }

    #endregion
  }
}
