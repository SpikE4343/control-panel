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
            return (int)Constants.Panel.SwitchId.Rcs;
          case KSPActionGroup.SAS:
            return (int)Constants.Panel.SwitchId.Sas;
          case KSPActionGroup.Stage:
            return (int)Constants.Panel.SwitchId.Stage;

          case KSPActionGroup.Light:
            return (int)Constants.Panel.SwitchId.Lights;
          case KSPActionGroup.Gear:
            return (int)Constants.Panel.SwitchId.Gear;
          case KSPActionGroup.Brakes:
            return (int)Constants.Panel.SwitchId.Brakes;
        }

        return -1;
      }

      public static KSPActionGroup getActionGroupFromId(int id)
      {
        var swid = (Constants.Panel.SwitchId)id;
        switch (swid)
        {
          case Constants.Panel.SwitchId.Rcs:
            return KSPActionGroup.RCS;

          case Constants.Panel.SwitchId.Sas:
            return KSPActionGroup.SAS;

          case Constants.Panel.SwitchId.Stage:
            return KSPActionGroup.Stage;

          case Constants.Panel.SwitchId.Lights:
            return KSPActionGroup.Light;

          case Constants.Panel.SwitchId.Gear:
            return KSPActionGroup.Gear;

          case Constants.Panel.SwitchId.Brakes:
            return KSPActionGroup.Brakes;
        }

        return KSPActionGroup.None;
      }
    }
  }
}
