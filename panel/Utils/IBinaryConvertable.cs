using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ControlPanelPlugin.Utils
{
  public interface IBinaryConvertable
  {
    void ToBinary(BinaryWriter stream);
    void FromBinary(BinaryReader stream);
  }
}
