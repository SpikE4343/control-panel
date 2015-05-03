using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace ControlPanelPlugin
{
  public class ButtonItem : PanelItem
  {
    public Constants.Panel.SwitchId Switch;
    public bool State = false;



    public ButtonItem(Constants.Panel.SwitchId button, bool state)
    {
      Switch = button;
      this.State = state;

      // TODO: register on connection create
      //       Need connection established event on connection manager
      var connection = PanelManager.Instance.Connection;
      if (connection != null)
      {
        connection.RegisterHandler(SerialConnection.MsgType.GroupState, InputMessageHandler);
      }
      else
      {
        PanelManager.Instance.ConnectionSet += Instance_ConnectionSet;
      }

    }

    void Instance_ConnectionSet(SerialConnection connection)
    {
      connection.RegisterHandler(SerialConnection.MsgType.GroupState, InputMessageHandler);
    }

    protected void InputMessageHandler(SerialConnection.MsgType type, byte size, System.IO.BinaryReader stream)
    {
      if (type != SerialConnection.MsgType.GroupState)
        return;

      byte id = stream.ReadByte();
      byte state = stream.ReadByte();
      var switchId = (Constants.Panel.SwitchId)id;

      if (switchId != Switch)
        return;

      HandleSwitchStateMsg(switchId, state == 1);
    }

    protected virtual void HandleSwitchStateMsg(Constants.Panel.SwitchId switchId, bool state)
    {
      State = state;
    }

    public override bool Update()
    {
      return true;
    }
  }
}
