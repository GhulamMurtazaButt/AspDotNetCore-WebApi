namespace WebApplication2.Utilities.Base64Methods
{
    public interface IBase64
    {
        public string Base64Encode(string plainText);
        public string Base64Decode(string base64EncodedData);
    }
}
