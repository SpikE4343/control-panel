
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
    public override void StateChange()
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
