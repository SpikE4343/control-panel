using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace tester
{
  public partial class VesselWindow : Form
  {
    public Timer inputTimer = new Timer();
    public Timer vesselTimer = new Timer();
    public Timer panelTimer = new Timer();

    public VesselWindow()
    {
      InitializeComponent();
    }

    public void StartTimers()
    {
      inputTimer.Start();
      vesselTimer.Start();
      panelTimer.Start();
    }
  }
}
