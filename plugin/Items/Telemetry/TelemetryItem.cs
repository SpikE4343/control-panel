

using ControlPanelPlugin.telemetry.display;
using Newtonsoft.Json;
using System.Reflection;
namespace ControlPanelPlugin.Telemetry
{
  public class TelemetryItem : PanelItem
  {
    private static int nextId = 0;
    public int Id = nextId++;

    [JsonIgnore]
    public float Value { get { return Display.Value; } }


    ControlPanel panel;
    public override ControlPanel Panel
    {
      get { return panel; }
      set
      {
        panel = value;
        Display.Panel = panel;
      }
    }

    public string Property { get; set; }
    private PropertyInfo property;


    public TelemetryDisplay Display { get; set; }

    public TelemetryItem(string propertyName, TelemetryDisplay display)
    {
      Display = display;
      Property = propertyName;

      SetupProperty();
    }

    public void SetupProperty()
    {
      var vessel = typeof(IVessel);
      property = vessel.GetProperty(Property);
    }

    public TelemetryItem()
    {

    }

    public virtual void OnGUI()
    {

    }

    public virtual float GetLatestValue()
    {
      if (Panel == null || Panel.CurrentVessel == null)
        return 0.0f;

      return (float)property.GetValue(Panel.CurrentVessel, null);
    }

    public override bool Update()
    {
      float next = GetLatestValue();
      return Display.Update(next);
    }
  }
}
