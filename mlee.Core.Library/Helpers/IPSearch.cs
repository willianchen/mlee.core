using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace mlee.Core.Library.Helpers
{
    ///<summary>
    /// 提供从纯真IP数据库搜索IP信息的方法；
    ///</summary>
    public class IPSearch
    {
        FileStream fileStream = null;
        static long[] ipArray = null;
        static long initPosition = 0;
        long ip;
        static MemoryStream ipFile = null;
        ///<summary>
        /// 构造函数
        ///</summary>
        public IPSearch(string filePath)
        {
            if (ipArray == null)
            {
                lock ("ip")
                {
                    if (ipArray == null)
                    {
                        fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
                        ipArray = BlockToArray(ReadIPBlock());
                        initPosition = fileStream.Position;
                        fileStream.Position = 0;
                        byte[] bytes = new byte[fileStream.Length];
                        fileStream.Read(bytes, 0, bytes.Length);

                        ipFile = new MemoryStream(bytes);
                        fileStream.Close();
                        fileStream = null;
                        bytes = null;
                    }
                }
            }
            ipFile.Position = initPosition;
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);//使.net core支持gb2312编码
        }

        /// <summary>
        /// 更新ip数据文件后,对内存中的流进行更新
        /// </summary>
        public void ClearStream()
        {
            ipFile.Close();
            ipFile = null;
            ipArray = null;
            initPosition = 0;
        }

        ///<summary>
        /// 地理位置,包括国家和地区
        ///</summary>
        public struct IPLocation
        {
            public string country, area;
        }

        ///<summary>
        /// 获取指定IP所在地理位置
        ///</summary>
        ///<param name="strIP">要查询的IP地址</param>
        ///<returns></returns>
        public IPLocation GetIPLocation(string strIP)
        {
            ip = IPToLong(strIP);

            long offset = SearchIP(ipArray, 0, ipArray.Length - 1) * 7 + 4;
            ipFile.Position += offset;//跳过起始IP
            ipFile.Position = ReadLongX(3) + 4;//跳过结束IP

            IPLocation loc = new IPLocation();
            int flag = ipFile.ReadByte();//读取标志
            if (flag == 1)//表示国家和地区被转向
            {
                ipFile.Position = ReadLongX(3);
                flag = ipFile.ReadByte();//再读标志
            }
            long countryOffset = ipFile.Position;
            loc.country = ReadString(flag);

            if (flag == 2)
            {
                ipFile.Position = countryOffset + 3;
            }
            flag = ipFile.ReadByte();
            loc.area = ReadString(flag);

            return loc;
        }
        ///<summary>
        /// 将字符串形式的IP转换位long
        ///</summary>
        ///<param name="strIP"></param>
        ///<returns></returns>
        public long IPToLong(string strIP)
        {
            byte[] ip_bytes = new byte[8];
            string[] strArr = strIP.Split(new char[] { '.' });
            for (int i = 0; i < 4; i++)
            {
                ip_bytes[i] = byte.Parse(strArr[3 - i]);
            }
            return BitConverter.ToInt64(ip_bytes, 0);
        }
        ///<summary>
        /// 将索引区字节块中的起始IP转换成Long数组
        ///</summary>
        ///<param name="ipBlock"></param>
        long[] BlockToArray(byte[] ipBlock)
        {
            long[] ipArray = new long[ipBlock.Length / 7];
            int ipIndex = 0;
            byte[] temp = new byte[8];
            for (int i = 0; i < ipBlock.Length; i += 7)
            {
                Array.Copy(ipBlock, i, temp, 0, 4);
                ipArray[ipIndex] = BitConverter.ToInt64(temp, 0);
                ipIndex++;
            }
            return ipArray;
        }
        ///<summary>
        /// 从IP数组中搜索指定IP并返回其索引
        ///</summary>
        ///<param name="ipArray">IP数组</param>
        ///<param name="start">指定搜索的起始位置</param>
        ///<param name="end">指定搜索的结束位置</param>
        ///<returns></returns>
        int SearchIP(long[] ipArray, int start, int end)
        {
            int middle = (start + end) / 2;
            if (middle == start)
                return middle;
            else if (ip < ipArray[middle])
                return SearchIP(ipArray, start, middle);
            else
                return SearchIP(ipArray, middle, end);
        }
        ///<summary>
        /// 读取IP文件中索引区块
        ///</summary>
        ///<returns></returns>
        byte[] ReadIPBlock()
        {
            long startPosition = StreamReadLongX(4);
            long endPosition = StreamReadLongX(4);
            long count = (endPosition - startPosition) / 7 + 1;//总记录数
            fileStream.Position = startPosition;
            byte[] ipBlock = new byte[count * 7];
            fileStream.Read(ipBlock, 0, ipBlock.Length);
            fileStream.Position = startPosition;
            return ipBlock;
        }
        ///<summary>
        /// 从IP文件中读取指定字节并转换位long
        ///</summary>
        ///<param name="bytesCount">需要转换的字节数，主意不要超过8字节</param>
        ///<returns></returns>
        long StreamReadLongX(int bytesCount)
        {
            byte[] _bytes = new byte[8];
            fileStream.Read(_bytes, 0, bytesCount);
            return BitConverter.ToInt64(_bytes, 0);
        }


        ///<summary>
        /// 从IP文件中读取指定字节并转换位long
        ///</summary>
        ///<param name="bytesCount">需要转换的字节数，主意不要超过8字节</param>
        ///<returns></returns>
        long ReadLongX(int bytesCount)
        {
            byte[] _bytes = new byte[8];
            ipFile.Read(_bytes, 0, bytesCount);

            return BitConverter.ToInt64(_bytes, 0);
        }
        ///<summary>
        /// 从IP文件中读取字符串
        ///</summary>
        ///<param name="flag">转向标志</param>
        ///<returns></returns>
        string ReadString(int flag)
        {
            if (flag == 1 || flag == 2)//转向标志
                ipFile.Position = ReadLongX(3);
            else
                ipFile.Position -= 1;

            List<byte> list = new List<byte>();
            byte b = (byte)ipFile.ReadByte();
            while (b > 0)
            {
                list.Add(b);
                b = (byte)ipFile.ReadByte();
            }

            return Encoding.GetEncoding("gb2312").GetString(list.ToArray());
        }
    }
}
