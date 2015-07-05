using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Boomlagoon.JSON;
using ControlPanelPlugin.Items;
using ControlPanelPlugin.Items.Button;
using ControlPanelPlugin.Utils;
using panel;


namespace ControlPanelPlugin.Items.Button.Action
{
  [ClassSerializer("ButtonStatusAction")]
  public class ButtonStatusAction : ButtonAction
  {
    public Constants.Panel.ActionGroup KspGroup = Constants.Panel.ActionGroup.None;

    public override string Name { get { return string.Format( "Status: {0}", KspGroup.ToString()); } }

    public ButtonStatusAction()
    {

    }

    public ButtonStatusAction(Constants.Panel.ActionGroup group)
    {
      KspGroup = group;
    }

    public override void StateChange()
    {
      bool state = Button.Panel.CurrentVessel.getActionGroup(KspGroup);
      Button.Panel.CurrentVessel.setActionGroup(KspGroup, Button.State);

      if (Button.State != state)
      {
        Button.Send();
      }
    }

    public override bool Update()
    {
      if (KspGroup == Constants.Panel.ActionGroup.None)
        return true;

      bool state = Button.Panel.CurrentVessel.getActionGroup(KspGroup);
      if (Button.State != state)
      {
        Button.State = state;
      }

      return true;
    }

    public override JSONObject ToJson()
    {
      var json = base.ToJson();

      json["group"] = Enum.GetName(typeof(Constants.Panel.ActionGroup), KspGroup);

      return json;
    }

    public override void FromJson(JSONObject json)
    {
      base.FromJson(json);

      KspGroup = (Constants.Panel.ActionGroup)Enum.Parse(typeof(Constants.Panel.ActionGroup), json["group"]);
    }
  }
}
