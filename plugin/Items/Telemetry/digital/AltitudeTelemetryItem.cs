﻿using ControlPanelPlugin.telemetry.digital;

namespace ControlPanelPlugin.Telemetry
{
    public class AltitudeTelemetryItem : DigitalTelemetryItem
    {
      public AltitudeTelemetryItem()
      {

      }

        public AltitudeTelemetryItem( int id,
                                     int display,
                                     int startDigit,
                                     int maxDigits,
                                     int precision)
            : base( id, display, startDigit, maxDigits,precision)
        {

        }

        public override bool Update()
        {
          float alt = Panel.CurrentVessel.altitude;

            if (alt >= 10000000)
            {
                Precision = 0;
            }
            else if (alt >= 1000000)
            {
                Precision = 1;
            }

            if (alt != Value)
            {
                Value = alt;
                ConnectionManager.Instance.Connection.SendTelemetryMessage(TelemetryId, Display, StartDigit, MaxDigits, Precision, Value);
                return true;
            }

            return false;
        }

    }
}
