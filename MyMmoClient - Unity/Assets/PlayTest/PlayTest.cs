using System;
using System.Linq;
using ExitGames.Client.Photon;
using MyMmo.Client;
using MyMmo.Client.Params;
using MyMmo.Commons.Scripts;
using MyMmo.Commons.Snapshots;
using UnityEngine;

public class PlayTest : MonoBehaviour, IGameListener {

    public static PlayTest Instance;
    private const string WorldName = "UnityWorld";

    public GameObject playerPrefab;
    
    private Game game;
    private UnityScriptsClipPlayer scriptsPlayer;

    private bool isConnectState;
    private bool isEnterState;
    private bool isPlayState;
    private bool isManualSetup;
    
    private void Awake() {
        Instance = this;
        Application.runInBackground = true;
        Application.targetFrameRate = 60;
        DontDestroyOnLoad(gameObject);
        scriptsPlayer = GetComponent<UnityScriptsClipPlayer>();
    }

    private void Start() {
        game = new Game(this);
        isConnectState = true;

#if UNITY_EDITOR
        isManualSetup = true;
        if (isManualSetup) {
            enteredUserName = "unity_editor_manual";
            game.Initialize(new PlayTestPeer(game, ConnectionProtocol.Tcp));
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
                    game.Initialize(new PlayTestPeer(game, ConnectionProtocol.WebSocket));
                } else if (uri.Scheme.Equals("wss")) {
                    game.Initialize(new PlayTestPeer(game, ConnectionProtocol.WebSocketSecure));
                } else if (uri.Scheme.Equals("tcp")) {
                    game.Initialize(new PlayTestPeer(game, ConnectionProtocol.Tcp));
                } else {
                    OnLog(DebugLevel.WARNING, "uri.Schema is empty, init as tcp");
                    game.Initialize(new PlayTestPeer(game, ConnectionProtocol.Tcp));
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
                GUILayout.Label($"+id={item.State.ItemId} location={item.State.LocationId}");
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

    public void OnLocationEntered(LocationSnapshotData locationSnapshotData) {
        var targetLocation = FindObjectsOfType<Location>().FirstOrDefault(location => location.Id == locationSnapshotData.LocationId);
        if (targetLocation == null) {
            throw new Exception("Location not found: " + locationSnapshotData.LocationId);
        }
        
        Debug.Log($"location {locationSnapshotData.LocationId} enters, with items snapshots [{locationSnapshotData.ItemsSnapshotData.AggregateToString()}]");
        foreach (var itemSnapshotData in locationSnapshotData.ItemsSnapshotData) {
            targetLocation.ReplaceAvatar(playerPrefab, itemSnapshotData);
        }
    }

    public void OnLocationExit(int locationId) {
        // for disposing unused resources, will have to implement some mechanism for that 
    }

    public void OnLocationUpdate(int locationId, ScriptsClipData clipData) {
        Debug.Log($"on location update: {locationId} with scripts[{clipData.ScriptsData.Length}] [{clipData.ScriptsData.Select(data => data.ItemScriptData).AggregateToString()}]");
        scriptsPlayer.PlayClip(locationId, clipData);
    }

    public void OnLog(DebugLevel debugLevel, string message) {
        Debug.Log($"GameListenerLog {debugLevel}: {message}");
    }

}