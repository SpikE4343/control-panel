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

namespace ControlPanelPlugin
{
  public class ControlPanel
  {
    byte[] buffer = new byte[2];
    bool running = true;

    Queue<string> LogLines = new Queue<string>();

    public IVessel CurrentVessel;

    

    public float InputUpdateInterval = 0.01f;
    public float PanelUpdateInterval = 0.0625f;

    static ControlPanel instance = null;

    public bool throttleEnabled = true;
    public bool stageArmed = false;
    public bool canFireStage = false;
    public float throttleValue = 0.0f;

    private bool HasOneHeartbeat = false;
    private bool registered = false;
    public int DeviceFrame = 0;

    public Constants.Panel.ViewMode viewMode = Constants.Panel.ViewMode.Staging;

    public List<PanelItem> PanelItems = new List<PanelItem>();

    public ControlPanel()
    {
      
    }

    public void Add(PanelItem item)
    {
      item.Panel = this;
      PanelItems.Add(item);
    }


    void AnalogInputHandler(SerialConnection.MsgType type, byte size, System.IO.BinaryReader stream)
    {
      byte id = stream.ReadByte();
      float input = stream.ReadSingle();
      //Log.Info("Msg: {0}, id: {1}, input: {2}\n", (int)type, id, input);

      if (id == 0)
      {
        throttleValue = input / 100.0f;
      }
    }

    protected void InputMessageHandler(SerialConnection.MsgType type, byte size, System.IO.BinaryReader stream)
    {
      //Log.Info("Msg type: {0}, size: {1}\n", type, size);
      switch (type)
      {
        case SerialConnection.MsgType.Heartbeat:
          HasOneHeartbeat = true;
          DeviceFrame = stream.ReadInt32();
          //Log.Info("[Device] HB: frame: {0}\n", frame);
          break;

        case SerialConnection.MsgType.GroupState:
          {
            byte id = stream.ReadByte();
            byte state = stream.ReadByte();
            //Log.Info("Msg: {0}, id: {1}, input: {2}\n", (int)type, id, state);
            var switchId = (Constants.Panel.SwitchId)id;

            HandleGroupState(switchId, state);
          }
          break;
        case SerialConnection.MsgType.LogInfo:
          {
            int len = stream.ReadInt16();
            string msg = new string(stream.ReadChars(len));
            Log.Info("msg: {0}\n", msg);
            System.Console.WriteLine(msg);

          }
          break;
      }
    }

    private void HandleGroupState(Constants.Panel.SwitchId switchId, byte state)
    {
      switch (switchId)
      {
        case Constants.Panel.SwitchId.SWITCH_SAS:
        case Constants.Panel.SwitchId.SWITCH_RCS:
        case Constants.Panel.SwitchId.SWITCH_GEAR:
        case Constants.Panel.SwitchId.SWITCH_LIGHTS:
        case Constants.Panel.SwitchId.SWITCH_BRAKES:
          CurrentVessel.setActionGroup(Constants.Panel.getActionGroupFromId((int)switchId), state == 1);
          break;


        case Constants.Panel.SwitchId.SWITCH_STAGE:
          if (stageArmed && canFireStage)
          {
            canFireStage = false;
            CurrentVessel.ActiveNextStage();
          }

          //sendState((int)Constants.Panel.StatusId.STAGE_ARMED, canFireStage);
          break;

        case Constants.Panel.SwitchId.SWITCH_STAGE_ARM:
          {
            break;
            bool last = stageArmed;
            stageArmed = state == 1;

            if (!canFireStage)
              canFireStage = stageArmed;

            if (last && !stageArmed && canFireStage)
              canFireStage = false;

            //sendState((int)Constants.Panel.StatusId.STAGE_ARMED, canFireStage);
          }
          break;

        case Constants.Panel.SwitchId.SWITCH_THROTTLE_MOMENT:
        case Constants.Panel.SwitchId.SWITCH_THROTTLE_TOGGLE:
          throttleEnabled = state == 1;
          break;

        case Constants.Panel.SwitchId.SWITCH_DOCKING_MODE:
          {
            bool wasDock = viewMode == Constants.Panel.ViewMode.Docking;
            viewMode = state == 1 ? Constants.Panel.ViewMode.Docking : Constants.Panel.ViewMode.Staging;
            bool isDock = viewMode == Constants.Panel.ViewMode.Docking;

            if (wasDock && !isDock)
            {
              CurrentVessel.ExitDockingMode();
            }
            else if (!wasDock && isDock)
            {
              CurrentVessel.EnterDockingMode();
            }
          }
          break;

        case Constants.Panel.SwitchId.SWITCH_MAP_MODE:
          {
            bool wasMap = viewMode == Constants.Panel.ViewMode.Map;
            viewMode = state == 1 ? Constants.Panel.ViewMode.Map : Constants.Panel.ViewMode.Staging;
            bool isMap = viewMode == Constants.Panel.ViewMode.Map;

            if (wasMap && !isMap)
            {
              CurrentVessel.ExitMapView();
            }
            else if (!wasMap && isMap)
            {
              CurrentVessel.EnterMapView();
            }
          }
          break;

        case Constants.Panel.SwitchId.SWITCH_TRANS_CTRL:
          break;

        case Constants.Panel.SwitchId.SWITCH_FINE_CTRL:
          break;
      }
    }

    public void Start()
    {
      if (!registered)
      {
        var connection = ConnectionManager.Instance.Connection;
        connection.RegisterHandler(SerialConnection.MsgType.AnalogInput, AnalogInputHandler);
        connection.RegisterHandler(SerialConnection.MsgType.GroupState, InputMessageHandler);
        connection.RegisterHandler(SerialConnection.MsgType.Heartbeat, InputMessageHandler);
        connection.RegisterHandler(SerialConnection.MsgType.LogInfo, InputMessageHandler);
        registered = true;
      }

      running = true;
      HasOneHeartbeat = false;
    }

    public void Stop()
    {
      if (!registered)
      {
        var connection = ConnectionManager.Instance.Connection;
        connection.UnregisterHandler(SerialConnection.MsgType.AnalogInput, AnalogInputHandler);
        connection.UnregisterHandler(SerialConnection.MsgType.GroupState, InputMessageHandler);
        connection.UnregisterHandler(SerialConnection.MsgType.Heartbeat, InputMessageHandler);
        connection.UnregisterHandler(SerialConnection.MsgType.LogInfo, InputMessageHandler);
        registered = false;
      }
      running = false;
      HasOneHeartbeat = false;
      ConnectionManager.Instance.Stop();
    }

    public void UpdateState()
    {
      if (CurrentVessel == null)
        return;

      if (!throttleEnabled)
      {
        CurrentVessel.mainThrottle = 0;
      }
      else
      {
        CurrentVessel.mainThrottle = throttleValue;
      }

      if (!ConnectionManager.Instance.Connection.IsOpen || !HasOneHeartbeat)
      {
        return;
      }

      foreach (var panelitem in PanelItems)
      {
        panelitem.Update();
      }
    }

    public void UpdateInput()
    {
      if (CurrentVessel == null)
        return;

      ConnectionManager.Instance.Update();
    }



    Vector2 scrollPos = new Vector2();
    public void OnGUI()
    {
      SerialConnection serial = ConnectionManager.Instance.Connection;
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
          PanelUpdateInterval = GUILayout.HorizontalSlider(PanelUpdateInterval, 0.01f, 1.0f, GUILayout.Width(100));
          GUILayout.Label("" + PanelUpdateInterval);
          GUILayout.EndHorizontal();

          GUILayout.BeginHorizontal();
          GUILayout.Label("Input Up: ");
          InputUpdateInterval = GUILayout.HorizontalSlider(InputUpdateInterval, 0.005f, 0.5f, GUILayout.Width(100));
          GUILayout.Label("" + InputUpdateInterval);
          GUILayout.EndHorizontal();

          if (GUILayout.Button("Disconnect"))
          {
            serial.Disconnect();
            serial.DesiredConnectionState = SerialConnection.ConnectionState.Disconnected;
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
            serial.DesiredConnectionState = SerialConnection.ConnectionState.Connected;
          }
        }
      }

      GUILayout.EndVertical();
      //GUILayout.EndScrollView();
    }

    class PanelSave
    {
      [JsonProperty("items")]
      public List<PanelItem> PanelItems;
    }

    public void Save(string file)
    {
      var output = new PanelSave();
      output.PanelItems = PanelItems;
      Persistence.Save(file, output);
    }

    public void Load(string file)
    {
      var input = Persistence.Load<PanelSave>(file);
      PanelItems = input.PanelItems;
    }
  }

}

