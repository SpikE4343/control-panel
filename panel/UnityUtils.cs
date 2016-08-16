using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace ControlPanelPlugin
{
  public class UnityUtils
  {
    public static void GUIField(string label, string value, int width = 100)
    {
      GUILayout.BeginHorizontal();
      GUILayout.Label(label);
      GUILayout.Label(value, GUILayout.Width(width));
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

    public static int GUIIntField(string label, int value, int width = 100)
    {
      GUILayout.BeginHorizontal();
      GUILayout.Label(label);
      string text = GUILayout.TextField(value.ToString(), GUILayout.Width(width));
      GUILayout.EndHorizontal();

      int result = value;
      Int32.TryParse(text, out result);
      return result;
    }

    public static bool GUISelection(string[] items,
                                    ref int selection,
                                    ref bool open,
                                    ref Vector2 scroll,
                                    int width)
    {
      bool pressed = false;
      if (!open)
      {
        pressed = GUILayout.Button(items[selection], GUILayout.Width(width));
      }

      if (!open && !pressed)
        return false;

      if (open && pressed)
      {
        open = false;
        return false;
      }

      GUILayout.BeginVertical("box", GUILayout.Height(250), GUILayout.Width(width));
      scroll = GUILayout.BeginScrollView(scroll, false, false);
      var next = GUILayout.SelectionGrid(selection, items, 1, GUILayout.Width(width - 50));
      GUILayout.EndScrollView();
      GUILayout.EndVertical();

      bool changed = next != selection;
      open = !changed;
      selection = next;
      return changed;
    }
  }
}
