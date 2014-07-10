using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ControlPanelPlugin.Messages
{
    public enum MsgType
    {
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
        void write(BinaryWriter stream);
        void read(BinaryReader stream);

        void Reset();
    }

    public class MessagePool
    {
        private static Dictionary< Type, Queue<IMessage> > messages = new Dictionary< Type, Queue<IMessage>>();
        public static T Grab<T>() where T : IMessage, new()
        {
            Queue<IMessage> queue;
            if( messages.TryGetValue(typeof(T), out queue) && 
                queue.Count > 0 )
            {
                return (T)queue.Dequeue();
            }
            return new T();
        }

        public static void Release<T>(T msg) where T: IMessage
        {
            Queue<IMessage> queue;
            if (!messages.TryGetValue(typeof(T), out queue))
            {
                queue = new Queue<IMessage>();
                messages.Add(typeof(T), queue);
            }
            msg.Reset();
            queue.Enqueue(msg);
        }
    }

    public class TelemetryMsg : IMessage
    {
        public byte display = 0;
        public byte precision;
        public byte startDigit;
        public byte maxDigits;
        public uint value;

        public void write(BinaryWriter stream)
        {
            stream.Write(display);
            stream.Write(precision);
            stream.Write(startDigit);
            stream.Write(maxDigits);
            stream.Write(value);
        }

        public void read(BinaryReader stream)
        {
            display = (byte)stream.ReadByte();
            precision = (byte)stream.ReadByte();
            startDigit = (byte)stream.ReadByte();
            maxDigits = (byte)stream.ReadByte();
            value = (uint)stream.ReadInt32();
        }
    
        public void Reset()
        {
 	        display = 0;
            precision = 0;
            startDigit = 0;
            maxDigits = 0;
            value = 0;
        }
    }

    public struct MessageHeader
    {
        public byte e;
        public byte R;
        public byte type;
        public byte size;

        public static int Size { get { return sizeof(byte)*4; } } 

        public void Read( BinaryReader rdr )
        {
            e = rdr.ReadByte();
            R = rdr.ReadByte();
            type = rdr.ReadByte();
            size = rdr.ReadByte();
        }
    }
    public class MessageManager
    {
        static BinaryWriter writer = new BinaryWriter( new MemoryStream() );
        static BinaryReader reader = new BinaryReader( new MemoryStream() );
        
        static private bool ReadHeader( BinaryReader rdr, out byte type, out byte size )
        {
            char e = (char)rdr.ReadByte();
            char r = (char)rdr.ReadByte();
            type = rdr.ReadByte();
            size = rdr.ReadByte();
            return e == 'e' && r == 'R' && type >= 0 && type < (int)SerialConnection.MsgType.NumMessages;
        }

        static private void WriteHeader( BinaryWriter wrt, byte type, byte size )
        {
            wrt.Write('e');
            wrt.Write('R');
            wrt.Write(type);
            wrt.Write(size);
        }

        static public void WriteMsg( IMessage msg, BinaryWriter msgOut )
        {
            writer.Seek( 0, SeekOrigin.Begin );
            msg.write(writer);
            int size = (int)writer.BaseStream.Position;

            MemoryStream ms = writer.BaseStream as MemoryStream;
            msgOut.Write( ms.GetBuffer() );
        }

        public static IMessage ReadMsg(BinaryReader reader)
        {
            byte type = 0;
            byte size = 0;
            if( !ReadHeader( reader, out type, out size ))
                return null;

            IMessage msg = null;
            MsgType msgType = (MsgType)type;
            switch( msgType )
            {
                case MsgType.GroupState:
                    break;

                case MsgType.Telemetry:
                    msg = new TelemetryMsg();
                    break;

                case MsgType.AnalogInput:
                    break;

                case MsgType.Action:
                    break;

                case MsgType.Heartbeat:
                    break;

                case MsgType.LogInfo:
                    break;
            }

            if( msg != null )
                msg.read(reader);
            
            return msg;
        }
    }
}
