using System.Collections.Generic;
using Newtonsoft.Json;

namespace ControlPanelPlugin
{
  public class ConnectionSave
  {
    public string COM { get; set; }
    public int Baud { get; set; }
  }

  public class PanelSave
  {
    public ConnectionSave Connection = new ConnectionSave();

    [JsonProperty("items")]
    public List<PanelItem> PanelItems;
  }
}
