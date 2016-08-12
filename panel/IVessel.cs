
namespace ControlPanelPlugin
{
  public interface IVessel
  {
    /// <summary>
    /// string identifier to display
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Orbital height in meters
    /// </summary>
    float altitude { get; set; }

    /// <summary>
    /// Orbital speed in meters/second
    /// </summary>
    float speed { get; set; }

    /// <summary>
    /// Position percentage of all throttles. Range: (0.0 - 1.0)
    /// </summary>
    float mainThrottle { get; set; }

    /// <summary>
    /// Percentage of liquid fuel remaining. Range: (0.0 - 1.0 )
    /// </summary>
    float liquidResourcePercent { get; set; }

    /// <summary>
    /// Percentage of oxidizing fuel remaining. Range: (0.0 - 1.0)
    /// </summary>
    float oxiResourcePercent { get; set; }

    /// <summary>
    /// Percentage of monopropelant fuel remaining. Range: (0.0 - 1.0)
    /// </summary>
    float monoResourcePercent { get; set; }

    /// <summary>
    /// Percentage of electricity remaining. Range: ( 0.0 - 1.0 )
    /// </summary>
    float electricResourcePercent { get; set; }

    /// <summary>
    /// Percentage of max G force currently applied. Range: ( 0.0 - 1.0 )
    /// </summary>
    float geeLevelPercent { get; set; }

    /// <summary>
    /// Atmospheric pressure currently applied. Range: (0.0 - 1.0)
    /// </summary>
    float psiPercent { get; set; }

    /// <summary>
    /// Speed in meters/second relative to the forward directon 
    /// of the vessel (is that right??)
    /// </summary>
    float verticalSpeed { get; set; }

    /// <summary>
    /// Distance from ground in meters
    /// </summary>
    float heightFromTerrain { get; set; }

    /// <summary>
    /// Number of seconds until the next event node is reached
    /// </summary>
    float nextNodeSeconds { get; }

    /// <summary>
    /// Transition main controls to translational controls
    /// </summary>
    bool TanslationControls { get; set; }

    /// <summary>
    /// Reduce max range of controls
    /// </summary>
    bool FineControls { get; set; }

    /// <summary>
    /// Start the next stage in the list
    /// </summary>
    void ActiveNextStage();

    /// <summary>
    /// Show planitary map view
    /// </summary>
    void EnterMapView();

    /// <summary>
    /// Leave planitary map view
    /// </summary>
    void ExitMapView();

    /// <summary>
    /// Transition controls to docking state
    /// </summary>
    void EnterDockingMode();

    /// <summary>
    /// Transition controls out of docking state
    /// </summary>
    void ExitDockingMode();

    /// <summary>
    /// Current state of an action group
    /// </summary>
    /// <param name="group"></param>
    /// <returns></returns>
    bool getActionGroup(Constants.Panel.ActionGroup group);

    /// <summary>
    /// Modify state of an action group
    /// </summary>
    /// <param name="group"></param>
    /// <param name="value"></param>
    void setActionGroup(Constants.Panel.ActionGroup group, bool value);

    /// <summary>
    /// Called once per-frame to maintain state with target.
    /// </summary>
    void Update();
  }
}
