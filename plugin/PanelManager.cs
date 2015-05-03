using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ControlPanelPlugin
{
  public class PanelManager
  {
    public delegate void ConnectionSetHandler(SerialConnection connection);
    public event ConnectionSetHandler ConnectionSet;

    private static PanelManager instance;
    public static PanelManager Instance
    {
      get
      {
        if (instance == null)
          instance = new PanelManager();

        return instance;
      }
      private set { }
    }

    public PanelManager()
    {
      Instance = this;
    }

    private SerialConnection connection;
    public SerialConnection Connection
    {
      get { return connection; }
      set
      {
        connection = value;
        if (ConnectionSet != null)
        {
          ConnectionSet(connection);
        }
      }
    }
    public ControlPanel Panel { get; set; }

    public void Start()
    {
      if (Connection != null)
        Connection.Start();

      if (Panel != null)
      {
        Panel.Start();
      }
    }

    public void Update()
    {
      if (Connection != null)
        Connection.Update();
    }

    public void Pause()
    {
      if (Panel != null)
      {
        Panel.Stop();
      }
    }

    public void Stop()
    {
      if (Connection != null)
      {
        Connection.Stop();
      }

      if (Panel != null)
      {
        Panel.Stop();
      }
    }

    public void Save(string file)
    {
      var output = new PanelSave();
      if (Connection != null)
      {
        output.Connection.COM = Connection.COM;
        output.Connection.Baud = Connection.Baud;
      }
      output.PanelItems = Panel.PanelItems;
      Persistence.Save(file, output);
    }

    public void Load(string file)
    {
      var input = Persistence.Load<PanelSave>(file);
      Panel.PanelItems = input.PanelItems;
    }
  }
}
