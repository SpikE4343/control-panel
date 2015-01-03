///
///
using ControlPanelPlugin.telemetry.digital;
using UnityEngine;

namespace ControlPanelPlugin.telemetry
{
  public class NextNodeTimeTelemetryItem : DigitalTelemetryItem
  {
    public int Days;
    public int Hours;
    public int Minutes;
    public int Seconds;

    public NextNodeTimeTelemetryItem()
    {

    }

    public NextNodeTimeTelemetryItem(int id,
                                 int display,
                                 int startDigit,
                                 int maxDigits,
                                 int precision)
      : base(id, display, startDigit, maxDigits, precision)
    {

    }

    public override bool Update()
    {
      float time = Mathf.Min(0.0f, Panel.CurrentVessel.nextNodeSeconds);
      Precision = 0;

      const int minute = 60;
      const int hour = 60 * minute;
      const int day = 24 * hour;

      float fdays = time / day;
      Days = (int)fdays;
      fdays -= Days * day;

      float fhours = (fdays * day) / hour;
      int Hours = (int)fhours;
      fhours -= Hours * hour;

      float fminutes = (fhours * hour) / minute;
      Minutes = (int)fminutes;
      fminutes -= Minutes * minute;

      Seconds = (int)(fminutes);

      float value = (float)(Days * 1000000 +
                            Hours * 10000 +
                            Minutes * 100 +
                            Seconds);

      // |7|6|5|4|3|2|1|0|
      // | D | H | M | S |
      if (value != Value)
      {
        Value = value;
        Send();
        return true;
      }

      return false;
    }

    public override void OnGUI()
    {
      GUILayout.BeginVertical();
      GUILayout.Label("Next Node Time: ");
      GUILayout.Label(string.Format("Days: {0}", Days));
      GUILayout.Label(string.Format("Hours: {0}", Hours));
      GUILayout.Label(string.Format("Minutes: {0}", Minutes));
      GUILayout.Label(string.Format("Seconds: {0}", Seconds));
      GUILayout.Label(string.Format("Value: {0:0.00}", Value));
      GUILayout.EndVertical();
    }

  }
}
