using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ControlPanelPlugin.Items;
using ControlPanelPlugin.Utils;
using Boomlagoon.JSON;

namespace ControlPanelPlugin
{
  [ClassSerializer("PanelItem")]
  public abstract class PanelItem : IJsonConvertable
  {
    public virtual ControlPanel Panel { get; set; }

    public abstract bool Update();

    public virtual void Initialize()
    {

    }

    public virtual void OnGUI()
    {

    }

    public virtual JSONObject ToJson()
    {
      return new JSONObject();
    }

    public virtual void FromJson(JSONObject json)
    {

    }
  }
}
