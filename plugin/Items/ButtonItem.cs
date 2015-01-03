using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

      var connection = ConnectionManager.Instance.Connection;
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
