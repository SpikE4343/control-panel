using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;



namespace ControlPanelPlugin
{
  public class MapViewButtonItem : ButtonItem
  {
    public MapViewButtonItem(bool state = false)
      : base(Constants.Panel.SwitchId.SWITCH_DOCKING_MODE, state)
    {

    }

    protected override void HandleSwitchStateMsg(Constants.Panel.SwitchId switchId, bool state)
    {
      bool wasMap = Panel.viewMode == Constants.Panel.ViewMode.Map;
      Panel.viewMode = state ? Constants.Panel.ViewMode.Map : Constants.Panel.ViewMode.Staging;
      bool isMap = Panel.viewMode == Constants.Panel.ViewMode.Map;

      if (wasMap && !isMap)
      {
        Panel.CurrentVessel.ExitMapView();
      }
      else if (!wasMap && isMap)
      {
        Panel.CurrentVessel.EnterMapView();
      }
    }
  }
}
