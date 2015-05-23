using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ControlPanelPlugin
{
    public interface IVessel
    {
        string Name { get; }
        float altitude { get; set; }
        float speed { get; set; }
        float mainThrottle { get; set; }

        float liquidResourcePercent { get; set; }
        float oxiResourcePercent { get; set; }
        float monoResourcePercent { get; set; }
        float electricResourcePercent { get; set; }
        float geeLevelPercent { get; set; }
        float psiPercent { get; set; }
        float verticalSpeed { get; set; }

        float heightFromTerrain { get; set; }
        float nextNodeSeconds { get; }

        bool TanslationControls { get; set; }
        bool FineControls { get; set; }

        void ActiveNextStage();
        void EnterMapView();
        void ExitMapView();

        void EnterDockingMode();
        void ExitDockingMode();

        bool getActionGroup(KSPActionGroup group);
        void setActionGroup(KSPActionGroup group, bool value);

        void Update();
    }
}
