using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Boomlagoon.JSON;
using ControlPanelPlugin.Items;
using ControlPanelPlugin.Items.Button;
using ControlPanelPlugin.Items.Button.Action;
using ControlPanelPlugin.Messages;
using ControlPanelPlugin.Network;
using ControlPanelPlugin.Utils;


namespace ControlPanelPlugin.Items.Button
{
  [ClassSerializer("ButtonItem")]
  public class ButtonItem : PanelItem
  {
    protected bool state = false;

    public Constants.Panel.SwitchId Switch;
    private ButtonAction action;

    public bool State
    {
      get { return state; }
      set { state = value; Send(); }
    }

    public ButtonItem()
    {

    }

    public ButtonItem(ButtonAction action)
    {
      Action = action;
    }

    public ButtonAction Action
    {
      get { return action; }
      set { action = value; action.Button = this; }
    }

    public override void Initialize()
    {
      Singleton.Get<InputDispatcher>().GroupStateHandler(Switch).OnEvent += HandleMessage;
    }

    protected void HandleMessage(Messages.GroupStateMsg msg)
    {
      bool v = msg.state == 1;
      if (v != state)
      {
        state = v;
        Action.StateChange();
      }
    }

    public void Send()
    {
      var msg = Singleton.Get<ObjectPool>().Grab<GroupStateMsg>();
      msg.id = (byte)Switch;
      msg.state = (byte)(State ? 1 : 0);

      Singleton.Get<MessageManager>().WriteMsg(msg);
    }

    public override bool Update()
    {
      return action.Update();
    }

    public override JSONObject ToJson()
    {
      var json = base.ToJson();

      json["switch"] = Enum.GetName(typeof(Constants.Panel.SwitchId), Switch);
      json.Add("action", Singleton.Get<ClassSerializer>().ToJson(Action));

      return json;
    }

    public override void FromJson(JSONObject json)
    {
      base.FromJson(json);
      Switch = (Constants.Panel.SwitchId)Enum.Parse(typeof(Constants.Panel.SwitchId), json["switch"]);
      Action = Singleton.Get<ClassSerializer>().FromJson<ButtonAction>(json["action"]);

    }
  }
}
