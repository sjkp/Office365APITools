using System;
using System.Collections;
using System.Linq;
using System.Web;
using Microsoft.Office365.OAuth;

namespace Office365APIToolsSample
{
    public class FixedSessionCache : ICache
    {
        private const string _prefix = "_O365#";

        public string Read(string key, out DateTime expiration)
        {
            FixedSessionCache.CacheValue cacheValue = HttpContext.Current.Session["_O365#" + key] as FixedSessionCache.CacheValue;
            if (cacheValue == null)
            {
                expiration = DateTime.UtcNow.AddMinutes(-10);
                return (string)null;
            }
            else
            {
                expiration = cacheValue.ExpiresOn;
                return cacheValue.Value;
            }
        }

        public void Write(string key, string value, DateTime expiration)
        {
            FixedSessionCache.CacheValue cacheValue = new FixedSessionCache.CacheValue()
            {
                Value = value,
                ExpiresOn = DateTime.UtcNow.AddMinutes(10)
            };
            HttpContext.Current.Session["_O365#" + key] = (object)cacheValue;
        }

        public void Delete(string key)
        {
            HttpContext.Current.Session.Remove("_O365#" + key);
        }

        public void Clear()
        {
            foreach (string name in Enumerable.ToArray<string>(Enumerable.Where<string>(Enumerable.OfType<string>((IEnumerable)HttpContext.Current.Session.Keys), (Func<string, bool>)(i => i.StartsWith("_O365#")))))
                HttpContext.Current.Session.Remove(name);
        }

        private class CacheValue
        {
            public string Value;
            public DateTime ExpiresOn;
        }

    }
}