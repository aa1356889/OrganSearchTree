using System.Web.Security;




namespace Hammer.Encrypt
{
	public static  class MD5
	{


        public static string Encode(string s)
        {
            return FormsAuthentication.HashPasswordForStoringInConfigFile(s, "MD5");
        }
        
        /// <summary>
        /// 将一组字符串进行MD5加密
        /// </summary>
        /// <param name="strs"></param>
        /// <returns></returns>
        public static string Encode(params string[] strs)
        {
            System.Text.StringBuilder strings =new System.Text.StringBuilder();
            if (strs == null || strs.Length == 0)
            {
                return string.Empty;
            }
            for (int i = 0; i < strs.Length; i++)
			{
                strings.Append(strs[i]);
			}
            return Encode(strings.ToString());
          
        }
	}
}
