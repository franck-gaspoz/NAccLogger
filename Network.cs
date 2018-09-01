using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace NAccLogger
{
    public static class Network
    {
        /// <summary>
        /// true if network tests (get current active adapter and host ip) have been performed
        /// </summary>
        public static bool NetworkChecked = false;

        static NetworkInterface _CurrentActiveNetworkAdapter = null;

        /// <summary>
        /// current and active network adapter
        /// </summary>
        public static NetworkInterface CurrentActiveNetworkAdapter
        {
            get
            {
                CheckNetwork();
                return _CurrentActiveNetworkAdapter;
            }
        }
        
        static string _HostName = null;
        
        /// <summary>
        /// current host name
        /// </summary>
        public static string HostName
        {
            get
            {
                CheckNetwork();
                return _HostName;
            }
        }

        static IPAddress _IP_V4 = null;

        /// <summary>
        /// current IP V4
        /// </summary>
        public static IPAddress IP_V4 {
            get {
                CheckNetwork();
                return _IP_V4;
            }
        }

        static IPAddress _IP_V6 = null;

        /// <summary>
        /// current IP V6
        /// </summary>
        public static IPAddress IP_V6
        {
            get
            {
                CheckNetwork();
                return _IP_V6;
            }
        }

        /// <summary>
        /// get information from network if NetworjChecked is set to false
        /// </summary>
        static void CheckNetwork()
        {
            if (NetworkChecked)
                return;
            _HostName = Dns.GetHostName();
            _CurrentActiveNetworkAdapter =
                GetActiveAdapter();            
            if (_CurrentActiveNetworkAdapter!=null)
            {
                _IP_V6 = _CurrentActiveNetworkAdapter
                    .GetIPProperties()
                    .UnicastAddresses
                    .Where(
                        x => x.Address.AddressFamily
                            == AddressFamily.InterNetworkV6)
                            .FirstOrDefault()
                            ?.Address;
                _IP_V4 = _CurrentActiveNetworkAdapter
                    .GetIPProperties()
                    .UnicastAddresses
                    .Where(
                        x => x.Address.AddressFamily
                            == AddressFamily.InterNetwork )
                            .FirstOrDefault()
                            ?.Address;
            }

            NetworkChecked = true;
        }

        /// <summary>
        /// get the current active interface
        /// <para>contact the provided ip to get the information. default is twitter's one</para>
        /// </summary>
        /// <param name="testIP">contacted ip for performing tests</param>
        /// <returns>active inteface name if solved, else null</returns>
        public static NetworkInterface GetActiveAdapter(
            string testIP = "199.59.149.230"
            )
        {
            string TwitterIP = "199.59.149.230";
            UdpClient u = new UdpClient(TwitterIP, 1);
            IPAddress localAddr = ((IPEndPoint)u.Client.LocalEndPoint).Address;

            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                IPInterfaceProperties ipProps = nic.GetIPProperties();
                foreach (UnicastIPAddressInformation addrinfo in ipProps.UnicastAddresses)
                {
                    if (localAddr.Equals(addrinfo.Address))
                    {
                        return nic;
                    }
                }
            }
            return null;
        }
    }
}
