using ControlPanelPlugin.Telemetry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace ControlPanelPlugin.telemetry.display
{
  public class TimeDigitalDisplay : DigitalDisplay
  {
    public int Days;
    public int Hours;
    public int Minutes;
    public int Seconds;

    public override bool Update(float next)
    {
      float time = Mathf.Min(0.0f, next);
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

      return base.Update(value);
    }
  }
}
