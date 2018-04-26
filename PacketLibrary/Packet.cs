using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace PacketLibrary
{
    public enum PacketType
    {
        초기화 = 0,
        회원가입,
        로그인,
        조회,
        업로드,
        //에러
    }
    public enum PacketSendERROR
    {
        정상 = 0,
        에러
    }

    [Serializable]
    public class Packet
    {
        public int Length;
        public int Type;

        public Packet()
        {
            this.Length = 0;
            this.Type = 0;
        }
        public static byte[] Serialize(Object o)
        {
            MemoryStream ms = new MemoryStream(1024 * 1 * 4);
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(ms, o);//0->o
            return ms.ToArray();
        }
        public byte[] serialize()
        {
            byte[] b = null;

            try
            {
                using (MemoryStream memory = new MemoryStream(1024 * 1 * 4))
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
        public static Object Desserialize(byte[] bt)
        {
            MemoryStream ms = new MemoryStream(1024 * 4);
            foreach (byte b in bt)
            {
                ms.WriteByte(b);
            }
            ms.Position = 0;
            BinaryFormatter bf = new BinaryFormatter();
            Object obj = bf.Deserialize(ms);
            ms.Close();
            return obj;
        }
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
    [Serializable]
    public class Initialize : Packet
    {
        public int Data = 0;
    }
    [Serializable]
    public class Join : Packet
    {
        public string m_strID = "";
        //public string Id = "";
        public string m_strPassword = "";
    }
    [Serializable]
    public class Login : Packet
    {
        public string m_strID = "";
        //public string Id = "";
        public string m_strPassword = "";
        public Login()
        {
            this.m_strID = null;
        }

    }
    [Serializable]
    public class Error : Packet
    {
        public int Data = 0;
        public string str = "";
    }
    [Serializable]
    public class Search : Packet
    {
        public int Data = 0;
        public string m_strID = "";
        public List<string> m_list;
    }

    [Serializable]
    public class Upload : Packet
    {

        //public FileStream m_file;
        public int Data = 0;
        public string m_strID = "";
        public string m_message = "";
        public string m_filename = "";
        public List<string> m_list;
        public byte[] m_byte;

    }
}
