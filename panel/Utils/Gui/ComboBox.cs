using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ControlPanelPlugin.Utils.Gui
{
  public class ComboBox
  {
    bool open = false;
    int selection = 0;
    Vector2 scroll;
    string[] items;

    public ComboBox(string[] items)
    {
      this.items = items;
    }

    public int Selection
    {
      get
      {
        return selection;
      }

      set
      {
        if (items != null && value >= 0 && value < items.Length)
        {
          selection = value;
        }
      }
    }

    public string SelectedItem
    {
      get { return items != null ? items[Selection] : string.Empty; }
    }

    public bool Show()
    {
      return UnityUtils.GUISelection(items, ref selection, ref open, ref scroll, 150);
    }
  }
}
