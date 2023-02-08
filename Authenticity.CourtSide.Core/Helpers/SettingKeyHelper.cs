using Microsoft.Extensions.Configuration;
using System;

namespace Authenticity.CourtSide.Core.Helpers
{
    public static class SettingKeyHelper
    { 
        public static T GetSettingKey<T>(IConfiguration configuration, string key) 
        { 
            T value; 
            try 
            { 
                value = configuration.GetValue<T>(key); 
            } 
            catch (Exception) 
            { 
                value = default(T); } return value; 
        } 
    }
}
