using ControlPanelPlugin;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;

using System.Windows.Forms;
using ControlPanelPlugin.Utils;

namespace tester
{
  public partial class VesselWindow : Form
  {
    public delegate void DisconnectHandler();
    public delegate void ConnectHandler(string port, int baud);
    public event ConnectHandler ConnectPressed;
    public event DisconnectHandler DisconnectPressed;
    public TestPanelController controller;

    public void ConnectionStateChange(bool connected)
    {
      connectionButton.Text = connected ? "Disconnect" : "Connect";
      connectionButton.Enabled = true;
    }

    bool connecting = false;

    private IVessel vessel;
    public IVessel Vessel
    {
      get { return vessel; }
      set
      {
        vesselGrid.SelectedObject = value;
        vessel = value;
      }
    }

    public object Panel
    {
      set
      {
        panelGrid.SelectedObject = value;
      }
    }

    public Timer inputTimer = new Timer();
    public Timer vesselTimer = new Timer();
    public Timer panelTimer = new Timer();

    public VesselWindow()
    {
      vesselTimer.Tick += vesselTimer_Tick;
      InitializeComponent();
    }

    void vesselTimer_Tick(object sender, EventArgs e)
    {
      vesselGrid.Refresh();
      //panelGrid.Refresh();
    }

    public void StartTimers()
    {
      inputTimer.Start();
      vesselTimer.Start();
      panelTimer.Start();
    }

    private void connectionButton_Click(object sender, EventArgs e)
    {
      if (Singleton.Get<Connection>().Connected)
      {
        if (DisconnectPressed != null)
        {
          DisconnectPressed();
        }
      }
      else if (ConnectPressed != null)
      {
        ConnectPressed(portText.Text, Int32.Parse(buadText.Text));
        connectionButton.Text = "Connecting...";
        connectionButton.Enabled = false;
      }
    }

    private void VesselWindow_FormClosed(object sender, FormClosedEventArgs e)
    {
      System.Windows.Forms.Application.Exit();
    }

    private void saveButtonClicked(object sender, EventArgs e)
    {
      controller.Save();
    }
  }
}
