using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ControlPanelPlugin;
using UnityEngine;
using UnityEditor;

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
        mAltitude = value;
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
        mSpeed = value;
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

    #region IVessel Members

    public float UpdateInterval { get { return 0.1f; } }

    public void Update()
    {
      mAltitude = mAltitude + 0.2f * mainThrottle;
      mSpeed = mSpeed + 0.01f * mainThrottle;
      mLiquidFuel = (mLiquidFuel - 100.0f) * mainThrottle;
      mOxiFuel -= 500.0f * mainThrottle;
      mMonoFuel -= 100.0f * mainThrottle;
      mEvFuel -= 1.0f * mainThrottle;
      verticalSpeed += 0.1f * mainThrottle;
      nodeSeconds -= UpdateInterval;
      terrainHeight -= 0.1f * mainThrottle;

      if (nodeSeconds < 0.0f)
        nodeSeconds = 2.0f * 24.0f * 60.0f * 60.0f;

      if (terrainHeight < 0.0f)
        terrainHeight = 1000.0f;
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





    public float liquidResourcePercent
    {
      get
      {
        return liquidFuelPercent;
      }
      set
      {
        liquidFuelPercent = value;
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
        oxiFuelPercent = value;
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
        monoFuelPercent = value;
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
        electricFuelPercent = value;
      }
    }

    private float terrainHeight = 0.0f;
    public float heightFromTerrain
    {
      get
      {
        return terrainHeight;
      }
      set
      {
        terrainHeight = value;
      }
    }

    private float nodeSeconds = 0.0f;
    public float nextNodeSeconds { get { return nodeSeconds; } }
    #endregion

    #region GUI

    Rect windowPos;
    Vector2 scrollPos;
    public void OnGUI()
    {
      windowPos = GUILayout.Window(12055, windowPos,
                                      OnWindowGUI,
                                      "Vessel",
                                      GUILayout.Width(300),
                                      GUILayout.Height(580),
                                      GUILayout.ExpandHeight(true),
                                      GUILayout.ExpandWidth(true));
    }

    void OnWindowGUI(int windowid)
    {
      GUILayout.BeginVertical("box");
      GUILayout.BeginHorizontal();
      GUILayout.Label(string.Format("Vessel: {0}", Name));
      GUILayout.EndHorizontal();

      scrollPos = GUILayout.BeginScrollView(scrollPos, GUILayout.ExpandHeight(true), GUILayout.MaxHeight(1000));

      var properties = GetType().GetProperties();

      foreach (var prop in properties)
      {
        if (prop.PropertyType.Name == "Single")
        {
          var value = (float)prop.GetValue(this, null);
          var valueStr = string.Format("{0}", value);

          GUILayout.BeginHorizontal();
          GUILayout.Label(prop.Name);
          GUILayout.FlexibleSpace();

          if (prop.GetSetMethod() == null)
          {
            GUILayout.Label(valueStr);
          }
          else
          {
            var nextStr = GUILayout.TextField(valueStr, GUILayout.MinWidth(100));

            if (nextStr != valueStr)
            {
              var next = Single.Parse(nextStr);
              prop.SetValue(this, next, null);
            }
          }

          GUILayout.EndHorizontal();
        }
      }

      GUILayout.EndScrollView();

      GUILayout.EndVertical();

      GUI.DragWindow(new Rect(0, 0, 10000, 10000));

    }
    #endregion
  }
}
