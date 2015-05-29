

using ControlPanelPlugin.Telemetry.Display;
using System.Reflection;
using System.Collections.Generic;
using ControlPanelPlugin.Utils;
using ControlPanelPlugin.Items;
using Boomlagoon.JSON;
using UnityEngine;
namespace ControlPanelPlugin.Telemetry
{
  [ClassSerializer("TelemetryItem")]
  public class TelemetryItem : PanelItem
  {
    private static int nextId = 0;
    public int Id = nextId++;

    public float Value { get { return Display != null ? Display.Value : 0.0f; } }


    ControlPanel panel;
    public override ControlPanel Panel
    {
      get { return panel; }
      set
      {
        panel = value;
        if (Display != null)
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

    private TelemetryDisplay display;
    public TelemetryDisplay Display
    {
      get { return display; }

      set
      {
        display = value;
        display.Panel = Panel;
      }
    }

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

    bool expanded = false;
    public override void OnGUI()
    {
      GUIStyle s = new GUIStyle();
      s.stretchHeight = false;
      s.alignment = TextAnchor.LowerRight;
      s.normal.textColor = Color.white;

      GUILayout.BeginVertical();
      GUILayout.BeginHorizontal();

      if (GUILayout.Button(expanded ? "-" : "+", "label"))
      {
        expanded = !expanded;
      }

      if (expanded)
      {
        propertyName = GUILayout.TextField(propertyName);
      }
      else
      {
        GUILayout.Label(propertyName, s, GUILayout.ExpandWidth(true));
      }



      GUILayout.Label(Value.ToString(), s);
      GUILayout.EndHorizontal();

      if (expanded && Display != null)
      {
        Display.OnGUI();
      }

      GUILayout.EndVertical();
    }

    public virtual float GetLatestValue()
    {
      if (Panel == null || Panel.CurrentVessel == null || property == null)
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
