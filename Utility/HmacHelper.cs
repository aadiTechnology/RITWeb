using System;
using System.Security.Cryptography;
using System.Text;
using System.Linq;
using System.Collections.Generic;

/// <summary>
/// Summary description for HmacHelper
/// </summary>
public class HmacHelper
{
    public static string GetHashValue(string key, string message, HMACTypes type)
    {
        string response = "";
        UTF8Encoding encoding = new UTF8Encoding();

        byte[] keyByte = encoding.GetBytes(key);
        byte[] messageBytes = encoding.GetBytes(message);

        switch (type)
        {
            case HMACTypes.HMAC_MD5:
                using (var hmacmd = new HMACMD5(keyByte))
                {
                    response = ByteToString(hmacmd.ComputeHash(messageBytes));
                }
                break;
            case HMACTypes.HMAC_SHA1:
                using (var hmacmd = new HMACSHA1(keyByte))
                {
                    response = ByteToString(hmacmd.ComputeHash(messageBytes));
                }
                break;
            case HMACTypes.HMAC_SHA256:
                using (var hmacmd = new HMACSHA256(keyByte))
                {
                    response = ByteToString(hmacmd.ComputeHash(messageBytes));
                }
                break;
            case HMACTypes.HMAC_SHA384:
                using (var hmacmd = new HMACSHA384(keyByte))
                {
                    response = ByteToString(hmacmd.ComputeHash(messageBytes));
                }
                break;
            case HMACTypes.HMAC_SHA512:
                using (var hmacmd = new HMACSHA512(keyByte))
                {
                    response = ByteToString(hmacmd.ComputeHash(messageBytes));
                }
                break;
        }
        return response;
    }
	
    private static string ByteToString(byte[] buff)
    {
        return buff.Aggregate("", (current, t) => current + t.ToString("X2"));
    }
	
    public static string getsecurehash()
    {
        // create a new sorted dictionary of strings, with string
        // keys.
        SortedDictionary<string, string> txnmsgparams = 
            new SortedDictionary<string, string>();		
			
        // add some elements to the dictionary. there should be no 
        // duplicate keys.
        txnmsgparams.Add("amount", "122.00");
        txnmsgparams.Add("currencycode", "356");
        txnmsgparams.Add("merchantid", "t_00038");
        txnmsgparams.Add("txnrefno", "12345");
        // form the hashinput
        SortedDictionary<string, string>.ValueCollection valuecoll = 
            txnmsgparams.Values;	

        //console.writeline();
        string hashinput = "";
        foreach( string s in valuecoll )
        {
            hashinput = hashinput + s;
        }
       // console.writeline("hashinput is >>" + hashinput);
		
        // form the securehash
        ////string securehash = GetHashValue("merchantkey", hashinput, HMACTypes.HMAC_SHA256);

        string securehash = GetHashValue("HmacSHA256", "aaabcxyz", HMACTypes.HMAC_SHA256);

        //console.writeline("securehash is >>" + securehash);
        return securehash;
    }
}

public enum HMACTypes
{
    HMAC_MD5 = 0,
    HMAC_SHA1,
    HMAC_SHA256,
    HMAC_SHA384,
    HMAC_SHA512
}