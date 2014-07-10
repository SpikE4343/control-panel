using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//using System.IO.Ports;
using System;
using System.Threading;
using KSP;

using OpenNETCF.IO.Ports;

namespace ControlPanelPlugin
{
  public class ControlPanel
  {
    byte[] buffer = new byte[2];
    bool running = true;
    Queue<string> LogLines = new Queue<string>();

    public IVessel CurrentVessel;

    private List<TelemetryItem> Telemetry = new List<TelemetryItem>();

    private class StatusItem
    {
      public StatusItem(KSPActionGroup group, bool state)
      {
        kspGroup = group;
        this.state = state;
      }

      public string name;
      public int id { get { return getIdForActionGroup(kspGroup); } }
      public KSPActionGroup kspGroup = KSPActionGroup.None;
      public bool state = false;
    }

    public abstract class TelemetryItem
    {
      public int Id=-1;
      public float Value;

      public TelemetryItem( int id )
      {
        Id = id;
      }

      public abstract bool Update(IVessel vessel);
 
    }

    #region Analog Telemetry Percent
    public abstract class AnalogTelemetryPercentItem : TelemetryItem
    {
        private byte mMeter = 255;
        private byte mValue = 0;
        public AnalogTelemetryPercentItem(int id, int meter)
            : base(id)
        {
            mMeter = (byte)meter;
        }

        protected bool UpdateValue(double value)
        {
            byte val = (byte)((value / 100.0f) * 255.0);
            bool same = val == mValue;
            mValue = val;
            return same;
        }

        public void Send()
        {
            ConnectionManager.Instance.Connection.SendAnalogMeterMsg(mMeter, mValue); 
        }
    }

    public class LiquidFuelItem : AnalogTelemetryPercentItem
    {
        public LiquidFuelItem(int id, int meter)
            : base(id, meter)
        {
        }

        public override bool Update(IVessel vessel)
        {
            if (!UpdateValue(vessel.liquidFuelPercent))
            {
                Send();
                return true;
            }

            return false;
        }
    }
    public class OxiFuelItem : AnalogTelemetryPercentItem
    {
      public OxiFuelItem(int id, int meter)
        : base(id, meter)
      {
      }

      public override bool Update(IVessel vessel)
      {
        if (!UpdateValue(vessel.oxiFuelPercent))
        {
          Send();
          return true;
        }

        return false;
      }
    }
    public class MonoFuelItem : AnalogTelemetryPercentItem
    {
      public MonoFuelItem(int id, int meter)
        : base(id, meter)
      {
      }

      public override bool Update(IVessel vessel)
      {
        if (!UpdateValue(vessel.monoFuelPercent))
        {
          Send();
          return true;
        }

        return false;
      }
    }
    public class EvFuelItem : AnalogTelemetryPercentItem
    {
      public EvFuelItem(int id, int meter)
        : base(id, meter)
      {
      }

      public override bool Update(IVessel vessel)
      {
        if (!UpdateValue(vessel.electricFuelPercent))
        {
          Send();
          return true;
        }

        return false;
      }
    }
    #endregion

    public float InputUpdateInterval = 0.01f;
    public float PanelUpdateInterval = 0.0625f;

    public class AltitudeTelemetryItem : TelemetryItem
    {
      public AltitudeTelemetryItem(int id) : base(id)
      {

      }

      public override bool Update(IVessel vessel)
      {
        float alt = vessel.altitude;
        int precision = 2;

        if (alt >= 10000000)
        {
            precision = 0;
        }
        else if (alt >= 1000000)
        {
            precision = 1;
        }

        if (alt != Value)
        {
          Value = alt;
          
          ConnectionManager.Instance.Connection.SendTelemetryMessage(0, 0, 0, 8, precision, Value);
          return true;
        }

        return false;
      }

    }

    public class SpeedTelemetryItem : TelemetryItem
    {
      public SpeedTelemetryItem(int id)
        : base(id)
      {

      }

      public override bool Update(IVessel vessel)
      {
        float speed = vessel.speed;
        int precision = 3;

        if (speed >= 1000)
        {
            precision = 0;
        }
        else if (speed >= 100)
        {
            precision = 1;
        }
        else if( speed >= 10 )
        {
            precision = 2;
        }

        if (speed != Value)
        {
          Value = speed;
          ConnectionManager.Instance.Connection.SendTelemetryMessage(1, 1, 0, 4, precision, Value);
          return true;
        }

        return false;
      }

    }

    public class ThrottleTelemetryItem : TelemetryItem
    {
        public ThrottleTelemetryItem(int id)
            : base(id)
        {

        }

        public override bool Update(IVessel vessel)
        {
            float speed = vessel.mainThrottle * 100;

            if (speed != Value)
            {
                Value = speed;
                ConnectionManager.Instance.Connection.SendTelemetryMessage(2, 1, 5, 4, 0, Value);
                return true;
            }

            return false;
        }

    }

    public static int getIdForActionGroup(KSPActionGroup group)
    {
      switch (group)
      {
        case KSPActionGroup.RCS:
          return (int)SwitchId.SWITCH_RCS;
        case KSPActionGroup.SAS:
          return (int)SwitchId.SWITCH_SAS;
        case KSPActionGroup.Stage:
          return (int)SwitchId.SWITCH_STAGE;

        case KSPActionGroup.Light:
          return (int)SwitchId.SWITCH_LIGHTS;
        case KSPActionGroup.Gear:
          return (int)SwitchId.SWITCH_GEAR;
        case KSPActionGroup.Brakes:
          return (int)SwitchId.SWITCH_BRAKES;
      }

      return -1;
    }

    public static KSPActionGroup getActionGroupFromId(int id)
    {
      SwitchId swid = (SwitchId)id;
      switch (swid)
      {
        case SwitchId.SWITCH_RCS:
          return KSPActionGroup.RCS;
        
        case SwitchId.SWITCH_SAS:
          return KSPActionGroup.SAS;
        
        case SwitchId.SWITCH_STAGE:
          return KSPActionGroup.Stage;
        
        case SwitchId.SWITCH_LIGHTS:
          return KSPActionGroup.Light;

        case SwitchId.SWITCH_GEAR:
          return KSPActionGroup.Gear;

        case SwitchId.SWITCH_BRAKES:
          return KSPActionGroup.Brakes;
      }

      return KSPActionGroup.None;
    }

    public enum StatusId
    {
      RCS = 0,
      SAS = 1,
      STAGE = 2,
      STAGE_ARMED = 3,
      THROTTLE = 4
    }

    enum SwitchId
    {
      SWITCH_RCS = 0,
      SWITCH_SAS = 1,
      SWITCH_STAGE = 2,
      SWITCH_STAGE_ARM = 3,
      SWITCH_DOCKING_MODE = 4,
      SWITCH_MAP_MODE = 5,
      SWITCH_THROTTLE_TOGGLE = 6,
      SWITCH_BRAKES = 7,
      SWITCH_GEAR = 8,
      SWITCH_LIGHTS = 9,
      SWITCH_TRANS_CTRL = 10,
      SWITCH_FINE_CTRL = 11,
      SWITCH_THROTTLE_MOMENT = 12,

      NUM_SWITCH
    }


    enum ViewMode
    {
      Map,
      Staging,
      Docking
    }

    List<StatusItem> Items = new List<StatusItem>();
    static ControlPanel instance = null;

    bool throttleEnabled = true;
    bool stageArmed = false;
    bool canFireStage = false;
    float throttleValue = 0.0f;

    ViewMode viewMode = ViewMode.Staging;

    public ControlPanel()
    {
      if (Log.Implementor == null)
      {
        Log.Implementor = new UnityLogger();
      }
    }

    public void Add(TelemetryItem telemetry)
    {
      Telemetry.Add(telemetry);
    }

    public void Add(KSPActionGroup group, bool state)
    {
      Items.Add(new StatusItem(group, state));
    }

    public int Count { get { return Items.Count; } }


    private bool HasOneHeartbeat = false;
    void InputMessageHandler(SerialConnection.MsgType type, byte size, System.IO.BinaryReader stream)
    {
        Log.Info("Msg type: {0}, size: {1}\n", type, size);
      switch (type)
      {
        case SerialConnection.MsgType.AnalogInput:
          {
            byte id = stream.ReadByte();
            float input = stream.ReadSingle();
            Log.Info("Msg: {0}, id: {1}, input: {2}\n", (int)type, id, input);

            if (id == 0)
            {
              throttleValue = input / 100.0f;  
            }
          }
          break;

        case SerialConnection.MsgType.Heartbeat:
          HasOneHeartbeat = true;
          int frame = stream.ReadInt32();
          Log.Info("[Device] HB: frame: {0}\n", frame);
          break;

        case SerialConnection.MsgType.GroupState:
          {
            byte id = stream.ReadByte();
            byte state = stream.ReadByte();
            Log.Info("Msg: {0}, id: {1}, input: {2}\n", (int)type, id, state);
            var switchId = (SwitchId)id;
            switch (switchId)
            {
              case SwitchId.SWITCH_SAS:
              case SwitchId.SWITCH_RCS:
              case SwitchId.SWITCH_GEAR:
              case SwitchId.SWITCH_LIGHTS:
              case SwitchId.SWITCH_BRAKES:
                CurrentVessel.setActionGroup(getActionGroupFromId(id), state == 1);
                break;

              
              case SwitchId.SWITCH_STAGE:
                if (stageArmed && canFireStage)
                {
                  canFireStage = false;
                  CurrentVessel.ActiveNextStage();
                }

                sendState((int)StatusId.STAGE_ARMED, canFireStage);
                break;

              case SwitchId.SWITCH_STAGE_ARM:
                {
                  bool last = stageArmed;
                  stageArmed = state == 1;

                  if (!canFireStage)
                    canFireStage = stageArmed;

                  if (last && !stageArmed && canFireStage)
                    canFireStage = false;

                  sendState((int)StatusId.STAGE_ARMED, canFireStage);
                }
                break;

              case SwitchId.SWITCH_THROTTLE_MOMENT:
              case SwitchId.SWITCH_THROTTLE_TOGGLE:
                throttleEnabled = state == 1;
                break;

              case SwitchId.SWITCH_DOCKING_MODE:
                {
                  bool wasDock = viewMode == ViewMode.Docking;
                  viewMode = state == 1 ? ViewMode.Docking : ViewMode.Staging;
                  bool isDock = viewMode == ViewMode.Docking;

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

              case SwitchId.SWITCH_MAP_MODE:
                {
                  bool wasMap = viewMode == ViewMode.Map;
                  viewMode = state == 1 ? ViewMode.Map : ViewMode.Staging;
                  bool isMap = viewMode == ViewMode.Map;

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

              case SwitchId.SWITCH_TRANS_CTRL:
                break;

              case SwitchId.SWITCH_FINE_CTRL:
                break;
            }
          }
          break;
        case SerialConnection.MsgType.LogInfo:
          {
              int len = stream.ReadInt16();
              string msg = new string(stream.ReadChars(len));
              Log.Info("Msg: {0}, len: {1}, msg: {2}\n", (int)type, len, msg);
              System.Console.WriteLine(msg);

          }
          break;
      }
    }

    // Use this for initialization
    public void Start(string port="COM4", int baud=9600)
    {
      if (ConnectionManager.Instance.Connection == null)
      {
        var connection = new SerialConnection(port, baud);
        ConnectionManager.Instance.Connection = connection;
        connection.Start();
        connection.RegisterHandler(SerialConnection.MsgType.AnalogInput, InputMessageHandler);
        connection.RegisterHandler(SerialConnection.MsgType.GroupState, InputMessageHandler);
        connection.RegisterHandler(SerialConnection.MsgType.Heartbeat, InputMessageHandler);
        connection.RegisterHandler(SerialConnection.MsgType.LogInfo, InputMessageHandler);
      }
      else
      {
        ConnectionManager.Instance.Start();
        running = true;
        HasOneHeartbeat = false;
      }
    }

    public void Stop()
    {
      running = false;
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

      if (!ConnectionManager.Instance.Connection.IsOpen ||
          !HasOneHeartbeat)
          return;

      foreach (StatusItem item in Items)
      {
        bool state = CurrentVessel.getActionGroup(item.kspGroup);
        if (item.state != state)
        {
          item.state = state;
          sendState((int)item.id, item.state);
          Console.WriteLine("[ControlPanel] id=" + item.id + ", state=" + item.state);
        }
      }

      foreach (var telemetry in Telemetry)
      {
        telemetry.Update(CurrentVessel);
        //Debug.Log("[ControlPanel] id=" + telemetry.Id + ", state=" + telemetry.Value);
      }
    }

    public void UpdateInput()
    {
      if (CurrentVessel == null)
        return;

      ConnectionManager.Instance.Update();
    }

    void sendState(int id, bool state)
    {
      buffer[0] = (byte)id;
      buffer[1] = (byte)(state ? 1 : 0);
      var serial = ConnectionManager.Instance.Connection;
      serial.SendMessage(SerialConnection.MsgType.GroupState, buffer);
    }

    Vector2 scrollPos = new Vector2();
    public void OnGUI()
    {
      SerialConnection serial = ConnectionManager.Instance.Connection;
      GUILayout.BeginVertical("box");

      GUILayout.Label((serial != null && serial.Connected) ? "Connected" : "Disconnected");

      GUILayout.Toggle(HasOneHeartbeat, "Has One HB");
      

      if( serial != null )
      {
        if( serial.Connected)
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
          GUILayout.Label(""+InputUpdateInterval);
          GUILayout.EndHorizontal();

          if (GUILayout.Button("Disconnect"))
          {
            serial.Disconnect();
            serial.DesiredConnectionState = SerialConnection.ConnectionState.Disconnected;
          }

          scrollPos = GUILayout.BeginScrollView(scrollPos);


          if (CurrentVessel != null)
          {
            GUILayout.BeginHorizontal();
            GUILayout.Label(CurrentVessel.Name);
            GUILayout.EndHorizontal();

            GUILayout.EndScrollView();

            GUILayout.Label(string.Format("Lq: {0:0.00}%", CurrentVessel.liquidFuelPercent));
            GUILayout.Label(string.Format("Ox: {0:0.00}%", CurrentVessel.oxiFuelPercent));
            GUILayout.Label(string.Format("Mo: {0:0.00}%", CurrentVessel.monoFuelPercent));
            GUILayout.Label(string.Format("Ev: {0:0.00}%", CurrentVessel.electricFuelPercent));
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
    }
  }

}

