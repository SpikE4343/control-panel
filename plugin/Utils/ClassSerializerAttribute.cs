using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ControlPanelPlugin.Utils
{
  [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
  public class ClassSerializerAttribute : Attribute
  {
    public readonly string SerializedName;

    public ClassSerializerAttribute(string name)
    {
      SerializedName = name;
    }
  }
}
