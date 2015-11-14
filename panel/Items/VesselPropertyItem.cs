using System.Reflection;
using Boomlagoon.JSON;
using ControlPanelPlugin.Utils;

namespace ControlPanelPlugin
{
  [ClassSerializer("VesselPropertyItem")]
  public abstract class VesselPropertyItem : PanelItem
  {
    private string propertyName;

    public string Property
    {
      get { return propertyName; }
      set
      {
        propertyName = value;
        SetupProperty();
      }
    }

    private PropertyInfo property;

    public void SetupProperty()
    {
      var vessel = typeof(IVessel);
      property = vessel.GetProperty(Property);
    }

    public float PropertyValue
    {
      get
      {
        if (Panel == null || Panel.CurrentVessel == null || Property == null)
          return 0.0f;

        return (float)property.GetValue(Panel.CurrentVessel, null);
      }
    }

    public override JSONObject ToJson()
    {
      var json = base.ToJson();
      json.Add("property", Property);
      return json;
    }

    public override void FromJson(JSONObject json)
    {
      Property = json["property"];
    }
  }
}
