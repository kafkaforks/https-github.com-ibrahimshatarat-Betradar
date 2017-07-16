using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Zyan.Communication;
using MsgPack.Serialization;

namespace SharedLibrary.RPC
{
    public class MsgPackSerializer : ISerializationHandler
    {
        public byte[] Serialize<T>(T thisObj)
        {
            var serializer = SerializationContext.Default.GetSerializer<T>();

            using (var byteStream = new MemoryStream())
            {
                serializer.Pack(byteStream, thisObj);
                return byteStream.ToArray();
            }
        }

        public T Deserialize<T>(byte[] bytes)
        {
            var serializer = SerializationContext.Default.GetSerializer<T>();
            using (var byteStream = new MemoryStream(bytes))
            {
                return serializer.Unpack(byteStream);
            }
        }

        public byte[] Serialize(object data)
        {
            throw new NotImplementedException();
        }

        public object Deserialize(Type dataType, byte[] data)
        {
            throw new NotImplementedException();
        }
    }
  
}
