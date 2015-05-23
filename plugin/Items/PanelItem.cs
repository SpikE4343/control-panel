using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ControlPanelPlugin.Items;
using ControlPanelPlugin.Utils;

namespace ControlPanelPlugin
{
  [ClassSerializer("PanelItem")]
  public abstract class PanelItem
  {
    public virtual ControlPanel Panel { get; set; }

    public abstract bool Update();

    public virtual void Initialize()
    {

    }

    public virtual void OnGUI()
    {

    }

    public virtual Dictionary<string, object> ToJson()
    {
      return new Dictionary<string, object>();
    }

    public virtual void FromJson(Dictionary<string, object> json)
    {

    }
  }
}
