

namespace ControlPanelPlugin.Telemetry
{
  public abstract class TelemetryItem : PanelItem
  {
    private static int nextId = 0;
    public int Id = nextId++;
    public float Value;

    public string Property { get; set; }

    public TelemetryItem()
    {

    }

    public virtual void OnGUI()
    {

    }

    public virtual float GetLatestValue()
    {
      return 0.0f;
    }

    public override bool Update()
    {
      float next = GetLatestValue();

      foreach (var p in PrecisionValues)
      {
        if (next > p.Value)
        {
          Precision = p.Precision;
          break;
        }
      }

      if (next != Value)
      {
        Value = next;
        Send();
        return true;
      }

      return false;
    }
  }
}
