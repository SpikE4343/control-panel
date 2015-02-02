using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ControlPanelPlugin.telemetry;
using ControlPanelPlugin.telemetry.analog;
using ControlPanelPlugin.Telemetry;
using UnityEngine;
using ControlPanelPlugin.telemetry.display;

namespace ControlPanelPlugin
{
  [KSPAddon(KSPAddon.Startup.Flight, false)]
  public class ControlPanelGUI : MonoBehaviour
  {
    ControlPanel panel;
    KSPVessel kspVessel = new KSPVessel();
    bool updatePanel = false;

    void Awake()
    {
      useGUILayout = true;
      GameEvents.onVesselChange.Add(onVesselChange);
    }

    bool coroutinesActive = false;
    void onVesselChange(Vessel v)
    {
      kspVessel.vessel = v;

      if (v == null)
      {
        PanelManager.Instance.Stop();
        updatePanel = false;
        return;
      }

      if (Log.Implementor == null)
      {
        Log.Implementor = new UnityLogger();
      }

      if (PanelManager.Instance.Connection == null)
      {
        var connection = new SerialConnection("COM4", 9600);
        PanelManager.Instance.Connection = connection;
      }

      if (PanelManager.Instance.Panel == null)
      {
        panel = new ControlPanel();

        panel.Load("panel.json");

        PanelManager.Instance.Panel = panel;

        panel.Add(new ButtonStatusItem(Constants.Panel.SwitchId.None, KSPActionGroup.RCS, false));
        panel.Add(new ButtonStatusItem(Constants.Panel.SwitchId.None, KSPActionGroup.SAS, false));
        panel.Add(new ButtonStatusItem(Constants.Panel.SwitchId.None, KSPActionGroup.Stage, false));
        panel.Add(new ButtonStatusItem(Constants.Panel.SwitchId.None, KSPActionGroup.Brakes, false));
        panel.Add(new ButtonStatusItem(Constants.Panel.SwitchId.None, KSPActionGroup.Gear, false));
        panel.Add(new ButtonStatusItem(Constants.Panel.SwitchId.None, KSPActionGroup.Light, false));

        panel.Add(new MapViewButtonItem());
        panel.Add(new DockingViewButtonItem());
        panel.Add(new StageArmButtonItem());
        panel.Add(new StageButtonItem());

        var display = new DigitalDisplay(5, 3, 0, 4);
        display.Add(1000, 0);
        display.Add(100, 1);
        display.Add(10, 2);
        display.Add(0, 3);
        panel.Add(new TelemetryItem("verticalSpeed", display));

        ////      [      ALT      ]
        //// [0]: |7|6|5|4|3|2|1|0|

        ////      [ Throt |  Spd  ]
        //// [1]: |7|6|5|4|3|2|1|0|

        ////      [ Next Node Time]                
        //// [2]: |7|6|5|4|3|2|1|0|

        ////      [ T.Ht  | V spd ]
        //// [3]: |7|6|5|4|3|2|1|0|
        //// [4]: |7|6|5|4|3|2|1|0|

        panel.Add(new AltitudeTelemetryItem(0, 0, 0, 8, 2));
        panel.Add(new SpeedTelemetryItem(1, 1, 0, 4, 3));
        //panel.Add(new ThrottleTelemetryItem(3, 1, 5, 3, 0));

        panel.Add(new NextNodeTimeTelemetryItem(4, 2, 0, 8, 0));

        //panel.Add(new VerticalSpeedTelemetryItem(5, 3, 0, 4, 3));
        //panel.Add(new TerrainHeightTelemetryItem(6, 3, 4, 4, 3));

        panel.Add(new LiquidResourceItem(0));
        //panel.Add(new OxiResourceItem(1));
        panel.Add(new MonoResourceItem(2));
        //panel.Add(new EvResourceItem(3));
      }
      else
      {
        panel = PanelManager.Instance.Panel;
      }

      if (!updatePanel)
      {
        Log.Info("[Control Panel] starting panel");
        panel.CurrentVessel = kspVessel;
        PanelManager.Instance.Start();
        updatePanel = true;
      }

      if (!coroutinesActive)
      {
        coroutinesActive = true;
        StartCoroutine(UpdatePanelInput());
        StartCoroutine(UpdatePanel());
        StartCoroutine(UpdateVessel());
      }

      panel.Save("panel.json");
    }

    // coroutine for polling input
    IEnumerator UpdatePanelInput()
    {
      while (active)
      {
        if (updatePanel && kspVessel.vessel != null)
        {
          panel.UpdateInput();
        }

        yield return new WaitForSeconds(panel.InputUpdateInterval);
      }
    }

    IEnumerator UpdateVessel()
    {
      while (active)
      {
        if (updatePanel && kspVessel.vessel != null)
        {
          kspVessel.Update();
        }

        yield return new WaitForSeconds(kspVessel.UpdateInterval);
      }
    }

    // coroutine for updating state/sending data to panel
    IEnumerator UpdatePanel()
    {
      while (active)
      {
        if (updatePanel && kspVessel.vessel != null)
        {
          panel.UpdateState();
        }

        yield return new WaitForSeconds(panel.PanelUpdateInterval);
      }
    }

    Rect windowPos = new Rect(20, 20, 200, 180);
    void OnGUI()
    {
      windowPos = GUILayout.Window(12053, windowPos,
                                      OnWindowGUI,
                                      "Control Panel",
                                      GUILayout.Width(200),
                                      GUILayout.Height(180),
                                      GUILayout.ExpandHeight(true),
                                      GUILayout.ExpandWidth(true));
    }

    void OnWindowGUI(int windowid)
    {
      DrawGUI();

      if (panel != null)
      {
        panel.OnGUI();
      }

      GUI.DragWindow(new Rect(0, 0, 10000, 10000));
    }

    void DrawGUI()
    {
      GUILayout.BeginVertical("box");
      if (kspVessel.vessel != null)
      {
        GUILayout.BeginHorizontal();
        GUILayout.Label(string.Format("Vessel: {0}", kspVessel.Name));
        GUILayout.EndHorizontal();
      }


      GUILayout.EndVertical();

      GUILayout.BeginVertical("box");
      var serial = PanelManager.Instance.Connection;
      if (serial != null)
      {
        GUILayout.Label(string.Format("Cs: {0}", serial.CurrentConnectionState));
        GUILayout.Label(string.Format("Ds: {0}", serial.DesiredConnectionState));
        GUILayout.Label(string.Format("TxB: {0}", serial.BytesToWrite));
        GUILayout.Label(string.Format("RxB: {0}", serial.BytesToRead));
      }

      GUILayout.Label((serial != null && serial.Connected) ? "Connected" : "Disconnected");
      GUILayout.EndVertical();
    }

    void Stop()
    {
      Log.Info("[Control Panel] stopping panel");
      if (panel != null)
      {
        panel.Stop();
      }
    }
  }
}
