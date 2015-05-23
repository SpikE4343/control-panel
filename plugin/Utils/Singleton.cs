﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ControlPanelPlugin.Utils
{
  public class Singleton
  {
    private static Dictionary<Type, object> instances = new Dictionary<Type, object>();

    public static T Set<T>(T instance) where T : class
    {
      instances.Add(typeof(T), instance);
      return instance as T;
    }

    public static T Get<T>() where T : class
    {
      return instances[typeof(T)] as T;
    }

    public static void Destroy<T>() where T : class
    {
      instances.Remove(typeof(T));
    }

    public static void DestroyAll()
    {
      instances.Clear();
    }
  }
}
