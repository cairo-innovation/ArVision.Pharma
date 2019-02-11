

namespace ArVision.Service.Pharma.Shared
{
    public static class PharmaServicePort
    {
        private static readonly int UDP_PORT_OFFSET = 2;

        public static readonly int TCP_PORT = 9080;
        //public static readonly int TCP_PORT = 5671;
        public static readonly int UDP_PORT = TCP_PORT + UDP_PORT_OFFSET;
    }
}
