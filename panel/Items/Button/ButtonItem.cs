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
using UnityEngine;


namespace ControlPanelPlugin.Items.Button
{
  [ClassSerializer("ButtonItem")]
  public class ButtonItem : PanelItem
  {
    protected bool state = false;
    protected bool listening = false;

    private Constants.Panel.SwitchId switchType = Constants.Panel.SwitchId.None;
    public Constants.Panel.SwitchId Switch
    {
      get { return switchType; }
      set
      {
        if (value != switchType)
        {
          if (listening)
          {
            Singleton.Get<InputDispatcher>().GroupStateHandler(Switch).OnEvent -= HandleMessage;
          }

          switchType = value;
          listening = true;
          Singleton.Get<InputDispatcher>().GroupStateHandler(Switch).OnEvent += HandleMessage;
        }
      }
    }
    private ButtonAction action;

    public bool State
    {
      get { return state; }
      set
      {
        bool changed = state != value;
        state = value;

        if (changed)
        {
          Action.StateChange();
          Send();
        }
      }
    }

    public ButtonItem()
    {

    }

    public ButtonItem(Constants.Panel.SwitchId type, ButtonAction action)
    {
      Action = action;
      Switch = type;
    }

    public ButtonAction Action
    {
      get { return action; }
      set { action = value; action.Button = this; }
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

    bool switchSelectionOpen = false;
    public override void OnGUI()
    {
      GUILayout.BeginHorizontal();

      State = GUILayout.Toggle(State, "");

      bool pressed = GUILayout.Button(Switch.ToString());
      if (switchSelectionOpen || pressed)
      {
        if (switchSelectionOpen && pressed)
        {
          switchSelectionOpen = false;
        }
        else
        {
          switchSelectionOpen = true;
          GUILayout.BeginVertical("box");
          var names = Enum.GetNames(typeof(Constants.Panel.SwitchId));
          foreach (var name in names)
          {
            if (name == Switch.ToString())
              continue;

            if (GUILayout.Button(name))
            {
              switchSelectionOpen = false;
              Switch = (Constants.Panel.SwitchId)Enum.Parse(typeof(Constants.Panel.SwitchId), name);
            }
          }
          GUILayout.EndVertical();
        }
      }


      //GUILayout.Label(State ? "on" : "off");

      GUILayout.EndHorizontal();

    }
  }
}
