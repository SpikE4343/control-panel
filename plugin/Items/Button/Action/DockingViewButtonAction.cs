using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ControlPanelPlugin.Items;
using ControlPanelPlugin.Items.Button;
using ControlPanelPlugin.Items.Button.Action;
using ControlPanelPlugin.Utils;


namespace ControlPanelPlugin.Items.Button.Action
{
  [ClassSerializer("DockingViewButtonAction")]
  public class DockingViewButtonAction : ButtonAction
  {
    public override void StateChange()
    {
      bool wasDock = Button.Panel.viewMode == Constants.Panel.ViewMode.Docking;
      Button.Panel.viewMode = Button.State ? Constants.Panel.ViewMode.Docking : Constants.Panel.ViewMode.Staging;
      bool isDock = Button.Panel.viewMode == Constants.Panel.ViewMode.Docking;

      if (wasDock && !isDock)
      {
        Button.Panel.CurrentVessel.ExitDockingMode();
      }
      else if (!wasDock && isDock)
      {
        Button.Panel.CurrentVessel.EnterDockingMode();
      }
    }
  }
}
