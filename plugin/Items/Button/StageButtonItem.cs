
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace ControlPanelPlugin
{
  public class StageButtonItem : ButtonItem
  {
    public StageButtonItem(bool state = false)
      : base(Constants.Panel.SwitchId.SWITCH_STAGE, state)
    {

    }

    protected override void HandleSwitchStateMsg(Constants.Panel.SwitchId switchId, bool state)
    {
      if (Panel.stageArmed && Panel.canFireStage)
      {
        Panel.canFireStage = false;
        Panel.CurrentVessel.ActiveNextStage();
      }

      SendState((int)Constants.Panel.StatusId.STAGE_ARMED, Panel.canFireStage);
    }
  }
}
