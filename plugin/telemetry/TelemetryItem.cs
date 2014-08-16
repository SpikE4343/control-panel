

namespace ControlPanelPlugin.Telemetry
{
    public abstract class TelemetryItem
    {
        private static int nextId = 0;
        public int Id = nextId++;
        public float Value;

        public TelemetryItem()
        {

        }

        public abstract bool Update(IVessel vessel);

        public virtual void OnGUI()
        {

        }
    }
}
