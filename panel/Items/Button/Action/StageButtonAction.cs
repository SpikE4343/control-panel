
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
      if (Button.Panel.StageArmed && Button.Panel.CanFireStage)
      {
        //Button.Panel.CanFireStage = false;
        Button.Panel.CurrentVessel.ActiveNextStage();
      }

      //Button.State = Button.Panel.CanFireStage;
    }
  }
}
