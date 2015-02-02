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
  }
}
