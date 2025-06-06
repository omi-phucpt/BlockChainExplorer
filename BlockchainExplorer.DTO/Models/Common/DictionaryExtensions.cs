using System.Collections.Generic;

namespace BlockchainExplorer.Extensions
{
    public static class DictionaryExtensions
    {
        public static Dictionary<string, string> AddIfNotNull(this Dictionary<string, string> dict, string key, string? value)
        {
            if (!string.IsNullOrEmpty(value))
                dict[key] = value;
            return dict;
        }
    }
}