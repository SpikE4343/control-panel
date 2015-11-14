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
using ControlPanelPlugin.Utils.Gui;
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
        if (value == switchType)
          return;

        RemoveMessageHandler();
        switchType = value;
        AddMessageHandler();
      }
    }
    private ButtonAction action;

    private void AddMessageHandler()
    {
      if (listening)
        return;

      listening = true;
      Singleton.Get<InputDispatcher>().GroupStateHandler(Switch).OnEvent += HandleMessage;
    }

    private void RemoveMessageHandler()
    {
      if (!listening)
        return;

      Singleton.Get<InputDispatcher>().GroupStateHandler(Switch).OnEvent -= HandleMessage;
    }

    public bool State
    {
      get { return state; }
      set
      {
        bool changed = state != value;
        state = value;

        if (!changed)
          return;

        if (Action != null)
          Action.StateChange();

        Send();
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
        if (Action != null)
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
      if (action == null)
        return true;

      return action.Update();
    }

    public override void Shutdown()
    {
      RemoveMessageHandler();
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
    int switchSelection = 0;
    Vector2 switchScroll;
    ComboBox switchGUI = new ComboBox(Enum.GetNames(typeof(Constants.Panel.SwitchId)));

    public override void OnGUI()
    {
      switchGUI.Selection = (int)Switch;
      GUILayout.BeginHorizontal();

      if (switchGUI.Show())
      {
        switchSelectionOpen = false;
        Switch = (Constants.Panel.SwitchId)Enum.Parse(typeof(Constants.Panel.SwitchId), switchGUI.SelectedItem);
      }

      GUILayout.Button(Action != null ? Action.Name : "Action", GUILayout.Width(150));

      GUILayout.FlexibleSpace();
      State = GUILayout.Toggle(State, "");
      //GUILayout.Label(State ? "on" : "off");

      GUILayout.EndHorizontal();

    }
  }
}
