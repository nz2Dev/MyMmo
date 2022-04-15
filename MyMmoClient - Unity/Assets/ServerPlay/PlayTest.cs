using System;
using ExitGames.Client.Photon;
using MyMmo.Client;
using MyMmo.Client.Params;
using MyMmo.Commons.Scripts;
using MyMmo.Commons.Snapshots;
using Player;
using UnityEngine;

namespace ServerPlay {
    public class PlayTest : MonoBehaviour, IGameListener {

        private const string WorldName = "UnityWorld";

        public UnityWorldPlayer worldPlayer;

        private bool isConnectState;
        private bool isEnterState;
        private bool isPlayState;
        private bool isManualSetup;
        private Game game;

        private void Awake() {
            Application.runInBackground = true;
            Application.targetFrameRate = 60;
            DontDestroyOnLoad(gameObject);
        }

        private void Start() {
            game = new Game(this);
            isConnectState = true;

#if UNITY_EDITOR
            isManualSetup = true;
            if (isManualSetup) {
                enteredUserName = "unity_editor_manual";
                game.Initialize(new UnityPeer(game, ConnectionProtocol.Tcp));
                game.Connect("localhost:4530");
            }
#elif UNITY_WEBGL
        isManualSetup = false;
        if (isManualSetup) {
            enteredUserName = "unity_webgl_manual";
            game.Initialize(new PlayTestPeer(game, ConnectionProtocol.WebSocketSecure));
            game.Connect("wss://a2cd-62-122-202-155.ngrok.io:443");
        }
#else
        isManualSetup = false;
#endif
        }

        private string serverAddress;
        private string enteredUserName;

        private void OnGUI() {
            if (isConnectState) {
                GUILayout.BeginHorizontal();
                GUILayout.Label("server address:");
                serverAddress = GUILayout.TextField(serverAddress, GUILayout.MinWidth(100));
                GUILayout.EndHorizontal();
                if (GUILayout.Button("Connect") && !string.IsNullOrEmpty(serverAddress)) {
                    if (serverAddress.Contains("ngrok.io") && !serverAddress.Contains("://")) {
                        serverAddress = $"wss://{serverAddress}:443";
                    }
                
                    var uri = new Uri(serverAddress);
                    if (uri.Scheme.Equals("ws")) {
                        game.Initialize(new UnityPeer(game, ConnectionProtocol.WebSocket));
                    } else if (uri.Scheme.Equals("wss")) {
                        game.Initialize(new UnityPeer(game, ConnectionProtocol.WebSocketSecure));
                    } else if (uri.Scheme.Equals("tcp")) {
                        game.Initialize(new UnityPeer(game, ConnectionProtocol.Tcp));
                    } else {
                        OnLog(DebugLevel.WARNING, "uri.Schema is empty, init as tcp");
                        game.Initialize(new UnityPeer(game, ConnectionProtocol.Tcp));
                    }
                
                    game.Connect(serverAddress);
                }
            } else if (isEnterState) {
                GUILayout.BeginHorizontal();
                GUILayout.Label("UserName:");
                enteredUserName = GUILayout.TextField(enteredUserName, GUILayout.MinWidth(100));
                GUILayout.EndHorizontal();
                if (GUILayout.Button("Enter") && !string.IsNullOrEmpty(enteredUserName)) {
                    CreateWorld();
                }
            } else if (isPlayState) {
                var currentItems = FindObjectsOfType<AvatarItem>();
                GUILayout.BeginVertical();
                foreach (var item in currentItems) {
                    GUILayout.Label($"+id={item.State.ItemId} location={item.LocationId}");
                }
                GUILayout.EndVertical();
            }
        }

        private void FixedUpdate() {
            game.Update();
        }

        [ContextMenu("CreateWorld")]
        public void CreateWorld() {
            game.CreateWorld(new CreateWorldParams {WorldName = WorldName});
        }

        [ContextMenu("EnterWorld")]
        public void EnterWorld() {
            game.EnterWorld(new EnterWorldParams {UserName = enteredUserName, WorldName = WorldName});
        }

        public void OnConnected() {
            Debug.Log("OnConnected");
            isConnectState = false;
            isEnterState = true;
            if (isManualSetup && !string.IsNullOrEmpty(enteredUserName)) {
                CreateWorld();
            }
        }

        public void OnWorldCreated() {
            EnterWorld();
        }

        public void OnWorldEntered() {
            Debug.Log("OnWorldEntered");
            isEnterState = false;
            isPlayState = true;
        }

        public void OnLocationEntered(int locationId, SceneSnapshotData sceneSnapshotData) {
            worldPlayer.EnterLocation(locationId, sceneSnapshotData);
        }

        public void OnLocationExit(int locationId) {
            worldPlayer.ExitLocation(locationId); 
        }

        public void OnLocationUpdate(int locationId, ScriptsClipData clipData) {
            worldPlayer.UpdateLocation(locationId, clipData);
        }

        public void OnLog(DebugLevel debugLevel, string message) {
            if (debugLevel <= DebugLevel.WARNING) {
                Debug.Log($"GameListenerLog {debugLevel}: {message}");
            }
        }

    }
}