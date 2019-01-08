using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Virtek.Base.UdpMulticast.Discovery
{
    public abstract class DiscoveryBase
    {
        protected T DeserializeObject<T>(byte[] message)
        {
            BinaryFormatter formater = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream(message))
            {
                var objectMessage = formater.Deserialize(ms);
                return (T)objectMessage;
            }
        }

        protected object DeserializeObject(byte[] message)
        {
            BinaryFormatter formater = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream(message))
            {
                return formater.Deserialize(ms);
            }
        }

        protected byte[] SerializeObject(object message)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                BinaryFormatter formater = new BinaryFormatter();
                formater.Serialize(ms, message);
                ms.Position = 0;
                var data = ReadFully(ms);
                return data;
            }
        }

        protected byte[] ReadFully(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }
    }
}
