using ControlPanelPlugin;
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
    public delegate void DisconnectHandler();
    public delegate void ConnectHandler(string port, int baud);
    public event ConnectHandler ConnectPressed;
    public event DisconnectHandler DisconnectPressed;

    private IVessel vessel;
    public IVessel Vessel 
    { 
      get { return vessel; } 
      set 
      { 
        propertyGrid1.SelectedObject = value; 
        vessel = value; 
      } 
    } 

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

    private void connectionButton_Click(object sender, EventArgs e)
    {
      if (ConnectionManager.Instance.Connection.Connected)
      {
        if (DisconnectPressed != null)
        {
          DisconnectPressed();
        }
      }
      else if( ConnectPressed != null )
      {
        ConnectPressed(portText.Text, Int32.Parse(buadText.Text)); 
      }
    }
  }
}
