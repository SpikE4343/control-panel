using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlPanelPlugin
{
  public class Constants
  {
    public class Panel
    {
      public enum SwitchId
      {
        None,
        SWITCH_DOCKING_MODE=0,
      }

      public enum StatusId
      {
        None
      }

      public enum ViewMode
      {
        Map,
        Staging,
        Docking
      }
    }
  }
}
