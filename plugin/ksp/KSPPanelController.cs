using System.Collections;
using ControlPanelPlugin.Messages;
using ControlPanelPlugin.Network;
using ControlPanelPlugin.Telemetry;
using ControlPanelPlugin.Telemetry.Display;
using ControlPanelPlugin.Utils;
using UnityEngine;

namespace ControlPanelPlugin
{
  [KSPAddon(KSPAddon.Startup.Flight, false)]
  public class KSPPanelController : MonoBehaviour
  {
    KSPVessel kspVessel = new KSPVessel();
    bool updatePanel = false;

    void Awake()
    {
      Initialize();

      useGUILayout = true;
      GameEvents.onVesselChange.Add(onVesselChange);
    }

    private void Initialize()
    {
      if (Log.Implementor == null)
      {
        Log.Implementor = new UnityLogger();
      }

      Singleton.Set(new Application()).Initialize();

    }

    bool coroutinesActive = false;

    void onVesselChange(Vessel v)
    {
      kspVessel.vessel = v;

      Singleton.Set<IVessel>(kspVessel);

      if (v == null)
      {
        updatePanel = false;
        return;
      }
    }

    // coroutine for polling input
    IEnumerator UpdatePanelInput()
    {
      while (active)
      {
        if (updatePanel && kspVessel.vessel != null)
        {
          Singleton.Get<Application>().UpdateInput();
        }

        yield return new WaitForSeconds(Singleton.Get<Config>().Intervals.InputUpdate);
      }
    }

    IEnumerator UpdateVessel()
    {
      while (active)
      {
        if (updatePanel && kspVessel.vessel != null)
        {
          Singleton.Get<Application>().UpdateVessel();
        }

        yield return new WaitForSeconds(Singleton.Get<Config>().Intervals.VesselUpdate);
      }
    }

    // coroutine for updating state/sending data to panel
    IEnumerator UpdatePanel()
    {
      while (active)
      {
        if (updatePanel && kspVessel.vessel != null)
        {
          Singleton.Get<Application>().UpdatePanel();
        }

        yield return new WaitForSeconds(Singleton.Get<Config>().Intervals.PanelUpdate);
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
      Singleton.Get<ControlPanel>().OnGUI();
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
      var serial = Singleton.Get<Connection>();
      if (serial != null)
      {
        GUILayout.Label(string.Format("Cs: {0}", serial.CurrentConnectionState));
        GUILayout.Label(string.Format("Ds: {0}", serial.DesiredConnectionState));
        //GUILayout.Label(string.Format("TxB: {0}", serial.BytesToWrite));
        //GUILayout.Label(string.Format("RxB: {0}", serial.BytesToRead));
      }

      GUILayout.Label((serial != null && serial.Connected) ? "Connected" : "Disconnected");
      GUILayout.EndVertical();
    }

    void Stop()
    {
      Log.Info("[Control Panel] stopping panel");
    }
  }
}
