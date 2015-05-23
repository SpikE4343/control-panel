
using ControlPanelPlugin;
using ControlPanelPlugin.Utils;
using UnityEngine;

namespace tester
{
  public class TestPanelController
  {
    public VesselWindow window;
    ControlPanel panel;
    TestVessel kspVessel = new TestVessel();
    bool updatePanel = false;

    private void createStatusItem(KSPActionGroup group)
    {
      //panel.Add(group, false);
    }

    bool coroutinesActive = false;
    public void Start()
    {
      window = new VesselWindow();

      window.ConnectPressed += window_ConnectPressed;

      window.Show();

      if (Log.Implementor == null)
      {
        Log.Implementor = new ConsoleLogger();
      }

      Singleton.Set<IVessel>(kspVessel);
      var app = Singleton.Set(new ControlPanelPlugin.Application());
      app.Initialize();

      if (!coroutinesActive)
      {
        coroutinesActive = true;

        window.Vessel = kspVessel;
        window.Panel = panel;
        window.inputTimer.Interval = (int)(Singleton.Get<Config>().Intervals.InputUpdate * 1000);
        window.panelTimer.Interval = (int)(Singleton.Get<Config>().Intervals.PanelUpdate * 1000);
        window.vesselTimer.Interval = 100;

        window.inputTimer.Tick += (s, e) => UpdatePanelInput();
        window.panelTimer.Tick += (s, e) => UpdatePanel();
        window.vesselTimer.Tick += (s, e) => UpdateVessel();

        window.StartTimers();

      }

      app.Save("controlpanel.json");
    }

    void window_ConnectPressed(string port, int baud)
    {
      var connection = Singleton.Get<Connection>();
      if (connection.Connected)
        connection.Stop();

      connection.COM = port;
      connection.Baud = baud;

      connection.Start();
    }



    public void UpdatePanelInput()
    {
      if (updatePanel && kspVessel != null)
      {
        Singleton.Get<ControlPanelPlugin.Application>().UpdateInput();
      }

      //yield return new WaitForSeconds(panel.InputUpdateInterval);
    }

    public void UpdateVessel()
    {
      if (updatePanel && kspVessel != null)
      {
        Singleton.Get<ControlPanelPlugin.Application>().UpdateVessel();
      }

      //yield return new WaitForSeconds(kspVessel.UpdateInterval);
    }

    public void UpdatePanel()
    {
      if (updatePanel && kspVessel != null)
      {
        Singleton.Get<ControlPanelPlugin.Application>().UpdatePanel();
      }

      // yield return new WaitForSeconds(panel.PanelUpdateInterval);
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
