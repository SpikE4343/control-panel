using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ControlPanelPlugin
{
  public class KSPVessel : IVessel
  {
    enum ResourceTypes
    {
      LiquidFuel = 0,
      Oxidizer = 1,
      MonoPropellant = 2,
      ElectricCharge = 3,
      NumResourceTypes
    }

    Dictionary<ResourceTypes, HashSet<PartResource>> resourceList = new Dictionary<ResourceTypes, HashSet<PartResource>>();
    Dictionary<ResourceTypes, Vessel.ActiveResource> activeResources = new Dictionary<ResourceTypes, Vessel.ActiveResource>();
    Dictionary<string, ResourceTypes> nameToResourceMap = new Dictionary<string, ResourceTypes>();

    public KSPVessel()
    {
      nameToResourceMap.Add( Enum.GetName( typeof(ResourceTypes), ResourceTypes.LiquidFuel), ResourceTypes.LiquidFuel );
      nameToResourceMap.Add(Enum.GetName(typeof(ResourceTypes), ResourceTypes.Oxidizer), ResourceTypes.Oxidizer);
      nameToResourceMap.Add(Enum.GetName(typeof(ResourceTypes), ResourceTypes.MonoPropellant), ResourceTypes.MonoPropellant);
      nameToResourceMap.Add(Enum.GetName(typeof(ResourceTypes), ResourceTypes.ElectricCharge), ResourceTypes.ElectricCharge); 
    }

    public Vessel vessel;

    public string Name
    {
      get
      {
        return vessel != null ? vessel.name : string.Empty;
      }
    }

    public float altitude
    {
      get
      {
        return (float)vessel.altitude;
      }
      set
      {
        throw new NotImplementedException();
      }
    }

    public float speed
    {
      get
      {
        if (altitude > 30000)
          return (float)vessel.obt_speed;

        return (float)vessel.srfSpeed;
      }
      set
      {
        throw new NotImplementedException();
      }
    }

    public float mainThrottle
    {

      get
      {
        return FlightInputHandler.state.mainThrottle;
      }
      set
      {
        FlightInputHandler.state.mainThrottle = value;
      }
    }

    public void ActiveNextStage()
    {
      Staging.ActivateNextStage();
    }

    public void EnterMapView()
    {
      MapView.EnterMapView();
    }

    public void ExitMapView()
    {
      MapView.ExitMapView();
    }

    public void EnterDockingMode()
    {
      FlightUIModeController.Instance.SetMode(FlightUIMode.DOCKING);
    }

    public void ExitDockingMode()
    {
      FlightUIModeController.Instance.SetMode(FlightUIMode.STAGING);
    }

    public bool getActionGroup(KSPActionGroup group)
    {
      return vessel.ActionGroups[group];
    }

    public void setActionGroup(KSPActionGroup group, bool value)
    {
      vessel.ActionGroups.SetGroup(group, value);
    }

    public float UpdateInterval { get { return 1.0f; } }

    float nextNodeTime = 0.0f;
    public void Update()
    {
      if (vessel != null)
      {
        updateResources();

        nextNodeTime = -1;
        if (vessel.patchedConicSolver != null && vessel.patchedConicSolver.maneuverNodes.Count > 0)
        {
          nextNodeTime = (float)(Planetarium.GetUniversalTime() - vessel.patchedConicSolver.maneuverNodes[0].UT);
        }
      }
    }

    public float nextNodeSeconds { get { return nextNodeTime; } }

    private void updateResources()
    {
      resourceList.Clear();
      foreach (Part part in vessel.parts)
      {
        if (part.Resources.Count <= 0)
          continue;
        
        foreach (PartResource partResource in part.Resources)
        {
          ResourceTypes type = ResourceTypes.NumResourceTypes;
          if (!nameToResourceMap.TryGetValue(partResource.info.name, out type))
            continue;

          HashSet<PartResource> list = null;
          if (!resourceList.TryGetValue(type, out list) )
          {
            list = new HashSet<PartResource>();
            resourceList.Add(type, list);
          }

          list.Add(partResource);
        }
      }

      activeResources.Clear();

      var activeList = vessel.GetActiveResources();
      foreach (var res in activeList)
      {
        ResourceTypes type = ResourceTypes.NumResourceTypes;
        if (!nameToResourceMap.TryGetValue(res.info.name, out type))
          continue;

        if (activeResources.ContainsKey(type))
        {
          activeResources[type] = res;
          continue;
        }

        activeResources.Add(type, res);
      }
    }

    private float getResourcePercent(ResourceTypes type)
    {
      // check stage resources first
      Vessel.ActiveResource ar = null;
      if (activeResources.TryGetValue(type, out ar) && ar != null)
      {
        return (float)(ar.amount / ar.maxAmount) * 100.0f;
      }

      HashSet<PartResource> rl = null;
      if (!resourceList.TryGetValue(type, out rl) || rl == null)
        return 0.0f;

      double amount=0.0, max=0.0;
      foreach (var part in rl)
      {
        amount += part.amount;
        max += part.maxAmount;
      }

      return (float)(amount / max) * 100.0f;
    }

    public float liquidResourcePercent
    {
      get
      {
        return getResourcePercent(ResourceTypes.LiquidFuel);
      }
      set
      {
        throw new NotImplementedException();
      }
    }

    public float oxiResourcePercent
    {
      get
      {
        return getResourcePercent(ResourceTypes.Oxidizer);
      }
      set
      {
        throw new NotImplementedException();
      }
    }

    public float monoResourcePercent
    {
      get
      {
        return getResourcePercent(ResourceTypes.MonoPropellant);
      }
      set
      {
        throw new NotImplementedException();
      }
    }

    public float electricResourcePercent
    {
      get
      {
        return getResourcePercent(ResourceTypes.ElectricCharge);
      }
      set
      {
        throw new NotImplementedException();
      }
    }

    public float geeLevelPercent
    {
      get
      {
        return (float)vessel.geeForce;
      }
      set
      {
        throw new NotImplementedException();
      }
    }

    public float psiPercent
    {
      get
      {
        throw new NotImplementedException();
      }
      set
      {
        throw new NotImplementedException();
      }
    }

    public float verticalSpeed
    {
      get
      {
        return (float)vessel.verticalSpeed;
      }
      set
      {
        throw new NotImplementedException();
      }
    }
    

    public bool TanslationControls { get; set; }
    public bool FineControls 
     {
      get
      {
        return FlightInputHandler.fetch.precisionMode;
      }
      set
      {
        FlightInputHandler.fetch.precisionMode = value;
      }
    }

    public List<Vessel.ActiveResource> ActiveResources
    {
      get
      {
        return null;
      }
    }


    public float heightFromTerrain
    {
      get
      {
        return vessel.heightFromTerrain;
      }
      set
      {
        throw new NotImplementedException();
      }
    }
  }
}
