using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ControlPanelPlugin.Items;
using ControlPanelPlugin.Items.Button.Action;
using ControlPanelPlugin.Utils;



namespace ControlPanelPlugin.Items.Button.Action
{
  [ClassSerializer("MapViewButtonAction")]
  public class MapViewButtonAction : ButtonAction
  {
    public override void StateChange()
    {
      bool wasMap = Button.Panel.viewMode == Constants.Panel.ViewMode.Map;
      Button.Panel.viewMode = Button.State ? Constants.Panel.ViewMode.Map : Constants.Panel.ViewMode.Staging;
      bool isMap = Button.Panel.viewMode == Constants.Panel.ViewMode.Map;

      if (wasMap && !isMap)
      {
        Button.Panel.CurrentVessel.ExitMapView();
      }
      else if (!wasMap && isMap)
      {
        Button.Panel.CurrentVessel.EnterMapView();
      }
    }
  }
}
