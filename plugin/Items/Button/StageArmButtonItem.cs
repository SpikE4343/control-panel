using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ControlPanelPlugin
{
  public class StageArmButtonItem : ButtonItem
  {
    public StageArmButtonItem(bool state = false)
      : base(Constants.Panel.SwitchId.SWITCH_STAGE_ARM, state)
    {

    }

    protected override void HandleSwitchStateMsg(Constants.Panel.SwitchId switchId, bool state)
    {
      bool last = Panel.stageArmed;
      Panel.stageArmed = state;

      if (!Panel.canFireStage)
        Panel.canFireStage = Panel.stageArmed;

      if (last && !Panel.stageArmed && Panel.canFireStage)
        Panel.canFireStage = false;

      SendState((int)Constants.Panel.StatusId.STAGE_ARMED, Panel.canFireStage);
    }
  }
}
