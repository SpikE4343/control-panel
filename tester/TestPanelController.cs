
using ControlPanelPlugin;
using ControlPanelPlugin.Utils;
using UnityEngine;

namespace tester
{
  public class TestPanelController
  {
    public VesselWindow window;
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

      window.controller = this;
      window.ConnectPressed += window_ConnectPressed;

      window.Show();

      if (Log.Implementor == null)
      {
        Log.Implementor = new ConsoleLogger();
      }



      Singleton.Set<IVessel>(kspVessel);
      var app = Singleton.Set(new ControlPanelPlugin.Application());
      app.Initialize();

      Singleton.Get<Connection>().ConnectionStateChanged += (state) => window.ConnectionStateChange(state == Connection.State.Connected);

      app.Load("controlpanel.json");

      if (!coroutinesActive)
      {
        coroutinesActive = true;

        window.Vessel = kspVessel;
        window.Panel = Singleton.Get<ControlPanel>();
        window.inputTimer.Interval = (int)(Singleton.Get<Config>().Intervals.InputUpdate * 1000);
        window.panelTimer.Interval = (int)(Singleton.Get<Config>().Intervals.PanelUpdate * 1000);
        window.vesselTimer.Interval = 100;

        window.inputTimer.Tick += (s, e) => UpdatePanelInput();
        window.panelTimer.Tick += (s, e) => UpdatePanel();
        window.vesselTimer.Tick += (s, e) => UpdateVessel();

        window.StartTimers();

      }

      Save();
    }

    public void Save()
    {
      Singleton.Get<ControlPanelPlugin.Application>().Save("controlpanel.json");
    }

    void window_ConnectPressed(string port, int baud)
    {
      updatePanel = true;
      var connection = Singleton.Get<Connection>();
      if (connection.Connected)
      {
        connection.Stop();
      }
      else
      {
        connection.COM = port;
        connection.Baud = baud;

        connection.Start();
      }
    }

    public void UpdatePanelInput()
    {
      if (updatePanel && kspVessel != null)
        Singleton.Get<ControlPanelPlugin.Application>().UpdateInput();
    }

    public void UpdateVessel()
    {
      if (updatePanel && kspVessel != null)
        Singleton.Get<ControlPanelPlugin.Application>().UpdateVessel();
    }

    public void UpdatePanel()
    {
      if (updatePanel && kspVessel != null)
        Singleton.Get<ControlPanelPlugin.Application>().UpdatePanel();
    }

    void Stop()
    {
      Log.Info("[Control Panel] stopping panel");
      if (Singleton.Get<ControlPanel>() != null)
      {
        Singleton.Get<ControlPanel>().Stop();
      }
    }
  }
}
