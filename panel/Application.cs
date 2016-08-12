using Boomlagoon.JSON;
using ControlPanelPlugin.Items.Button;
using ControlPanelPlugin.Items.Button.Action;
using ControlPanelPlugin.Messages;
using ControlPanelPlugin.Network;
using ControlPanelPlugin.Telemetry;
using ControlPanelPlugin.Telemetry.Display;
using ControlPanelPlugin.Utils;
using System.IO;

namespace ControlPanelPlugin
{
  public class Application : IJsonConvertable
  {
    public void Initialize()
    {
      Singleton.Set(new Config());
      Singleton.Set(new ObjectPool());
      Singleton.Set(new MessageManager());
      var input = Singleton.Set(new InputDispatcher());
      Singleton.Set(new EventDispatcher());
      Singleton.Set(new ClassSerializer());
      Singleton.Set(new ControlPanel());
      Singleton.Set(new Connection());


      input.Initialize();
      //CreateDefaultLayout();
    }

    public void Shutdown()
    {
      Singleton.DestroyAll();
    }

    public void UpdateVessel()
    {
      var vessel = Singleton.Get<IVessel>();
      if (vessel != null)
      {
        vessel.Update();
      }
    }

    public void UpdateInput()
    {
      //Log.Debug("Update Input");
      Singleton.Get<ControlPanel>().UpdateInput();

      var connection = Singleton.Get<Connection>();
      if (connection != null)
      {
        connection.Update();
      }
    }

    public void UpdatePanel()
    {
      Singleton.Get<Connection>().Update();
      Singleton.Get<ControlPanel>().UpdateState();

    }

    public void CreateDefaultLayout()
    {
      var panel = Singleton.Get<ControlPanel>();

      panel.Add(new ButtonItem(Constants.Panel.SwitchId.Rcs, new ButtonStatusAction(Constants.Panel.ActionGroup.RCS)));
      panel.Add(new ButtonItem(Constants.Panel.SwitchId.Sas, new ButtonStatusAction(Constants.Panel.ActionGroup.SAS)));
      //panel.Add(new ButtonItem(Constants.Panel.SwitchId.Stage, new ButtonStatusAction(KSPActionGroup.Stage)));
      panel.Add(new ButtonItem(Constants.Panel.SwitchId.Brakes, new ButtonStatusAction(Constants.Panel.ActionGroup.Brakes)));
      panel.Add(new ButtonItem(Constants.Panel.SwitchId.Gear, new ButtonStatusAction(Constants.Panel.ActionGroup.Gear)));
      panel.Add(new ButtonItem(Constants.Panel.SwitchId.Lights, new ButtonStatusAction(Constants.Panel.ActionGroup.Light)));

      panel.Add(new ButtonItem(Constants.Panel.SwitchId.MapMode, new MapViewButtonAction()));
      panel.Add(new ButtonItem(Constants.Panel.SwitchId.DockingMode, new DockingViewButtonAction()));
      panel.Add(new ButtonItem(Constants.Panel.SwitchId.StageArm, new StageArmButtonAction()));
      panel.Add(new ButtonItem(Constants.Panel.SwitchId.Stage, new StageButtonAction()));

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

      display = new DigitalDisplay(0, 0, 8, 2);
      display.Add(10000000, 0);
      display.Add(1000000, 1);
      panel.Add(new TelemetryItem("altitude", display));

      display = new DigitalDisplay(1, 1, 4, 3);
      display.Add(1000, 0);
      display.Add(100, 1);
      display.Add(10, 2);
      display.Add(0, 3);
      panel.Add(new TelemetryItem("speed", display));

      //panel.Add(new AltitudeTelemetryItem(0, 0, 0, 8, 2));
      //panel.Add(new SpeedTelemetryItem(1, 1, 0, 4, 3));
      //panel.Add(new ThrottleTelemetryItem(3, 1, 5, 3, 0));

      display = new TimeDigitalDisplay(2, 2, 8, 0);
      panel.Add(new TelemetryItem("nextNodeSeconds", display));

      //panel.Add(new NextNodeTimeTelemetryItem(4, 2, 0, 8, 0));

      //panel.Add(new VerticalSpeedTelemetryItem(5, 3, 0, 4, 3));
      //panel.Add(new TerrainHeightTelemetryItem(6, 3, 4, 4, 3));

      var analog = new AnalogMeterDisplay(0);
      panel.Add(new TelemetryItem("liquidResourcePercent", analog));

      analog = new AnalogMeterDisplay(1);
      panel.Add(new TelemetryItem("oxiResourcePercent", analog));

      analog = new AnalogMeterDisplay(2);
      panel.Add(new TelemetryItem("monoResourcePercent", analog));

      analog = new AnalogMeterDisplay(3);
      panel.Add(new TelemetryItem("electricResourcePercent", analog));
    }

    public void Load(string file)
    {
      if (!File.Exists(file))
        return;

      string text = File.ReadAllText(file);
      FromJson(JSONObject.Parse(text));
    }

    public void Save(string file)
    {
      string text = ToJson().ToString();
      File.WriteAllText(file, text);
    }

    #region IJsonConvertable Members

    public JSONObject ToJson()
    {
      var json = new JSONObject();
      json.Add("config", Singleton.Get<Config>().ToJson());
      json.Add("connection", Singleton.Get<Connection>().ToJson());
      json.Add("panel", Singleton.Get<ControlPanel>().ToJson());
      return json;
    }

    public void FromJson(JSONObject json)
    {
      Singleton.Get<Config>().FromJson(json["config"]);
      Singleton.Get<Connection>().FromJson(json["connection"]);
      Singleton.Get<ControlPanel>().FromJson(json["panel"]);
    }

    #endregion
  }
}
