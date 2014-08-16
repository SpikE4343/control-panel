using ControlPanelPlugin;
using ControlPanelPlugin.Telemetry;
using ControlPanelPlugin.telemetry.digital;

namespace ControlPanelPlugin.telemetry
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
                Send();
                return true;
            }

            return false;
        }
    }
}
