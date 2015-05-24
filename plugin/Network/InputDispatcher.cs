﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ControlPanelPlugin.Messages;
using ControlPanelPlugin.Utils;

namespace ControlPanelPlugin.Network
{
  public class InputDispatcher
  {
    public class GroupStateEventHandler
    {
      public delegate void MessageReceivedHandler(GroupStateMsg msg);
      public event MessageReceivedHandler OnEvent;

      public void Fire(GroupStateMsg msg)
      {
        if (OnEvent != null)
          OnEvent(msg);
      }
    }

    private Dictionary<Constants.Panel.SwitchId, GroupStateEventHandler> groupStateHandlers = new Dictionary<Constants.Panel.SwitchId, GroupStateEventHandler>();


    public void Initialize()
    {
      Singleton.Get<EventDispatcher>().Handler<GroupStateMsg>().OnEvent += GroupStateMessageHandler;
    }

    private void GroupStateMessageHandler(GroupStateMsg msg)
    {
      groupStateHandlers[(Constants.Panel.SwitchId)msg.id].Fire(msg);
    }

    public GroupStateEventHandler GroupStateHandler(Constants.Panel.SwitchId id)
    {
      return groupStateHandlers[id];
    }
  }
}