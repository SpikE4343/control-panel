using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//using System.IO.Ports;
using System;
using System.Threading;
using KSP;

using OpenNETCF.IO.Ports;
using ControlPanelPlugin.Telemetry;
using System.Xml.Serialization;
using Newtonsoft.Json;
using ControlPanelPlugin.Utils;
using ControlPanelPlugin.Messages;
using Boomlagoon.JSON;

// copy /Y $(TargetPath) "C:\Program Files (x86)\Steam\steamapps\common\Kerbal Space Program\GameData\Controlpanel\Plugins\$(TargetFileName)"

namespace ControlPanelPlugin
{
  public class ControlPanel : IJsonConvertable
  {
    bool running = true;

    public IVessel CurrentVessel { get; set; }

    public bool throttleEnabled = true;
    public bool stageArmed = false;
    public bool canFireStage = false;
    public float throttleValue = 0.0f;

    private bool HasOneHeartbeat = false;
    private bool registered = false;
    public int DeviceFrame = 0;

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
        throttleValue = msg.value / 100.0f;
      }
    }

    #endregion

    public void Add(PanelItem item)
    {
      item.Panel = this;
      PanelItems.Add(item);
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

      CurrentVessel.mainThrottle = throttleEnabled ? throttleValue : 0;

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



    Vector2 scrollPos = new Vector2();
    public void OnGUI()
    {
      Connection serial = Singleton.Get<Connection>();
      var config = Singleton.Get<Config>();
      //scrollPos = GUILayout.BeginScrollView(scrollPos);
      GUILayout.BeginVertical("box");

      GUILayout.Toggle(HasOneHeartbeat, "Has One HB");

      if (serial != null)
      {
        if (serial.Connected)
        {
          GUILayout.Label("T: " + throttleValue);

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

          if (CurrentVessel != null)
          {
            GUILayout.BeginHorizontal();
            GUILayout.Label(CurrentVessel.Name);
            GUILayout.EndHorizontal();

            GUILayout.Label(string.Format("Lq: {0:0.00}%", CurrentVessel.liquidResourcePercent));
            GUILayout.Label(string.Format("Ox: {0:0.00}%", CurrentVessel.oxiResourcePercent));
            GUILayout.Label(string.Format("Mo: {0:0.00}%", CurrentVessel.monoResourcePercent));
            GUILayout.Label(string.Format("Ev: {0:0.00}%", CurrentVessel.electricResourcePercent));

            GUILayout.Label(string.Format("NN: {0:00.00}", CurrentVessel.nextNodeSeconds));

            GUILayout.BeginVertical();
            GUILayout.Label("Telemetry items");

            foreach (var item in PanelItems)
            {
              item.OnGUI();
            }
            GUILayout.EndVertical();
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
      //GUILayout.EndScrollView();
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
        PanelItems.Add(Singleton.Get<ClassSerializer>().FromJson<PanelItem>(item));
      }
    }

    #endregion
  }

}

