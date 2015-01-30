using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ControlPanelPlugin
{
  public class ConnectionManager
  {
    private static ConnectionManager instance;
    public static ConnectionManager Instance
    {
      get
      {
        if (instance == null)
          instance = new ConnectionManager();

        return instance;
      }
      private set { }
    }

    public ConnectionManager()
    {
      Instance = this;
    }

    public SerialConnection Connection { get; set; }
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
  }
}
