using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ControlPanelPlugin;

namespace tester
{
  public class TestVessel : IVessel
  {
    private double mAltitude = 0.0f;
    private double mSpeed = 0.0f;
    private double mMainThrottle = 0.0;
    private double mLiquidFuelMax = 1000000.0;
    private double mLiquidFuel = 1000000.0;

    private double mOxiFuelMax = 1000000.0;
    private double mOxiFuel = 1000000.0;

    private double mMonoFuelMax = 1000000.0;
    private double mMonoFuel = 1000000.0;

    private double mEvFuelMax = 1000000.0;
    private double mEvFuel = 1000000.0;

    private int mStage = 1;

    private Dictionary<Constants.Panel.ActionGroup, bool> actionGroups = new Dictionary<Constants.Panel.ActionGroup, bool>();

    public string Name
    {
      get { return "test"; }
    }

    public float altitude
    {
      get
      {
        return (float)mAltitude;
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
        return (float)mSpeed;
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
        return (float)mMainThrottle;
      }
      set
      {
        mMainThrottle = value;
      }
    }

    public void ActiveNextStage()
    {
      ++mStage;
    }

    public void EnterMapView()
    {

    }

    public void ExitMapView()
    {

    }

    public bool getActionGroup(Constants.Panel.ActionGroup group)
    {
      if (actionGroups.ContainsKey(group))
        return actionGroups[group];

      return false;
    }

    public void setActionGroup(Constants.Panel.ActionGroup group, bool value)
    {
      actionGroups[group] = value;
    }

    public float UpdateInterval { get { return 0.01f; } }

    public void Update()
    {
      mAltitude = mAltitude + 0.5f;// *mainThrottle;
      mSpeed = mSpeed + 0.01f;// *mainThrottle;
      mLiquidFuel = (mLiquidFuel - 100.0f);
      mOxiFuel -= 500.0f;
      mMonoFuel -= 100.0f;
      mEvFuel -= 1.0f;
    }

    public float liquidFuelPercent
    {
      get
      {
        return ((float)(mLiquidFuel / mLiquidFuelMax) * 100.0f);
      }
      set
      {
        mLiquidFuel = mLiquidFuelMax * (value / 100.0f);
      }
    }

    public float oxiFuelPercent
    {
      get
      {
        return ((float)(mOxiFuel / mOxiFuelMax) * 100.0f);
      }
      set
      {
        mOxiFuel = mOxiFuelMax * (value / 100.0f);
      }
    }

    public float monoFuelPercent
    {
      get
      {
        return ((float)(mMonoFuel / mMonoFuelMax) * 100.0f);
      }
      set
      {
        mMonoFuel = mMonoFuelMax * (value / 100.0f);
      }
    }

    public float electricFuelPercent
    {
      get
      {
        return ((float)(mEvFuel / mEvFuelMax) * 100.0f);
      }
      set
      {
        mEvFuel = mEvFuelMax * (value / 100.0f);
      }
    }

    public float geeLevelPercent
    {
      get
      {
        return 0.0f;
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
        return 0.0f;
      }
      set
      {
        throw new NotImplementedException();
      }
    }

    float vertSpd = 0.0f;
    public float verticalSpeed { get; set; }


    public void EnterDockingMode()
    {
      //throw new NotImplementedException();
      Log.Debug("Enter Docking Mode\n");
    }

    public void ExitDockingMode()
    {
      //  throw new NotImplementedException();
      Log.Debug("Exit Docking Mode\n");
    }

    public bool TanslationControls { get; set; }
    public bool FineControls { get; set; }


    #region IVessel Members


    public float liquidResourcePercent
    {
      get
      {
        return liquidFuelPercent;
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
        return oxiFuelPercent;
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
        return monoFuelPercent;
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
        return electricFuelPercent;
      }
      set
      {
        throw new NotImplementedException();
      }
    }

    public float heightFromTerrain
    {
      get
      {
        return 0.0f;
      }
      set
      {
        throw new NotImplementedException();
      }
    }

    public float nextNodeSeconds
    {
      get { return 0.0f; }
    }

    #endregion
  }
}
