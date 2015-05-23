using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ControlPanelPlugin.Telemetry.display;

namespace tests
{
  [TestClass]
  public class AnalogMeterDisplayTests
  {
    [TestMethod]
    public void Simple()
    {
      var display = new AnalogMeterDisplay(1);

      display.Update(0.0f);

      Assert.AreEqual(0.0f, display.Value);
    }


    [TestMethod]
    public void Format()
    {
      var display = new AnalogMeterDisplay(1);

      Assert.IsTrue( display.Update(10.0f) );

      Assert.AreEqual(0.0f, display.Value);
    }
  }
}
