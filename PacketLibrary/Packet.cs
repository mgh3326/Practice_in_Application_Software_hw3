using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace PacketLibrary
{
    [Serializable]
    public class Packet
    {
        /// <summary>
        /// 패킷의 크기입니다. 패킷 자체의 헤더 512KiB와 데이터 2MiB로 잡았습니다
        /// </summary>
        public const int PACKET_SIZE = (512 * 1024) + (2 * 1024 * 1024);
        /// <summary>
        /// 패킷 데이터 크기입니다
        /// </summary>
        public const int PACKET_DATA_SIZE = (2 * 1024 * 1024);
        /// <summary>
        /// 패킷 종류입니다
        /// </summary>
        public enum PACKET_TYPE
        {
            /// <summary>
            /// 파일 리스트를 요청합니다
            /// </summary>
            TYPE_GET_FILE_LIST,
            /// <summary>
            /// 파일 리스트를 반환합니다
            /// </summary>
            TYPE_FILE_LIST,
            /// <summary>
            /// 파일의 이름을 보냅니다
            /// </summary>
            TYPE_FILE_NAME,
            /// <summary>
            /// 보내질 파일의 패킷 갯수를 보냅니다
            /// </summary>
            TYPE_FILE_PKT_CNT,
            /// <summary>
            /// 파일을 보냅니다
            /// </summary>
            TYPE_FILE_DATA,
            /// <summary>
            /// 연결 확립 확인 패킷입니다
            /// </summary>
            TYPE_CONNECTION_OK
        };
        /// <summary>
        /// 패킷 종류입니다
        /// </summary>
        public PACKET_TYPE packet_type;
        /// <summary>
        /// 패킷의 데이터입니다
        /// </summary>
        public object data;
        /// <summary>
        /// 패킷을 바이트 배열로 직렬화합니다
        /// </summary>
        /// <returns>직렬화된 패킷의 바이트 배열</returns>
        public byte[] serialize()
        {
            byte[] b = null;

            try
            {
                using (MemoryStream memory = new MemoryStream(PACKET_SIZE))
                {
                    new BinaryFormatter().Serialize(memory, this);
                    b = memory.ToArray();
                }
            }
            catch (OutOfMemoryException)
            {
                b = null;
            }
            catch (Exception)
            {
                b = null;
            }

            return b;
        }
        /// <summary>
        /// 바이트 배열으로부터 패킷 객체로 역직렬화합니다
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Packet deserialize(byte[] b)
        {
            Packet pkt = null;

            if (b == null)
            {
                return null;
            }

            try
            {
                using (MemoryStream memory = new MemoryStream(b))
                {
                    pkt = (Packet)new BinaryFormatter().Deserialize(memory);
                }
            }
            catch (Exception)
            {
                return null;
            }

            return pkt;
        }
    }
}
