using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        None,
        SWITCH_RCS = 0,
        SWITCH_SAS = 1,
        SWITCH_STAGE = 2,
        SWITCH_STAGE_ARM = 3,
        SWITCH_DOCKING_MODE = 4,
        SWITCH_MAP_MODE = 5,
        SWITCH_THROTTLE_TOGGLE = 6,
        SWITCH_BRAKES = 7,
        SWITCH_GEAR = 8,
        SWITCH_LIGHTS = 9,
        SWITCH_TRANS_CTRL = 10,
        SWITCH_FINE_CTRL = 11,
        SWITCH_THROTTLE_MOMENT = 12,

        NUM_SWITCH
      }

      public enum ViewMode
      {
        Map,
        Staging,
        Docking
      }

      public static int getIdForActionGroup(KSPActionGroup group)
      {
        switch (group)
        {
          case KSPActionGroup.RCS:
            return (int)Constants.Panel.SwitchId.SWITCH_RCS;
          case KSPActionGroup.SAS:
            return (int)Constants.Panel.SwitchId.SWITCH_SAS;
          case KSPActionGroup.Stage:
            return (int)Constants.Panel.SwitchId.SWITCH_STAGE;

          case KSPActionGroup.Light:
            return (int)Constants.Panel.SwitchId.SWITCH_LIGHTS;
          case KSPActionGroup.Gear:
            return (int)Constants.Panel.SwitchId.SWITCH_GEAR;
          case KSPActionGroup.Brakes:
            return (int)Constants.Panel.SwitchId.SWITCH_BRAKES;
        }

        return -1;
      }

      public static KSPActionGroup getActionGroupFromId(int id)
      {
        var swid = (Constants.Panel.SwitchId)id;
        switch (swid)
        {
          case Constants.Panel.SwitchId.SWITCH_RCS:
            return KSPActionGroup.RCS;

          case Constants.Panel.SwitchId.SWITCH_SAS:
            return KSPActionGroup.SAS;

          case Constants.Panel.SwitchId.SWITCH_STAGE:
            return KSPActionGroup.Stage;

          case Constants.Panel.SwitchId.SWITCH_LIGHTS:
            return KSPActionGroup.Light;

          case Constants.Panel.SwitchId.SWITCH_GEAR:
            return KSPActionGroup.Gear;

          case Constants.Panel.SwitchId.SWITCH_BRAKES:
            return KSPActionGroup.Brakes;
        }

        return KSPActionGroup.None;
      }
    }
  }
}
