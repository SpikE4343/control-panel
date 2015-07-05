using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Boomlagoon.JSON;
using ControlPanelPlugin.Utils;

namespace ControlPanelPlugin.Items.Button.Action
{
  public abstract class ButtonAction : IJsonConvertable
  {
    public ButtonItem Button;

    public virtual string Name { get { return GetType().Name; } }
    public abstract void StateChange();
    public virtual bool Update()
    {
      return true;
    }

    #region IJsonConvertable Members

    public virtual JSONObject ToJson()
    {
      return new JSONObject();
    }

    public virtual void FromJson(JSONObject json)
    {

    }

    #endregion
  }
}
