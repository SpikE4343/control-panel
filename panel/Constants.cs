using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace ControlPanelPlugin
{
  public class Constants
  {
    public class Panel
    {
      public enum StatusId
      {
        None,
        RCS = 0,
        SAS = 1,
        STAGE = 2,
        STAGE_ARMED = 3,
        THROTTLE = 4
      }

      public enum SwitchId
      {
        None = -1,
        Rcs = 0,
        Sas = 1,
        Stage = 2,
        StageArm = 3,
        DockingMode = 4,
        MapMode = 5,
        TrottleToggle = 6,
        Brakes = 7,
        Gear = 8,
        Lights = 9,
        TranslationCtrl = 10,
        FineCtrl = 11,
        ThrottleMoment = 12,

        Num
      }

      public enum ActionGroup
      {
        RCS,
        SAS,
        Stage,
        Light,
        Gear,
        Brakes,
        None,
      }

      public enum ViewMode
      {
        Map,
        Staging,
        Docking
      }

      public static int getIdForActionGroup(ActionGroup group)
      {
        switch (group)
        {
          case ActionGroup.RCS:
            return (int)Constants.Panel.SwitchId.Rcs;
          case ActionGroup.SAS:
            return (int)Constants.Panel.SwitchId.Sas;
          case ActionGroup.Stage:
            return (int)Constants.Panel.SwitchId.Stage;

          case ActionGroup.Light:
            return (int)Constants.Panel.SwitchId.Lights;
          case ActionGroup.Gear:
            return (int)Constants.Panel.SwitchId.Gear;
          case ActionGroup.Brakes:
            return (int)Constants.Panel.SwitchId.Brakes;
        }

        return -1;
      }



      public static ActionGroup getActionGroupFromId(int id)
      {
        var swid = (Constants.Panel.SwitchId)id;
        switch (swid)
        {
          case Constants.Panel.SwitchId.Rcs:
            return ActionGroup.RCS;

          case Constants.Panel.SwitchId.Sas:
            return ActionGroup.SAS;

          case Constants.Panel.SwitchId.Stage:
            return ActionGroup.Stage;

          case Constants.Panel.SwitchId.Lights:
            return ActionGroup.Light;

          case Constants.Panel.SwitchId.Gear:
            return ActionGroup.Gear;

          case Constants.Panel.SwitchId.Brakes:
            return ActionGroup.Brakes;
        }

        return ActionGroup.None;
      }
    }
  }
}

namespace panel
{

}
