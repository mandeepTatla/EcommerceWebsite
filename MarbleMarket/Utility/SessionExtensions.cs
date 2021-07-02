﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;

namespace MarbleMarket.Utility
{
    public static class SessionExtensions
    {
        public static void Set<T>(this ISession session, string Key, T value)
        {
            session.SetString(Key, JsonSerializer.Serialize(value));
        }

        public static T Get<T>(this ISession session, string Key)
        {
            var value = session.GetString(Key);
            return value == null ? default : JsonSerializer.Deserialize<T>(value);
            session.SetString(Key, JsonSerializer.Serialize(value));

        }
    }
}
