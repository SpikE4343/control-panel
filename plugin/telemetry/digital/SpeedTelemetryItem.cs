///
///

using ControlPanelPlugin;
using ControlPanelPlugin.Telemetry;
using plugin.telemetry.digital;

namespace plugin.telemetry
{
    public class SpeedTelemetryItem : DigitalTelemetryItem
    {
        public SpeedTelemetryItem(int id,
                                     int display,
                                     int startDigit,
                                     int maxDigits,
                                     int precision)
            : base(id, display, startDigit, maxDigits, precision)
        {

        }

        public override bool Update(IVessel vessel)
        {
            float speed = vessel.speed;
            int precision = 3;

            if (speed >= 1000)
            {
                precision = 0;
            }
            else if (speed >= 100)
            {
                precision = 1;
            }
            else if (speed >= 10)
            {
                precision = 2;
            }

            if (speed != Value)
            {
                Value = speed;
                ConnectionManager.Instance.Connection.SendTelemetryMessage(1, 1, 0, 4, precision, Value);
                return true;
            }

            return false;
        }

    }
}
