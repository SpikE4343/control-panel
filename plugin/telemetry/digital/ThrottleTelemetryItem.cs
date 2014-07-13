using ControlPanelPlugin;
using ControlPanelPlugin.Telemetry;
using plugin.telemetry.digital;

namespace plugin.telemetry
{
    public class ThrottleTelemetryItem : DigitalTelemetryItem
    {
        public ThrottleTelemetryItem(int id,
                                     int display,
                                     int startDigit,
                                     int maxDigits,
                                     int precision)
            : base(id, display, startDigit, maxDigits, precision)
        {

        }

        public override bool Update(IVessel vessel)
        {
            float speed = vessel.mainThrottle * 100;
            if (speed != Value)
            {
                Value = speed;
                ConnectionManager.Instance.Connection.SendTelemetryMessage(TelemetryId, Display, StartDigit, MaxDigits, Precision, Value);
                return true;
            }

            return false;
        }
    }
}
