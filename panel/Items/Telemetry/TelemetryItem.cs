﻿

using ControlPanelPlugin.Telemetry.Display;
using System.Reflection;
using System.Collections.Generic;
using ControlPanelPlugin.Utils;
using ControlPanelPlugin.Items;
using Boomlagoon.JSON;
namespace ControlPanelPlugin.Telemetry
{
  [ClassSerializer("TelemetryItem")]
  public class TelemetryItem : PanelItem
  {
    private static int nextId = 0;
    public int Id = nextId++;

    public float Value { get { return Display.Value; } }


    ControlPanel panel;
    public override ControlPanel Panel
    {
      get { return panel; }
      set
      {
        panel = value;
        Display.Panel = panel;
      }
    }

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


    public TelemetryDisplay Display { get; set; }

    public TelemetryItem()
    {

    }

    public TelemetryItem(string propertyName, TelemetryDisplay display)
    {
      Display = display;
      Property = propertyName;

      SetupProperty();
    }

    public void SetupProperty()
    {
      var vessel = typeof(IVessel);
      property = vessel.GetProperty(Property);
    }

    public virtual void OnGUI()
    {

    }

    public virtual float GetLatestValue()
    {
      if (Panel == null || Panel.CurrentVessel == null)
        return 0.0f;

      return (float)property.GetValue(Panel.CurrentVessel, null);
    }

    public override bool Update()
    {
      float next = GetLatestValue();
      return Display.Update(next);
    }


    public override JSONObject ToJson()
    {
      var json = base.ToJson();
      json.Add("property", Property);
      json.Add("display", Singleton.Get<ClassSerializer>().ToJson(Display));
      return json;
    }

    public override void FromJson(JSONObject json)
    {
      Property = json["property"];
      Display = Singleton.Get<ClassSerializer>().FromJson<TelemetryDisplay>(json["display"]);
    }
  }
}