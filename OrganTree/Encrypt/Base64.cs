using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using zlib;

namespace Hammer.Encrypt
{
    public   class Base64
    {
        public  string Encode(string str)
        {

            byte[] b = Utf8ToBytes(str);
            byte[] bb = Compress(b);
            string str1 = Base64Encode(bb);
            return str1;
        }

        /// <summary>
        ///  从流转为Base64
        /// </summary>
        /// <param name="sr"></param>
        /// <returns></returns>
        public string Encode(Stream sr)
        {
            
            byte [] b = new byte[sr.Length];
            
            sr.Read(b, 0, b.Length);
            sr.Close();
            byte[] bb = Compress(b);
            string str1 = Base64Encode(bb);
            return str1;
        }

        /// <summary>
        ///  从Base64转回到流
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public Stream DecodeToStream(string str)
        {
            byte[] b = Convert.FromBase64String(str);

            byte[] bbb = Decompress(b);

            Stream sr = new MemoryStream();
            sr.Write(bbb, 0, bbb.Length);
            return sr;
        }

        public  string Decode(string str)
        {


            byte[] b = Convert.FromBase64String(str);

            byte[] bbb = Decompress(b);
            str = BytesToUtf8(bbb);

            return str;
        }

        private   byte[] Compress(Byte[] bytes)
        {
            try
            {
                using (MemoryStream ms = new MemoryStream(bytes))
                {
                    MemoryStream tempMs = new MemoryStream();
                    zlib.ZOutputStream outZStream = new zlib.ZOutputStream(tempMs, zlib.zlibConst.Z_DEFAULT_COMPRESSION);
                    CopyStream(ms, outZStream);
                    ms.Close();
                    outZStream.Close();
                    return tempMs.ToArray();
                }
            }
            catch (Exception)
            {
                throw;
            }

        }

        private  byte[] Decompress(Byte[] bytes)
        {
            try
            {
                using (MemoryStream ms = new MemoryStream(bytes))
                {
                    MemoryStream tempMs = new MemoryStream();
                    ZOutputStream outZStream = new ZOutputStream(tempMs);
                    CopyStream(ms, outZStream);
                    ms.Close();
                    //tempMs.Close();
                    return tempMs.ToArray();
                }
            }
            catch (Exception)
            {
                throw;
            }

        }

        private  void CopyStream(System.IO.Stream input, System.IO.Stream output)
        {
            byte[] buffer = new byte[input.Length];
            int len;
            while ((len = input.Read(buffer, 0, buffer.Length)) > 0)
            {
                output.Write(buffer, 0, len);
            }
            output.Flush();
            input.Dispose();
        }

        private byte[] Utf8ToBytes(string str)
        {
            return System.Text.Encoding.UTF8.GetBytes(str);
        }
        private  string Base64Encode(byte[] b)
        {
            return Convert.ToBase64String(b);
        }

        private string BytesToUtf8(byte[] b)
        {
            return System.Text.Encoding.UTF8.GetString(b);
        }
    }

}
