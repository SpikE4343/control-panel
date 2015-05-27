
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ControlPanelPlugin.Items;
using ControlPanelPlugin.Utils;


namespace ControlPanelPlugin.Items.Button.Action
{
  [ClassSerializer("ThrottleToggleAction")]
  public class ThrottleToggleAction : ButtonAction
  {
    public override void StateChange()
    {
      Button.Panel.ThrottleEnabled = Button.State;
    }
  }
}
