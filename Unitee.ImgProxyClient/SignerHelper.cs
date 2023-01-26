using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Unitee.ImgProxyClient;

internal static class SignerHelper
{
    public static string SignPath(string key, string salt, string path)
    {
        var keybin = HexadecimalStringToByteArray(key);
        var saltBin = HexadecimalStringToByteArray(salt);

        var passwordWithSaltBytes = new List<byte>();
        passwordWithSaltBytes.AddRange(saltBin);
        passwordWithSaltBytes.AddRange(Encoding.UTF8.GetBytes(path));

        using var hmac = new HMACSHA256(keybin);
        byte[] digestBytes = hmac.ComputeHash(passwordWithSaltBytes.ToArray());
        var urlSafeBase64 = EncodeBase64URLSafeString(digestBytes);
        return $"/{urlSafeBase64}{path}";
    }

    static byte[] HexadecimalStringToByteArray(string input)
    {
        var outputLength = input.Length / 2;
        var output = new byte[outputLength];
        using (var sr = new StringReader(input))
        {
            for (var i = 0; i < outputLength; i++)
                output[i] = Convert.ToByte(new string(new char[2] { (char)sr.Read(), (char)sr.Read() }), 16);
        }
        return output;
    }

    public static string EncodeBase64URLSafeString(this byte[] stream)
        => Convert.ToBase64String(stream).TrimEnd('=').Replace('+', '-').Replace('/', '_');

    public static string EncodeBase64URLSafeString(this string str)
        => EncodeBase64URLSafeString(Encoding.UTF8.GetBytes(str));
}