#if UNITY_4_7 || UNITY_5 || UNITY_5_3_OR_NEWER
#define SUPPORTED_UNITY
#endif

using System;
using ExitGames.Client.Photon;

public class UnityPeer : PhotonPeer {

    public UnityPeer(ConnectionProtocol protocolType) : base(protocolType) {
        ConfigUnitySockets();
    }

    public UnityPeer(IPhotonPeerListener listener, ConnectionProtocol protocolType) : this(protocolType) {
        Listener = listener;
    }

    // Sets up the socket implementations to use, depending on platform
    [System.Diagnostics.Conditional("SUPPORTED_UNITY")]
    private void ConfigUnitySockets() {
        Type websocketType = null;
#if (UNITY_XBOXONE || UNITY_GAMECORE) && !UNITY_EDITOR
        websocketType = Type.GetType("ExitGames.Client.Photon.SocketNativeSource, Assembly-CSharp", false);
        if (websocketType == null) {
            websocketType =
                Type.GetType("ExitGames.Client.Photon.SocketNativeSource, Assembly-CSharp-firstpass", false);
        }

        if (websocketType == null) {
            websocketType = Type.GetType("ExitGames.Client.Photon.SocketNativeSource, PhotonRealtime", false);
        }

        if (websocketType != null) {
            this.SocketImplementationConfig[ConnectionProtocol.Udp] =
                websocketType; // on Xbox, the native socket plugin supports UDP as well
        }
#else
        // to support WebGL export in Unity, we find and assign the SocketWebTcp class (if it's in the project).
        // alternatively class SocketWebTcp might be in the Photon3Unity3D.dll
        websocketType = Type.GetType("ExitGames.Client.Photon.SocketWebTcp, PhotonWebSocket", false);
        if (websocketType == null) {
            websocketType = Type.GetType("ExitGames.Client.Photon.SocketWebTcp, Assembly-CSharp-firstpass", false);
        }

        if (websocketType == null) {
            websocketType = Type.GetType("ExitGames.Client.Photon.SocketWebTcp, Assembly-CSharp", false);
        }
#endif

        if (websocketType != null) {
            this.SocketImplementationConfig[ConnectionProtocol.WebSocket] = websocketType;
            this.SocketImplementationConfig[ConnectionProtocol.WebSocketSecure] = websocketType;
        }

#if NET_4_6 && (UNITY_EDITOR || !ENABLE_IL2CPP) && !NETFX_CORE
        this.SocketImplementationConfig[ConnectionProtocol.Udp] = typeof(SocketUdpAsync);
        this.SocketImplementationConfig[ConnectionProtocol.Tcp] = typeof(SocketTcpAsync);
#endif
    }

}