using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ControlPanelPlugin.Items;
using ControlPanelPlugin.Utils;



namespace ControlPanelPlugin.Items.Button.Action
{
  [ClassSerializer("StageArmButtonAction")]
  public class StageArmButtonAction : ButtonAction
  {
    public override void StateChange()
    {
      bool last = Button.Panel.stageArmed;
      Button.Panel.stageArmed = Button.State;

      if (!Button.Panel.canFireStage)
        Button.Panel.canFireStage = Button.Panel.stageArmed;

      if (last && !Button.Panel.stageArmed && Button.Panel.canFireStage)
        Button.Panel.canFireStage = false;

      Button.State = Button.Panel.canFireStage;
    }
  }
}
