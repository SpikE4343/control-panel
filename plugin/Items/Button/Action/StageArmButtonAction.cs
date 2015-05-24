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
      bool last = Button.Panel.StageArmed;
      Button.Panel.StageArmed = Button.State;

      if (!Button.Panel.CanFireStage)
        Button.Panel.CanFireStage = Button.Panel.StageArmed;

      if (last && !Button.Panel.StageArmed && Button.Panel.CanFireStage)
        Button.Panel.CanFireStage = false;

      Button.State = Button.Panel.CanFireStage;
    }
  }
}
