using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ControlPanelPlugin.Utils
{
  public interface IJsonConvertable
  {
    Dictionary<string, object> ToJson();
    void FromJson(Dictionary<string, object> json);
  }
}
