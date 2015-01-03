using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlPanelPlugin
{
  public class DockingViewButtonItem : ButtonItem
  {
    public DockingViewButtonItem(bool state = false)
      : base(Constants.Panel.SwitchId.SWITCH_DOCKING_MODE, state)
    {

    }

    protected override void HandleSwitchStateMsg(Constants.Panel.SwitchId switchId, bool state)
    {
      bool wasDock = Panel.viewMode == Constants.Panel.ViewMode.Docking;
      Panel.viewMode = state ? Constants.Panel.ViewMode.Docking : Constants.Panel.ViewMode.Staging;
      bool isDock = Panel.viewMode == Constants.Panel.ViewMode.Docking;

      if (wasDock && !isDock)
      {
        Panel.CurrentVessel.ExitDockingMode();
      }
      else if (!wasDock && isDock)
      {
        Panel.CurrentVessel.EnterDockingMode();
      }
    }
  }
}
