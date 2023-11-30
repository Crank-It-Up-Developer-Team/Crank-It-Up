using System;
using Newtonsoft.Json.Linq;

namespace CrankItUp.Game
{
    public static class JobjectGetValueOrFail
    {
        public static JToken GetValueOrFail(this JToken token, string name)
        {
            if (
                token
                    .ToObject<JObject>()
                    .TryGetValue(name, StringComparison.CurrentCulture, out JToken value) == false
            )
            {
                throw new ArgumentException("Value not found!");
            }
            return value;
        }

        public static T GetValueOrFail<T>(this JToken token, string name)
        {
            if (
                token
                    .ToObject<JObject>()
                    .TryGetValue(name, StringComparison.CurrentCulture, out JToken value) == false
            )
            {
                throw new ArgumentException("Value not found!");
            }
            return value.Value<T>();
        }
    }
}
