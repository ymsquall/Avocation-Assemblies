public class MD5Utils
{
    public static string GetMD5(byte[] buff)
    {
        string sTemp = "";
        try
        {
            System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] bytHash = md5.ComputeHash(buff);
            md5.Clear();
            for (int i = 0; i < bytHash.Length; i++)
            {
                sTemp += bytHash[i].ToString("X").PadLeft(2, '0');
            }
        }
        catch (System.Exception ex)
        {
            throw new System.Exception("GetMD5(byte[]) fail, error:" + ex.Message);
        }
        return sTemp.ToLower();
    }
    public static string GetMD5(string sDataIn)
    {
        string sTemp = "";
        try
        {
            System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] bytValue, bytHash;
            bytValue = System.Text.Encoding.UTF8.GetBytes(sDataIn);
            bytHash = md5.ComputeHash(bytValue);
            md5.Clear();
            for (int i = 0; i < bytHash.Length; i++)
            {
                sTemp += bytHash[i].ToString("X").PadLeft(2, '0');
            }
        }
        catch (System.Exception ex)
        {
            throw new System.Exception("GetMD5(string) fail, error:" + ex.Message);
        }
        return sTemp.ToLower();
    }
    public static string GetMD5HashFromFile(string fileName)
    {
        try
        {
            System.IO.FileStream file = new System.IO.FileStream(fileName, System.IO.FileMode.Open);
            System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] retVal = md5.ComputeHash(file);
            file.Close();
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            for (int i = 0; i < retVal.Length; i++)
            {
                sb.Append(retVal[i].ToString("x2"));
            }
            return sb.ToString();
        }
        catch (System.Exception ex)
        {
            throw new System.Exception("GetMD5HashFromFile(string) fail, error:" + ex.Message);
        }
    }
}