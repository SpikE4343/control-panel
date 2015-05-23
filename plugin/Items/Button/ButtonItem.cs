﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

    protected void Send()
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

    public override Dictionary<string, object> ToJson()
    {
      var json = base.ToJson();

      json.Add("action", Singleton.Get<ClassSerializer>().ToJson(Action));

      return json;
    }

    public override void FromJson(Dictionary<string, object> json)
    {
      base.FromJson(json);

      Action = Singleton.Get<ClassSerializer>().FromJson<ButtonAction>(json["action"] as Dictionary<string, object>);

    }
  }
}
