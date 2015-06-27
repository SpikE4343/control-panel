using System.Collections;
using ControlPanelPlugin;
using ControlPanelPlugin.Messages;
using ControlPanelPlugin.Network;
using ControlPanelPlugin.Telemetry;
using ControlPanelPlugin.Telemetry.Display;
using ControlPanelPlugin.Utils;
using tester;
using UnityEngine;


public class UITestController : MonoBehaviour
{
  IVessel kspVessel = new TestVessel();
  bool updatePanel = false;

  void Awake()
  {
    Initialize();

    useGUILayout = true;
  }

  private void Initialize()
  {
    if (Log.Implementor == null)
    {
      Log.Implementor = new UnityLogger();
    }

    var app = Singleton.Set(new ControlPanelPlugin.Application());
    app.Initialize();

    app.Load("controlpanel.json");

    if (Singleton.Get<ControlPanel>().PanelItems.Count == 0)
    {
      app.CreateDefaultLayout();
    }

    app.Save("controlpanel.json");

    Singleton.Set<IVessel>(kspVessel);

    updatePanel = true;

    if (!coroutinesActive)
    {
      coroutinesActive = true;
      StartCoroutine(UpdatePanelInput());
      StartCoroutine(UpdatePanel());
      StartCoroutine(UpdateVessel());
    }
  }

  bool coroutinesActive = false;

  // coroutine for polling input
  IEnumerator UpdatePanelInput()
  {
    while (coroutinesActive)
    {
      if (updatePanel)
      {
        Singleton.Get<ControlPanelPlugin.Application>().UpdateInput();
      }

      yield return new WaitForSeconds(Singleton.Get<Config>().Intervals.InputUpdate);
    }
  }

  IEnumerator UpdateVessel()
  {
    while (coroutinesActive)
    {
      if (updatePanel)
      {
        Singleton.Get<ControlPanelPlugin.Application>().UpdateVessel();
      }

      yield return new WaitForSeconds(Singleton.Get<Config>().Intervals.VesselUpdate);
    }
  }

  // coroutine for updating state/sending data to panel
  IEnumerator UpdatePanel()
  {
    while (coroutinesActive)
    {
      if (updatePanel)
      {
        Singleton.Get<ControlPanelPlugin.Application>().UpdatePanel();
      }

      yield return new WaitForSeconds(Singleton.Get<Config>().Intervals.PanelUpdate);
    }
  }

  void OnGUI()
  {
    Singleton.Get<ControlPanel>().OnGUI();
  }

  void Stop()
  {
    Log.Info("[Control Panel] stopping panel");
  }
}

