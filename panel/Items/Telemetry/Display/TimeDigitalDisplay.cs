using ControlPanelPlugin.Telemetry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using ControlPanelPlugin.Items;
using ControlPanelPlugin.Utils;

namespace ControlPanelPlugin.Telemetry.Display
{
  [ClassSerializer("TimeDigitalDisplay")]
  public class TimeDigitalDisplay : DigitalDisplay
  {
    public int Days;
    public int Hours;
    public int Minutes;
    public int Seconds;

    public TimeDigitalDisplay()
    {

    }

    public TimeDigitalDisplay(int id, int display, int startDigit, int maxDigits)
      : base(id, display, startDigit, maxDigits)
    {

    }

    public override bool Update(float next)
    {
      float time = Mathf.Max(0.0f, next);
      Precision = 0;

      const int minute = 60;
      const int hour = 60 * minute;
      const int day = 24 * hour;

      float fdays = time / day;
      Days = (int)fdays;
      fdays -= Days;

      float fhours = (fdays * day) / hour;
      int Hours = (int)fhours;
      fhours -= Hours;

      float fminutes = (fhours * hour) / minute;
      Minutes = (int)fminutes;
      fminutes -= Minutes;

      Seconds = (int)(fminutes * minute);

      float value = (float)(Days * 1000000.0f +
                            Hours * 10000.0f +
                            Minutes * 100.0f +
                            Seconds);

      return base.Update(value);
    }
  }
}
