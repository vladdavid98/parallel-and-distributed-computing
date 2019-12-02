using System.Collections.Generic;
using Futures_and_continuations.implementation;

namespace Futures_and_continuations
{
    public static class Program
    {
        // adding 3 hosts, each returning a response in different format:
        private static readonly List<string> Hosts = new List<string> {
            // - gzip form (compressed)
            "www.cs.ubbcluj.ro/~rlupsa/edu/pdp",
            // - empty body (just signals that the page has moved and the HTTPS protocol should be used from now on)
            "facebook.com",
            // - plain text
            "google.com",
        };

        public static void Main(string[] args)
        {
            DirectCallbacks.Run(Hosts);
//            TaskMechanism.Run(Hosts);
//            AsyncTaskMechanism.Run(Hosts);
        }
    }
}