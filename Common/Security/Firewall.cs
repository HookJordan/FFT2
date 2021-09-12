using System.Collections.Generic;

namespace Common.Security
{
    public class Firewall
    {
        // TODO: Connection limits, rate limits, encryption requirements, version verification
        public HashSet<string> IP_BLOCK_LIST { get; private set; }

        public Firewall()
        {
            IP_BLOCK_LIST = new HashSet<string>();
        }

        public void BlockIpAddress(string ipAddress)
        {
            IP_BLOCK_LIST.Add(ipAddress);
        }

        public void AllowIpAddress(string ipAddress)
        {
            IP_BLOCK_LIST.Remove(ipAddress);
        }

        public bool IsIpBlocked(string ipAddress)
        {
            // TODO: Add logic for wildcards and subnets
            return IP_BLOCK_LIST.Contains(ipAddress);
        }
    }
}
