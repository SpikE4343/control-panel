using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Newtonsoft.Json;
//using LitJson;

namespace ControlPanelPlugin
{
  public class Persistence
  {
    public static void Save<T>(string file, T o)
    {
      var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto };
     
      File.WriteAllText(file, JsonConvert.SerializeObject(o, Formatting.Indented, settings));
    }

    public static T Load<T>(string file) where T : class
    {
      var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto };
      return JsonConvert.DeserializeObject<T>(File.ReadAllText(file), settings);
    }
  }
}
