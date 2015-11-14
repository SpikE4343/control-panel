

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
  public class TelemetryItem : VesselPropertyItem
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

      if (GUILayout.Button(expanded ? "-" : "+", GUILayout.Width(25)))
      {
        expanded = !expanded;
      }

      if (expanded)
      {
        Property = GUILayout.TextField(Property, s);
      }
      else
      {
        s.alignment = TextAnchor.MiddleLeft;
        GUILayout.Label(Property, s, GUILayout.ExpandWidth(true));
      }

      s.alignment = TextAnchor.LowerRight;
      GUILayout.Label(Value.ToString(), s, GUILayout.Width(100));

      GUILayout.EndHorizontal();

      if (expanded && Display != null)
      {
        Display.OnGUI();
      }

      GUILayout.EndVertical();
    }

    public virtual float GetLatestValue()
    {
      return PropertyValue;
    }

    public override bool Update()
    {
      float next = GetLatestValue();
      if (Display == null)
        return true;

      return Display.Update(next);
    }


    public override JSONObject ToJson()
    {
      var json = base.ToJson();
      json.Add("display", Singleton.Get<ClassSerializer>().ToJson(Display));
      return json;
    }

    public override void FromJson(JSONObject json)
    {
      base.FromJson(json);
      Display = Singleton.Get<ClassSerializer>().FromJson<TelemetryDisplay>(json["display"]);
    }
  }
}
