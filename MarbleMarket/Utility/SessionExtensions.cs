using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;

namespace MarbleMarket.Utility
{
    // Extension method always has static class  
    public static class SessionExtensions
    {
        //Create getter and setter  which will implement session extension to store the complex object by
        //serializing an deserializing them.
        // Now need key to access the session  will store in WC file
        public static void Set<T>(this ISession session, string Key, T value)
        {
            session.SetString(Key, JsonSerializer.Serialize(value));
        }

        public static T Get<T>(this ISession session, string Key)
        {
            var value = session.GetString(Key);
            return value == null ? default : JsonSerializer.Deserialize<T>(value);
           

        }
    }
}
