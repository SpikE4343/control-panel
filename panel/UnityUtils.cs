using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace ControlPanelPlugin
{
  public class UnityUtils
  {
    public static void GUIField(string label, string value)
    {
      GUILayout.BeginHorizontal();
      GUILayout.Label(label);
      GUILayout.Label(value);
      GUILayout.EndHorizontal();
    }

    public static string GUIStringField(string label, string value)
    {
      GUILayout.BeginHorizontal();
      GUILayout.Label(label);
      string text = GUILayout.TextField(value);
      GUILayout.EndHorizontal();
      return text;
    }

    public static int GUIIntField(string label, int value)
    {
      GUILayout.BeginHorizontal();
      GUILayout.Label(label);
      string text = GUILayout.TextField(value.ToString());
      GUILayout.EndHorizontal();

      int result = value;
      Int32.TryParse(text, out result);
      return result;
    }
  }
}
