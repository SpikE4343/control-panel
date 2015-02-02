using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ControlPanelPlugin
{
  public abstract class PanelItem
  {
    private static byte[] buffer = new byte[2];

    [JsonIgnore]
    public virtual ControlPanel Panel { get; set; }
    public abstract bool Update();

    protected void SendState(int id, bool state)
    {
      buffer[0] = (byte)id;
      buffer[1] = (byte)(state ? 1 : 0);
      var serial = PanelManager.Instance.Connection;
      serial.SendMessage(SerialConnection.MsgType.GroupState, buffer);
    }

    public virtual void Send()
    {

    }

    public virtual void OnGUI()
    {

    }
  }
}
