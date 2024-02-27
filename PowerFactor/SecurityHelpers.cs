using System.Security.Cryptography;
using System.Text;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Parameters;
using System.Xml;
using PowerFactor.Requests;

namespace PowerFactor;

public static class SecurityHelpers
{
    private static readonly string PrivateKey = "<RSAKeyValue><Modulus>i2d12.............";
    private static readonly string PwfPublicKey = "<RSAKeyValue><Modulus>g31t............"; 
    private static readonly byte[] aesIV = new byte[] { 15, 111, 19, 46, 53, 194, 205, 249, 5, 70, 156, 234, 168, 75, 115, 204 };
    private const string RandomKeyChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/";

    public static PWFRequest EncryptRequest(string request)
    {
        string aesIVString = GenerateRandomKey();
        string aesKeyString = GenerateRandomKey();

        string token = aesIVString + aesKeyString;

        var pwfRequest = new PWFRequest
        {
            CipherText = AesEncryptGCM(request, aesIVString, aesKeyString),
            SecretKey = RSAEncryption(token)
        };
        
        pwfRequest.VerificationSignature = Sign(pwfRequest.CipherText);
        return pwfRequest;
    }
    
    public static string AESEncryption(string plainText, string token, string salt = "")
    {
        using RijndaelManaged aesProvider = new RijndaelManaged();
        byte[] keyByte = GetLegalKey(aesProvider, token, salt);
        aesProvider.Key = keyByte;
        aesProvider.IV = aesIV;
        aesProvider.Padding = PaddingMode.PKCS7;
        ICryptoTransform cryptoTransform = aesProvider.CreateEncryptor();
        using MemoryStream memoryStream = new MemoryStream();
        using CryptoStream cryptoStream = new CryptoStream(memoryStream, cryptoTransform, CryptoStreamMode.Write);
        byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);
        cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
        cryptoStream.FlushFinalBlock();
        byte[] cipherTextBytes = memoryStream.ToArray();
        return Convert.ToBase64String(cipherTextBytes);
    }
    public static string AesEncryptGCM(string plainText, string aesIVString, string aesKeyString)
    {
        byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);
        byte[] keyBytes = Encoding.ASCII.GetBytes(aesKeyString);
        byte[] ivBytes = Encoding.ASCII.GetBytes(aesIVString);

        GcmBlockCipher cipher = new GcmBlockCipher(new AesEngine());

        AeadParameters parameters = new AeadParameters(new KeyParameter(keyBytes), 128, ivBytes);
        cipher.Init(true, parameters);

        byte[] cipherText = new byte[cipher.GetOutputSize(plainTextBytes.Length)];
        int len = cipher.ProcessBytes(plainTextBytes, 0, plainTextBytes.Length, cipherText, 0);
        len += cipher.DoFinal(cipherText, len);

        return Convert.ToBase64String(cipherText, 0, len);
    }
    
    public static string AESDecryption(string cipherText, string token, string salt = "")
    {
        byte[] cipherTextBytes = Convert.FromBase64String(cipherText);

        byte[] keyBytes = Encoding.ASCII.GetBytes(token.Substring(16, 16));
        byte[] ivBytes = Encoding.ASCII.GetBytes(token.Substring(0, 16));
        byte[] saltBytes = Encoding.ASCII.GetBytes(salt);

        GcmBlockCipher cipher = new GcmBlockCipher(new AesEngine());
        AeadParameters parameters = new AeadParameters(new KeyParameter(keyBytes), 128, ivBytes, saltBytes);
        cipher.Init(false, parameters);

        byte[] plainText = new byte[cipher.GetOutputSize(cipherTextBytes.Length)];
        int len = cipher.ProcessBytes(cipherTextBytes, 0, cipherTextBytes.Length, plainText, 0);
        len += cipher.DoFinal(plainText, len);
        return Encoding.UTF8.GetString(plainText, 0, len);
    }
    
    public static string RSAEncryption(string data)
    {
        var RSA = new RSACryptoServiceProvider();
        RSA.FromXmlString(PwfPublicKey);
        byte[] byt = Encoding.UTF8.GetBytes(data);
        var strModified = Convert.ToBase64String(byt);
        var bytesPlainTextData = Convert.FromBase64String(strModified);
        var bytesCypherText = RSA.Encrypt(bytesPlainTextData, RSAEncryptionPadding.OaepSHA1);
        return Convert.ToBase64String(bytesCypherText);
    }
    
    public static string RSADecryption(string data)
    {
        var rsa = new RSACryptoServiceProvider();
        rsa.FromXmlString(PrivateKey);
        var bytesPlainTextData = Convert.FromBase64String(data);
        var result = rsa.Decrypt(bytesPlainTextData, RSAEncryptionPadding.OaepSHA1);
        return Encoding.UTF8.GetString(result);
    }

    public static string Sign(string cipherText, string hashAlgorithm = "SHA256")
    {
        var RSA = new RSACryptoServiceProvider();
        RSA.FromXmlString(PrivateKey);
        byte[] hash = RSA.SignData(Encoding.UTF8.GetBytes(cipherText), (HashAlgorithm)CryptoConfig.CreateFromName(hashAlgorithm));
        return Convert.ToBase64String(hash);
    }
    
    public static bool VerifySign(string cipherText, string verificationSignature, string hashAlgorithm = "SHA256")
    {
        using RSACryptoServiceProvider rsaProvider = new RSACryptoServiceProvider();
        rsaProvider.ImportParameters(GetRSAParametersByXmlString(PwfPublicKey));
        byte[] cipherTextBytes = Encoding.UTF8.GetBytes(cipherText);
        byte[] cipherTextHashBytes = Convert.FromBase64String(verificationSignature);
        bool isVerified = rsaProvider.VerifyData(cipherTextBytes, (HashAlgorithm)CryptoConfig.CreateFromName(hashAlgorithm), cipherTextHashBytes);
        if (!isVerified)
            return false;

        return true;
    }

    public static RSAParameters GetRSAParametersByXmlString(string xmlString)
    {
        RSAParameters parameters = new RSAParameters();
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(xmlString);
        if (xmlDoc.DocumentElement is { Name: "RSAKeyValue" })
        {
            foreach (XmlNode node in xmlDoc.DocumentElement.ChildNodes)
            {
                switch (node.Name)
                {
                    case "Modulus": parameters.Modulus = (string.IsNullOrEmpty(node.InnerText) ? null : Convert.FromBase64String(node.InnerText)); break;
                    case "Exponent": parameters.Exponent = (string.IsNullOrEmpty(node.InnerText) ? null : Convert.FromBase64String(node.InnerText)); break;
                    case "P": parameters.P = (string.IsNullOrEmpty(node.InnerText) ? null : Convert.FromBase64String(node.InnerText)); break;
                    case "Q": parameters.Q = (string.IsNullOrEmpty(node.InnerText) ? null : Convert.FromBase64String(node.InnerText)); break;
                    case "DP": parameters.DP = (string.IsNullOrEmpty(node.InnerText) ? null : Convert.FromBase64String(node.InnerText)); break;
                    case "DQ": parameters.DQ = (string.IsNullOrEmpty(node.InnerText) ? null : Convert.FromBase64String(node.InnerText)); break;
                    case "InverseQ": parameters.InverseQ = (string.IsNullOrEmpty(node.InnerText) ? null : Convert.FromBase64String(node.InnerText)); break;
                    case "D": parameters.D = (string.IsNullOrEmpty(node.InnerText) ? null : Convert.FromBase64String(node.InnerText)); break;
                }
            }
        }
        else
        {
            throw new Exception("Invalid XML RSA key.");
        }

        return parameters;
    }
    
    public static string GenerateRandomKey(int count = 16)
    {
        using var rng = new RNGCryptoServiceProvider();
        var data = new byte[count];
        rng.GetBytes(data);
        var allowable = RandomKeyChars.ToCharArray();
        var l = allowable.Length;
        var chars = new char[count];
        for (var i = 0; i < count; i++)
            chars[i] = allowable[data[i] % l];

        return new string(chars);
    }
    
    private static byte[] GetLegalKey(RijndaelManaged aesProvider, string key, string salt)
    {
        byte[] legalKey = Convert.FromBase64String(key);
        if (legalKey.Length == 32) // added for performance
            return legalKey;

        if (aesProvider.LegalKeySizes.Length > 0)
        {
            int keySize = key.Length * 8;
            int minSize = aesProvider.LegalKeySizes[0].MinSize;
            int maxSize = aesProvider.LegalKeySizes[0].MaxSize;
            int skipSize = aesProvider.LegalKeySizes[0].SkipSize;
            if (keySize > maxSize)
            {
                key = key.Substring(0, maxSize / 8);
            }
            else if (keySize < maxSize)
            {
                int validSize = (keySize <= minSize) ? minSize : (keySize - keySize % skipSize) + skipSize;
                if (keySize < validSize)
                {
                    key = key.PadRight(validSize / 8, '*');
                }
            }
        }
        PasswordDeriveBytes password = new PasswordDeriveBytes(key, Encoding.UTF8.GetBytes(salt));
        return password.GetBytes(key.Length);
    }

    public static string Base64Encode(string plainText)
    {
        var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
        return Convert.ToBase64String(plainTextBytes);
    }


}