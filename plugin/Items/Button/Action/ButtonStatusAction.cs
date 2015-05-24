using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Boomlagoon.JSON;
using ControlPanelPlugin.Items;
using ControlPanelPlugin.Items.Button;
using ControlPanelPlugin.Utils;


namespace ControlPanelPlugin.Items.Button.Action
{
  [ClassSerializer("ButtonStatusAction")]
  public class ButtonStatusAction : ButtonAction
  {
    public KSPActionGroup KspGroup = KSPActionGroup.None;

    public ButtonStatusAction()
    {

    }

    public ButtonStatusAction(KSPActionGroup group)
    {
      KspGroup = group;
    }

    public override void StateChange()
    {
      Button.Panel.CurrentVessel.setActionGroup(KspGroup, Button.State);
    }

    public override bool Update()
    {
      if (KspGroup == KSPActionGroup.None)
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

      json["group"] = Enum.GetName(typeof(KSPActionGroup), KspGroup);

      return json;
    }

    public override void FromJson(JSONObject json)
    {
      base.FromJson(json);

      KspGroup = (KSPActionGroup)Enum.Parse(typeof(KSPActionGroup), json["group"]);
    }
  }
}
