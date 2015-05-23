using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ControlPanelPlugin.Items;
using ControlPanelPlugin.Utils;

namespace ControlPanelPlugin.Messages
{
  public enum MsgType
  {
    None,
    GroupState = 0,
    Telemetry,
    AnalogInput,
    Action,
    Heartbeat,
    AnalogMeterTelemetry,
    LogInfo,
    NumMessages
  }

  public interface IMessage
  {
    MsgType Type { get; }
    void write(BinaryWriter stream);
    void read(BinaryReader stream);
  }

  [ClassSerializer("TelemetryMsg")]
  public class TelemetryMsg : IMessage, IPoolable
  {
    public byte id;
    public byte display = 0;
    public byte precision;
    public byte startDigit;
    public byte maxDigits;
    public int value;

    public void write(BinaryWriter stream)
    {
      stream.Write(id);
      stream.Write(display);
      stream.Write(precision);
      stream.Write(startDigit);
      stream.Write(maxDigits);
      int lvalue = (int)(value * Math.Pow(10, precision));
      stream.Write(lvalue);
    }

    public void read(BinaryReader stream)
    {
      id = (byte)stream.ReadByte();
      display = (byte)stream.ReadByte();
      precision = (byte)stream.ReadByte();
      startDigit = (byte)stream.ReadByte();
      maxDigits = (byte)stream.ReadByte();
      value = stream.ReadInt32();
    }

    public void Reset()
    {
      display = 0;
      precision = 0;
      startDigit = 0;
      maxDigits = 0;
      value = 0;
    }

    #region IMessage Members

    public MsgType Type
    {
      get { return MsgType.Telemetry; }
    }

    #endregion
  }

  [ClassSerializer("GroupStateMsg")]
  public class GroupStateMsg : IMessage, IPoolable
  {
    public byte id;
    public byte state = 0;

    #region IMessage Members

    public MsgType Type
    {
      get { return MsgType.GroupState; }
    }

    #endregion

    public void write(BinaryWriter stream)
    {
      stream.Write(id);
      stream.Write(state);
    }

    public void read(BinaryReader stream)
    {
      id = (byte)stream.ReadByte();
      state = (byte)stream.ReadByte();
    }

    public void Reset()
    {
      id = 0;
      state = 0;
    }
  }

  [ClassSerializer("AnalogMeterMsg")]
  public class AnalogMeterMsg : IMessage, IPoolable
  {
    public byte meter;
    public byte value = 0;

    #region IMessage Members

    public MsgType Type
    {
      get { return MsgType.AnalogMeterTelemetry; }
    }

    #endregion

    public void write(BinaryWriter stream)
    {
      stream.Write(meter);
      stream.Write(value);
    }

    public void read(BinaryReader stream)
    {
      meter = (byte)stream.ReadByte();
      value = (byte)stream.ReadByte();
    }

    public void Reset()
    {
      meter = 0;
      value = 0;
    }
  }

  [ClassSerializer("HeartbeatMsg")]
  public class HeartbeatMsg : IMessage, IPoolable
  {
    public int frame;

    #region IMessage Members

    public MsgType Type
    {
      get { return MsgType.Heartbeat; }
    }

    #endregion

    public void write(BinaryWriter stream)
    {
      stream.Write(frame);
    }

    public void read(BinaryReader stream)
    {
      frame = stream.ReadInt32();
    }

    public void Reset()
    {
      frame = 0;
    }
  }

  [ClassSerializer("LogInfoMsg")]
  public class LogInfoMsg : IMessage, IPoolable
  {
    public string message;

    #region IMessage Members

    public MsgType Type
    {
      get { return MsgType.LogInfo; }
    }

    #endregion

    public void write(BinaryWriter stream)
    {
      short len = (short)message.Length;
      stream.Write(len);
      stream.Write(message.ToCharArray(), 0, len);
    }

    public void read(BinaryReader stream)
    {
      short len = stream.ReadInt16();
      message = new string(stream.ReadChars(len));
    }

    public void Reset()
    {
      message = string.Empty;
    }
  }

  [ClassSerializer("AnalogInputMsg")]
  public class AnalogInputMsg : IMessage, IPoolable
  {
    public MsgType Type { get { return MsgType.AnalogInput; } }

    public byte id;
    public float value;

    public void write(BinaryWriter stream)
    {
      stream.Write(id);
      stream.Write(value);
    }

    public void read(BinaryReader stream)
    {
      id = stream.ReadByte();
      value = stream.ReadSingle();
    }

    public void Reset()
    {
      id = 0;
      value = 0.0f;
    }




  }

  public struct MessageHeader
  {
    public byte e;
    public byte R;
    public byte type;
    public byte size;

    public static int Size { get { return sizeof(byte) * 4; } }

    public void Read(BinaryReader rdr)
    {
      e = rdr.ReadByte();
      R = rdr.ReadByte();
      type = rdr.ReadByte();
      size = rdr.ReadByte();
    }
  }

  public class MessageManager
  {
    BinaryWriter msgWriter = new BinaryWriter(new MemoryStream());
    BinaryWriter writer = new BinaryWriter(new MemoryStream());
    BinaryReader reader = new BinaryReader(new MemoryStream());

    private Stream stream { get { return reader.BaseStream; } }
    private int BytesRemaining { get { return (int)(stream.Length - stream.Position); } }

    private bool HasPendingMessage = false;
    static Dictionary<MsgType, Type> msgCreator = new Dictionary<MsgType, Type>();

    MsgType PendingType = MsgType.None;
    byte PendingSize = 0;

    public void SetStream(Stream stream)
    {
      reader = new BinaryReader(stream);
      writer = new BinaryWriter(stream);
    }

    public void AddMessage(MsgType msg, Type creator)
    {
      msgCreator.Add(msg, creator);
    }

    private bool ReadHeader(BinaryReader rdr, out MsgType type, out byte size)
    {
      type = MsgType.None;
      size = 0;

      if (BytesRemaining < 4)
        return false;

      //Console.Write((char)streamReader.PeekChar()); x 
      byte marker = reader.ReadByte();

      if (marker != 'e')
      {
        //Console.Write((char)marker);
        return false;
      }

      char marker1 = (char)reader.ReadByte();
      //Console.Write(marker);
      if (marker1 != 'R')
      {
        return false;
      }

      type = (MsgType)reader.ReadByte();
      //Console.Write((byte)PendingType);
      size = reader.ReadByte();

      return true;

      //rdr.BaseStream.
      //char e = (char)rdr.ReadByte();
      //char r = (char)rdr.ReadByte();
      //type = rdr.ReadByte();
      //size = rdr.ReadByte();
      //return e == 'e' && r == 'R' && type >= 0 && type < (int)SerialConnection.MsgType.NumMessages;
    }

    private void WriteHeader(BinaryWriter wrt, byte type, byte size)
    {
      wrt.Write('e');
      wrt.Write('R');
      wrt.Write(type);
      wrt.Write(size);
    }

    public void WriteMsg(IMessage msg)
    {
      msgWriter.Seek(0, SeekOrigin.Begin);
      msg.write(msgWriter);
      int size = (int)msgWriter.BaseStream.Position;

      var ms = msgWriter.BaseStream as MemoryStream;
      writer.Write(ms.GetBuffer());

      Singleton.Get<ObjectPool>().Release((IPoolable)msg);
    }

    public void ReadMsg()
    {
      if (!HasPendingMessage)
      {
        if (!ReadHeader(reader, out PendingType, out PendingSize))
          return;

        HasPendingMessage = true;
      }

      if (!HasPendingMessage || BytesRemaining < PendingSize)
      {
        return;
      }

      HasPendingMessage = false;

      var msgType = (MsgType)PendingType;
      var msg = Singleton.Get<ObjectPool>().Grab<IMessage>(msgCreator[msgType]);
      msg.read(reader);

      Singleton.Get<EventDispatcher>().Fire(msg);
      Singleton.Get<ObjectPool>().Release((IPoolable)msg);
    }
  }
}
