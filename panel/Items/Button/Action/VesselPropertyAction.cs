using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Boomlagoon.JSON;
using ControlPanelPlugin.Items;
using ControlPanelPlugin.Items.Button;
using ControlPanelPlugin.Utils;
using panel;


namespace ControlPanelPlugin.Items.Button.Action
{
  [ClassSerializer("VesselPropertyAction")]
  public class VesselPropertyAction : ButtonAction
  {
    public override string Name { get { return string.Format("Prop: {0}", propertyName.ToString()); } }

    public VesselPropertyAction()
    {

    }

    public VesselPropertyAction(string propName)
    {
      Property = propName;
    }

    public override void StateChange()
    {
      if (PropertyValue == Button.State)
        return;

      PropertyValue = Button.State;
    }

    private string propertyName = string.Empty;

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
      if (property.PropertyType != typeof(bool))
      {
        property = null;
      }
    }

    public bool PropertyValue
    {
      get
      {
        if (Button.Panel == null || Button.Panel.CurrentVessel == null || property == null)
          return false;

        return (bool)property.GetValue(Button.Panel.CurrentVessel, null);
      }

      set
      {
        if (Button.Panel == null || Button.Panel.CurrentVessel == null || property == null)
          return;

        property.SetValue(Button.Panel.CurrentVessel, value, null);
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
