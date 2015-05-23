using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ControlPanelPlugin.Utils;

namespace ControlPanelPlugin.Items.Button.Action
{
  public abstract class ButtonAction : IJsonConvertable
  {
    public ButtonItem Button;
    public abstract void StateChange();
    public virtual bool Update()
    {
      return true;
    }

    #region IJsonConvertable Members

    public virtual Dictionary<string, object> ToJson()
    {
      return new Dictionary<string, object>();
    }

    public virtual void FromJson(Dictionary<string, object> json)
    {

    }

    #endregion
  }
}
