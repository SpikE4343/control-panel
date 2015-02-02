using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace ControlPanelPlugin
{
  public class ButtonStatusItem : ButtonItem
  {
    public Constants.Panel.SwitchId Switch = Constants.Panel.SwitchId.None;
    public Constants.Panel.StatusId Status = Constants.Panel.StatusId.None;
    public KSPActionGroup KspGroup = KSPActionGroup.None;
    public bool State = false;

    private static byte[] buffer = new byte[2];

    public ButtonStatusItem( Constants.Panel.SwitchId button, KSPActionGroup group, bool state)
      : base( button, state )
    {
      KspGroup = group;
    }

    protected override void HandleSwitchStateMsg(Constants.Panel.SwitchId switchId, bool state)
    {
      base.HandleSwitchStateMsg(switchId, state);

      if (KspGroup != KSPActionGroup.None)
      {
        Panel.CurrentVessel.setActionGroup(KspGroup, state);
      }

      if (Status != Constants.Panel.StatusId.None)
      {
        SendState((int)Status, state);
      }
    }

    public override bool Update()
    {
      if (KspGroup == KSPActionGroup.None)
        return true;

      bool state = Panel.CurrentVessel.getActionGroup(KspGroup);
      if (State != state)
      {
        State = state;
        SendState((int)Switch, State);
        Log.Debug("[ControlPanel] id={0}, state={1}", (int)Switch, State);
      }

      return true;
    }

    private void SendState(int id, bool state)
    {
      buffer[0] = (byte)id;
      buffer[1] = (byte)(state ? 1 : 0);
      var serial = PanelManager.Instance.Connection;
      serial.SendMessage(SerialConnection.MsgType.GroupState, buffer);
    }
  }
}
