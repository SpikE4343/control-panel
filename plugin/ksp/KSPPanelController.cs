/// 
///
///
using System.Collections;
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

      if ( Singleton.Get<Application>() != null )
        return;

      var app = Singleton.Set(new Application());
      app.Initialize();

      app.Load("controlpanel.json");

      if (Singleton.Get<ControlPanel>().PanelItems.Count == 0)
      {
        app.CreateDefaultLayout();
      }

      app.Save("controlpanel.json");
    }

    bool coroutinesActive = false;

    void onVesselChange(Vessel v)
    {
      kspVessel.vessel = v;
      
      if( Singleton.Get<IVessel>() == null )
        Singleton.Set<IVessel>(kspVessel);

      if (v == null)
      {
        updatePanel = false;
        return;
      }

      updatePanel = true;

      if (!coroutinesActive)
      {
        coroutinesActive = true;
        StartCoroutine(UpdatePanelInput());
        StartCoroutine(UpdatePanel());
        StartCoroutine(UpdateVessel());
      }
    }

    // coroutine for polling input
    IEnumerator UpdatePanelInput()
    {
      while ( gameObject.activeSelf )
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
      while (gameObject.activeSelf)
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
      while ( gameObject.activeSelf )
      {
        if (updatePanel && kspVessel.vessel != null)
        {
          Singleton.Get<Application>().UpdatePanel();
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
}
