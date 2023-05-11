using System.Threading;
using System.Collections.Generic;

namespace VoxelMapsTestTask
{
    public class TokenManager
    {
        private static Dictionary<string, CancellationTokenSource> dictionary = new Dictionary<string, CancellationTokenSource>();



        public static void RegisterCancellationToken(string hubConnectionId, CancellationTokenSource cancellationTokenSource)
        {
            var hasVal = dictionary.ContainsKey(hubConnectionId);
            if (!hasVal)
            {
                dictionary.Add(hubConnectionId, cancellationTokenSource);
            }
            else
            {
                dictionary.Remove(hubConnectionId);
                dictionary.Add(hubConnectionId, cancellationTokenSource);
            }
        }
        public static CancellationTokenSource GetCancellationTokenSource(string hubConnectionId)
        {
            return dictionary.GetValueOrDefault(hubConnectionId);
        }

        public static void RemoveCancellationTokenSource(string hubConnectionId)
        {
            dictionary.Remove(hubConnectionId);
        }
    }
}
