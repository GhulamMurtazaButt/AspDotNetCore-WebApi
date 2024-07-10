using System.Text;

namespace WebApplication2.Utilities.Base64Methods.Impl
{
    public class Base64: IBase64
    {
        public string Base64Encode(string plainText)
        {
            try
            {
                byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);
                return Convert.ToBase64String(plainTextBytes);
            }
            catch(Exception exp){
                return exp.Message;
            }
        }
        public string Base64Decode(string base64EncodedData)
        {
            try
            {
                byte[] base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
                return Encoding.UTF8.GetString(base64EncodedBytes);
            }
            catch (Exception exp)
            {
                return exp.Message;
            }

        }

    }
}
