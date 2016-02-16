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


namespace ControlPanelPlugin.Items.Status
{
  [ClassSerializer("StatusItem")]
  public class StatusItem : PanelItem
  {
    protected bool state = false;
    protected bool listening = false;

    private Constants.Panel.StatusId statusType = Constants.Panel.StatusId.None;
    public Constants.Panel.StatusId Status
    {
      get { return statusType; }
      set
      {
        if (value == statusType)
          return;

        statusType = value;
      }
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

        Send();
      }
    }

    public StatusItem()
    {

    }

    public StatusItem(Constants.Panel.StatusId type)
    {
      Status = type;
    }

    public void Send()
    {
      var msg = Singleton.Get<ObjectPool>().Grab<GroupStateMsg>();
      msg.id = (byte)Status;
      msg.state = (byte)(State ? 1 : 0);

      Singleton.Get<MessageManager>().WriteMsg(msg);
    }

    public override bool Update()
    {
      bool next = Panel.CurrentVessel.getActionGroup(
                      Constants.Panel.getActionGroupFromId((int)Status));

      if (State != next)
      {
        State = next;
        return true;
      }

      return false;
    }

    public override void Shutdown()
    {

    }

    public override JSONObject ToJson()
    {
      var json = base.ToJson();

      json["status"] = Enum.GetName(typeof(Constants.Panel.StatusId), Status);

      return json;
    }

    public override void FromJson(JSONObject json)
    {
      base.FromJson(json);
      Status = (Constants.Panel.StatusId)Enum.Parse(typeof(Constants.Panel.StatusId), json["status"]);

    }

    bool switchSelectionOpen = false;
    int switchSelection = 0;
    Vector2 switchScroll;
    ComboBox switchGUI = new ComboBox(Enum.GetNames(typeof(Constants.Panel.StatusId)));

    public override void OnGUI()
    {
      switchGUI.Selection = (int)Status;
      GUILayout.BeginHorizontal();

      if (switchGUI.Show())
      {
        switchSelectionOpen = false;
        Status = (Constants.Panel.StatusId)Enum.Parse(typeof(Constants.Panel.StatusId), switchGUI.SelectedItem);
      }

      //GUILayout.Button(Status.ToString(), GUILayout.Width(150));

      GUILayout.FlexibleSpace();
      State = GUILayout.Toggle(State, "");

      GUILayout.EndHorizontal();

    }
  }
}
