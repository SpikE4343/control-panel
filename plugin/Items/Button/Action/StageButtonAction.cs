
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ControlPanelPlugin.Items;
using ControlPanelPlugin.Utils;


namespace ControlPanelPlugin.Items.Button.Action
{
  [ClassSerializer("StageButtonAction")]
  public class StageButtonAction : ButtonAction
  {
    protected override void HandleSwitchStateMsg(Constants.Panel.SwitchId switchId, bool state)
    {
      if (Button.Panel.stageArmed && Button.Panel.canFireStage)
      {
        Button.Panel.canFireStage = false;
        Button.Panel.CurrentVessel.ActiveNextStage();
      }

      Button.State = Button.Panel.canFireStage;
    }
  }
}
