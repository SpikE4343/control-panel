﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using ControlPanelPlugin.Utils;

namespace ControlPanelPlugin.Utils
{
  public class ClassSerializer
  {
    private Dictionary<string, Type> types = new Dictionary<string, Type>();
    private Dictionary<Type, string> names = new Dictionary<Type, string>();

    public ClassSerializer()
    {

    }

    public T CreateItem<T>(string name) where T : class
    {
      return (T)Activator.CreateInstance(types[name]);
    }

    public Type GetTypeForName(string name)
    {
      return types[name];
    }

    public string GetItemName<T>() where T : class
    {
      return GetItemName(typeof(T));
    }

    public string GetItemName(Type type)
    {
      return names[type];
    }

    public Dictionary<string, object> ToJson(IJsonConvertable obj)
    {
      var json = new Dictionary<string, object>();
      json.Add("type", GetItemName(obj.GetType()));
      json.Add("data", obj.ToJson());
      return json;
    }

    public T FromJson<T>(Dictionary<string, object> json) where T : class
    {
      string type = json["type"] as string;
      object instance = Activator.CreateInstance(types[type]);
      (instance as IJsonConvertable).FromJson(json["data"] as Dictionary<string, object>);
      return (T)instance;
    }

    private void InitializeItems()
    {
      foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
      {
        foreach (var type in GetTypesWithPanelItemAttribute(assembly))
        {
          foreach (var attr in type.GetCustomAttributes(typeof(ClassSerializerAttribute), false))
          {
            var item = attr as ClassSerializerAttribute;
            types.Add(item.SerializedName, type);
            names.Add(type, item.SerializedName);
          }
        }
      }
    }

    private IEnumerable<Type> GetTypesWithPanelItemAttribute(Assembly assembly)
    {
      foreach (Type type in assembly.GetTypes())
      {
        if (type.GetCustomAttributes(typeof(ClassSerializerAttribute), false).Length > 0)
        {
          yield return type;
        }
      }
    }


  }
}
