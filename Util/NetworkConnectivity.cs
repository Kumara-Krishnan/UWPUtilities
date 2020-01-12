using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking.Connectivity;

namespace UWPUtilities.Util
{
    public delegate void NetworkStatusChangedEventHandler(NetworkInformation networkInfo);

    public sealed class NetworkConnectivity
    {
        public static NetworkConnectivity Instance { get { return NetworkConnectivitySingleton.Instance; } }

        public static event NetworkStatusChangedEventHandler NetworkStatusChanged;

        public static bool IsInternetAvailable { get { return Instance.NetworkInformation.IsInternetAvailable; } }

        public readonly NetworkInformation NetworkInformation = new NetworkInformation();

        private NetworkConnectivity()
        {
            Windows.Networking.Connectivity.NetworkInformation.NetworkStatusChanged += OnNetworkStatusChanged;
            UpdateNetworkStatus();
        }

        ~NetworkConnectivity()
        {
            Windows.Networking.Connectivity.NetworkInformation.NetworkStatusChanged -= OnNetworkStatusChanged;
        }

        public void UpdateNetworkStatus()
        {
            lock (NetworkInformation)
            {
                try
                {
                    NetworkInformation.UpdateNetworkInformation(Windows.Networking.Connectivity.NetworkInformation.GetInternetConnectionProfile());
                }
                catch
                {
                    NetworkInformation.Reset();
                }

                if (NetworkInformation.PreviouslyInternetAvailable != default)
                {
                    NetworkStatusChanged?.Invoke(NetworkInformation);
                }
            }
        }

        private void OnNetworkStatusChanged(object sender)
        {
            UpdateNetworkStatus();
        }

        private class NetworkConnectivitySingleton
        {
            static NetworkConnectivitySingleton() { }

            internal static readonly NetworkConnectivity Instance = new NetworkConnectivity();
        }
    }

    public sealed class NetworkInformation
    {
        public bool IsInternetAvailable { get; private set; }

        public bool IsInternetReconnected { get; private set; }

        public bool? PreviouslyInternetAvailable { get; private set; }

        public ConnectionType ConnectionType { get; private set; }

        public ConnectionCost ConnectionCost { get; private set; }

        public NetworkCostType NetworkCostType { get { return ConnectionCost?.NetworkCostType ?? NetworkCostType.Unknown; } }

        private bool? IsUpdatedOnce;

        internal void UpdateNetworkInformation(ConnectionProfile connectionProfile)
        {
            if (connectionProfile == default)
            {
                Reset();
            }
            else
            {
                if (IsUpdatedOnce == true) { PreviouslyInternetAvailable = IsInternetAvailable; }
                ConnectionType = GetConnectionType(connectionProfile.NetworkAdapter.IanaInterfaceType);
                IsInternetAvailable = GetInternetAvailability(connectionProfile.GetNetworkConnectivityLevel());
                ConnectionCost = connectionProfile.GetConnectionCost();
                IsInternetReconnected = IsInternetAvailable && PreviouslyInternetAvailable == false;
            }

            if (IsUpdatedOnce == default)
            {
                IsUpdatedOnce = true;
            }
        }

        internal void Reset()
        {
            if (IsUpdatedOnce == true) { PreviouslyInternetAvailable = IsInternetAvailable; }
            IsInternetAvailable = false;
            IsInternetReconnected = false;
            ConnectionCost = null;
            ConnectionType = ConnectionType.Unknown;
            if (IsUpdatedOnce == default)
            {
                IsUpdatedOnce = true;
            }
        }

        private ConnectionType GetConnectionType(uint ianaInterfaceType)
        {
            switch (ianaInterfaceType)
            {
                case 6:
                    return ConnectionType.Ethernet;
                case 71:
                    return ConnectionType.Wifi;
                case 243:
                case 244:
                    return ConnectionType.Data;
                default:
                    return ConnectionType.Unknown;
            }
        }

        private bool GetInternetAvailability(NetworkConnectivityLevel connectivityLevel)
        {
            switch (connectivityLevel)
            {
                case NetworkConnectivityLevel.None:
                case NetworkConnectivityLevel.LocalAccess:
                    return false;
                default:
                    return true;
            }
        }
    }

    public enum ConnectionType
    {
        Unknown,
        Ethernet,
        Wifi,
        Data
    }
}
