using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using ControlPanelPlugin;
using ControlPanelPlugin.telemetry;
using ControlPanelPlugin.telemetry.analog;
using ControlPanelPlugin.Telemetry;
using UnityEngine;
using ControlPanelPlugin.telemetry.display;

namespace tester
{
  public class TestModule
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

      if (ConnectionManager.Instance.Connection == null)
      {
        ConnectionManager.Instance.Connection = new SerialConnection();
      }

      if (ConnectionManager.Instance.Panel == null)
      {
        panel = new ControlPanel();
        ConnectionManager.Instance.Panel = panel;

        //panel.Load("panel.json");


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

        /*
        createStatusItem(KSPActionGroup.RCS);
        createStatusItem(KSPActionGroup.SAS);
        createStatusItem(KSPActionGroup.Stage);
        createStatusItem(KSPActionGroup.Brakes);
        createStatusItem(KSPActionGroup.Gear);
        createStatusItem(KSPActionGroup.Light);

        //      [      ALT      ]
        // [0]: |7|6|5|4|3|2|1|0|

        //      [ Throt |  Spd  ]
        // [1]: |7|6|5|4|3|2|1|0|

        //      [ Next Node Time]                
        // [2]: |7|6|5|4|3|2|1|0|

        //      [ T.Ht  | V spd ]
        // [3]: |7|6|5|4|3|2|1|0|
        // [4]: |7|6|5|4|3|2|1|0|

        panel.Add(new AltitudeTelemetryItem(0, 0, 0, 8, 2));
        panel.Add(new SpeedTelemetryItem(1, 1, 0, 4, 3));
        panel.Add(new ThrottleTelemetryItem(3, 1, 5, 3, 0));

        panel.Add(new NextNodeTimeTelemetryItem(4, 2, 0, 8, 0));

        panel.Add(new VerticalSpeedTelemetryItem(5, 3, 0, 4, 3));
        panel.Add(new TerrainHeightTelemetryItem(6, 3, 4, 4, 3));

        panel.Add(new LiquidResourceItem(0));
        panel.Add(new OxiResourceItem(1));
        panel.Add(new MonoResourceItem(2));
        panel.Add(new EvResourceItem(3));
         * */
      }
      else
      {
        panel = ConnectionManager.Instance.Panel;
      }


      if (!updatePanel)
      {
        Log.Info("[Control Panel] starting panel");
        panel.CurrentVessel = kspVessel;
        ConnectionManager.Instance.Start();
        updatePanel = true;
      }
                                                                             
      if (!coroutinesActive)
      {
        coroutinesActive = true;

        window.Vessel = kspVessel;
        window.inputTimer.Interval = (int)(panel.InputUpdateInterval * 1000);
        window.panelTimer.Interval = (int)(panel.PanelUpdateInterval * 1000);
        window.vesselTimer.Interval = 100;

        window.inputTimer.Tick += (s,e) => UpdatePanelInput();
        window.panelTimer.Tick += (s, e) => UpdatePanel();
        window.vesselTimer.Tick += (s, e) => UpdateVessel();

        window.StartTimers();

      }

      panel.Save("panel.json");
    }

    void window_ConnectPressed(string port, int baud)
    {
        var connection = ConnectionManager.Instance.Connection;
        connection.Stop();

        connection.COM = port;
        connection.Baud = baud;

        connection.Start();
    }

    
    
    public void UpdatePanelInput()
    {
        if (updatePanel && kspVessel != null)
        {
          panel.UpdateInput();
        }

        //yield return new WaitForSeconds(panel.InputUpdateInterval);
    }

    public void UpdateVessel()
    {
        if (updatePanel && kspVessel != null)
        {
          kspVessel.Update();
        }

        //yield return new WaitForSeconds(kspVessel.UpdateInterval);
    }

    public void UpdatePanel()
    {
        if (updatePanel && kspVessel != null)
        {
          panel.UpdateState();
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
