using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//using System.IO.Ports;
using System;
using System.Threading;

using OpenNETCF.IO.Ports;
using ControlPanelPlugin.Telemetry;
using System.Xml.Serialization;
using ControlPanelPlugin.Utils;
using ControlPanelPlugin.Messages;
using Boomlagoon.JSON;
using ControlPanelPlugin.Items.Button;

// copy /Y $(TargetPath) "C:\Program Files (x86)\Steam\steamapps\common\Kerbal Space Program\GameData\Controlpanel\Plugins\$(TargetFileName)"

namespace ControlPanelPlugin
{
  public class ControlPanel : IJsonConvertable
  {
    bool running = true;

    public IVessel CurrentVessel { get { return Singleton.Get<IVessel>(); } }

    public bool ThrottleEnabled { get; set; }
    public bool StageArmed { get; set; }
    public bool CanFireStage { get; set; }
    public float ThrottleValue { get; set; }

    private bool HasOneHeartbeat { get; set; }
    private bool registered = false;
    public int DeviceFrame { get; set; }

    public int BytesToWrite { get { return Singleton.Get<Connection>().BytesToWrite; } }
    public int BytesToRead { get { return Singleton.Get<Connection>().BytesToRead; } }

    public Constants.Panel.ViewMode viewMode { get; set; }
    public List<PanelItem> PanelItems { get; set; }

    public ControlPanel()
    {
      PanelItems = new List<PanelItem>();
      viewMode = Constants.Panel.ViewMode.Staging;

      Singleton.Get<EventDispatcher>().Handler<LogInfoMsg>().OnEvent += OnLogInfoMsg;
      Singleton.Get<EventDispatcher>().Handler<HeartbeatMsg>().OnEvent += OnHeartbeatMsg;
      Singleton.Get<EventDispatcher>().Handler<AnalogInputMsg>().OnEvent += OnAnalogInputMsg;
    }

    #region Message Handlers

    private void OnLogInfoMsg(LogInfoMsg msg)
    {
      Log.Info("msg: {0}\n", msg.message);
    }

    private void OnHeartbeatMsg(HeartbeatMsg msg)
    {
      HasOneHeartbeat = true;
      DeviceFrame = msg.frame;
    }

    private void OnAnalogInputMsg(AnalogInputMsg msg)
    {
      if (msg.id == 0)
      {
        ThrottleValue = msg.value / 100.0f;
      }
    }

    #endregion

    public void Add(PanelItem item)
    {
      item.Panel = this;
      PanelItems.Add(item);
      item.Initialize();
    }


    public void Start()
    {
      running = true;
      HasOneHeartbeat = false;
    }

    public void Stop()
    {
      running = false;
      HasOneHeartbeat = false;
    }

    public void UpdateState()
    {
      if (CurrentVessel == null)
        return;

      CurrentVessel.mainThrottle = ThrottleEnabled ? ThrottleValue : 0;

      var connection = Singleton.Get<Connection>();
      if (connection == null || !connection.Connected || !HasOneHeartbeat)
      {
        return;
      }

      foreach (var panelitem in PanelItems)
      {
        panelitem.Update();
      }
    }

    bool wasConnected = false;
    public bool ResendAll { get; set; }
    public void UpdateInput()
    {
      ResendAll = false;
      if (CurrentVessel == null)
        return;

      bool connected = false;
      var connection = Singleton.Get<Connection>();
      if (connection != null)
      {
        connected = connection.Connected;
      }

      if (!wasConnected && connected)
      {
        ResendAll = true;
      }

      wasConnected = connected;
    }

    Rect windowPos = new Rect(20, 20, 200, 180);
    Vector2 scrollPos = new Vector2();
    public void OnGUI()
    {
      windowPos = GUILayout.Window(12053, windowPos,
                                      OnWindowGUI,
                                      "Control Panel",
                                      GUILayout.Width(200),
                                      GUILayout.Height(180),
                                      GUILayout.ExpandHeight(true),
                                      GUILayout.ExpandWidth(true));
    }



    bool editItems = false;
    string fileName = "controlpanel";
    string newPanelItem = "Telemetry";
    bool pickNewItemOpen = false;

    string[] panelItemTypes = new string[] { "Telemetry", "Button" };

    void OnWindowGUI(int windowid)
    {
      var serial = Singleton.Get<Connection>();
      var config = Singleton.Get<Config>();

      GUILayout.BeginVertical("box");
      if (Singleton.Get<IVessel>() != null)
      {
        GUILayout.BeginHorizontal();
        GUILayout.Label(string.Format("Vessel: {0}", Singleton.Get<IVessel>().Name));
        GUILayout.EndHorizontal();
      }
      GUILayout.EndVertical();

      GUILayout.BeginVertical("box");
      if (serial != null)
      {
        GUILayout.BeginHorizontal();
        GUILayout.Label(string.Format("Cs: {0}", serial.CurrentConnectionState));
        GUILayout.Label(string.Format("Ds: {0}", serial.DesiredConnectionState));
        GUILayout.EndHorizontal();

        if (serial.Connected)
        {
          GUILayout.BeginHorizontal();
          GUILayout.Label(string.Format("TxB: {0}", serial.BytesToWrite));
          GUILayout.Label(string.Format("RxB: {0}", serial.BytesToRead));
          GUILayout.EndHorizontal();
        }

        serial.COM = UnityUtils.GUIStringField("COM", serial.COM);
        serial.Baud = UnityUtils.GUIIntField("Baud", serial.Baud);
      }

      GUILayout.Label((serial != null && serial.Connected) ? "Connected" : "Disconnected");


      //scrollPos = GUILayout.BeginScrollView(scrollPos);
      //GUILayout.BeginVertical("box");

      GUILayout.Toggle(HasOneHeartbeat, "Has One HB");

      if (serial != null)
      {
        if (serial.Connected)
        {
          GUILayout.Label("T: " + ThrottleValue);

          GUILayout.BeginHorizontal();
          GUILayout.Label("Panel Up: ");
          config.Intervals.PanelUpdate = GUILayout.HorizontalSlider(config.Intervals.PanelUpdate, 0.01f, 1.0f, GUILayout.Width(100));
          GUILayout.Label("" + config.Intervals.PanelUpdate);
          GUILayout.EndHorizontal();

          GUILayout.BeginHorizontal();
          GUILayout.Label("Input Up: ");
          config.Intervals.InputUpdate = GUILayout.HorizontalSlider(config.Intervals.InputUpdate, 0.005f, 0.5f, GUILayout.Width(100));
          GUILayout.Label("" + config.Intervals.InputUpdate);
          GUILayout.EndHorizontal();

          if (GUILayout.Button("Disconnect"))
          {
            serial.Disconnect();
            serial.DesiredConnectionState = Connection.State.Disconnected;
            HasOneHeartbeat = false;
          }
        }
        else
        {
          if (GUILayout.Button("Connect"))
          {
            serial.DesiredConnectionState = Connection.State.Connected;
          }
        }
      }

      GUILayout.EndVertical();
      GUILayout.BeginVertical("box");

      fileName = GUILayout.TextField(fileName);
      GUILayout.BeginHorizontal();

      if (GUILayout.Button("Load"))
      {
        Singleton.Get<Application>().Load(fileName + ".json");
      }

      if (GUILayout.Button("Save"))
      {
        Singleton.Get<Application>().Save(fileName + ".json");
      }

      GUILayout.EndHorizontal();

      if (GUILayout.Button("Edit Items"))
      {
        editItems = !editItems;
      }

      GUILayout.EndVertical();



      if (editItems)
      {
        GUILayout.BeginVertical("box");

        scrollPos = GUILayout.BeginScrollView(scrollPos, GUILayout.Height(300), GUILayout.Width(400));
        GUILayout.BeginVertical();
        GUILayout.Label("Telemetry items");

        GUILayout.BeginHorizontal();

        if (GUILayout.Button(newPanelItem))
        {
          pickNewItemOpen = !pickNewItemOpen;
        }

        if (GUILayout.Button("+"))
        {
          if (newPanelItem == "Telemetry")
          {
            Add(new TelemetryItem());
          }
          else if (newPanelItem == "Button")
          {
            Add(new ButtonItem());
          }

        }
        GUILayout.EndHorizontal();

        if (pickNewItemOpen)
        {
          foreach (var name in panelItemTypes)
          {
            if (GUILayout.Button(name))
            {
              pickNewItemOpen = false;
              newPanelItem = name;
            }
          }
        }

        foreach (var item in PanelItems)
        {
          GUILayout.BeginVertical("box");
          item.OnGUI();
          GUILayout.EndVertical();
        }

        GUILayout.EndVertical();
        GUILayout.EndScrollView();
        GUILayout.EndVertical();
      }


      //GUILayout.EndScrollView();

      GUI.DragWindow(new Rect(0, 0, 10000, 10000));
    }

    #region IJsonConvertable Members

    public JSONObject ToJson()
    {
      var json = new JSONObject();

      var itemsJson = new JSONArray();
      foreach (var item in PanelItems)
      {
        itemsJson.Add(Singleton.Get<ClassSerializer>().ToJson(item));
      }

      json.Add("panelItems", itemsJson);

      return json;
    }

    public void FromJson(JSONObject json)
    {
      JSONArray itemsJson = json["panelItems"];
      foreach (var item in itemsJson)
      {
        Add(Singleton.Get<ClassSerializer>().FromJson<PanelItem>(item));
      }
    }

    #endregion
  }

}

