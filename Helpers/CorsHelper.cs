using System;
using System.Collections.Generic;

namespace MacMarketGroupApi.Services;

public class CorsHelper
{

    public string GenerateUniqueId(int length)
    {
        const string chars = "0123456789";
        var random = new Random();
        var uniqueId = new List<char>();

        while (uniqueId.Count < length)
        {
            char randomChar = chars[random.Next(chars.Length)];
            uniqueId.Add(randomChar);
        }
        return new string(uniqueId.ToArray());
    }
}


