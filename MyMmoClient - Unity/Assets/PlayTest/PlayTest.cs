using System;
using System.Collections.Generic;
using System.Linq;
using ExitGames.Client.Photon;
using MyMmo.Client;
using MyMmo.Client.Params;
using UnityEngine;

public class PlayTest : MonoBehaviour, IGameListener {

    private const string WorldName = "UnityWorld";

    public GameObject playerPrefab;
    
    private Game game;

    private bool isConnectState;
    private bool isEnterState;
    private bool isPlayState;
    private bool isManualSetup;
    private bool isLogGUIEnabled;
    
    private readonly List<string> logs = new List<string>();

    private void Awake() {
        Application.runInBackground = true;
        Application.targetFrameRate = 60;
    }

    private void Start() {
        isManualSetup = true;
        isLogGUIEnabled = false;
        game = new Game(this);
        game.Initialize(new PlayTestPeer(game, ConnectionProtocol.Tcp));
        game.Connect("localhost:4530");
    }

    private string serverAddress;
    private string enteredUserName;
    private Vector2 scrollPosition = Vector2.zero;

    private void OnGUI() {
        if (isConnectState) {
            GUILayout.BeginHorizontal();
            GUILayout.Label("server address:");
            serverAddress = GUILayout.TextField(serverAddress, GUILayout.MinWidth(100));
            GUILayout.EndHorizontal();
            if (GUILayout.Button("Connect") && !string.IsNullOrEmpty(serverAddress)) {
                var uri = new Uri(serverAddress);
                if (uri.Scheme.Equals("ws")) {
                    game.Initialize(new PlayTestPeer(game, ConnectionProtocol.WebSocket));
                } else if (uri.Scheme.Equals("tcp")) {
                    game.Initialize(new PlayTestPeer(game, ConnectionProtocol.Tcp));
                } else {
                    OnLog(DebugLevel.INFO, "uri.Schema is empty, init as tcp");
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
            var currentItems = game.Items;
            GUILayout.BeginVertical();
            foreach (var item in currentItems) {
                GUILayout.Label($"+id={item.Id} location={item.LocationId}");
            }
            GUILayout.EndVertical();
        }

        if (isLogGUIEnabled) {
            GUILayout.Label("----- logs ------");
            scrollPosition = GUILayout.BeginScrollView(scrollPosition);
            GUILayout.BeginVertical();
            foreach (var logEntry in logs) {
                GUILayout.Label(logEntry);
            }

            GUILayout.EndVertical();
            GUILayout.EndScrollView();
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
        if (isManualSetup) {
            enteredUserName = "ping_unity_hardcoded";
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

    public void OnItemSubscribed(Item item) {
        Debug.Log($"OnItemSubscribed {item.Id}");
        var target = FindObjectsOfType<AvatarItem>().FirstOrDefault(i => i.source.Id == item.Id);
        if (target != null) {
            target.source = item;
            return;
        }

        const int distanceOffset = 2;
        var centerOfLocation = Vector3.zero;
        var avatarsOnLocation = FindObjectsOfType<AvatarItem>().Where(avatar => avatar.source.LocationId == item.LocationId).ToArray();
        var radianStep = (1 / ((float) avatarsOnLocation.Length + 1) /*plus spawned*/) * Mathf.PI;
        for (var i = 0; i < avatarsOnLocation.Length; i++) {
            var direction = new Vector3(Mathf.Sin(radianStep * i), 0, Mathf.Cos(radianStep * i));
            avatarsOnLocation[i].MoveTo(centerOfLocation + direction * distanceOffset);
        }

        const int spawnHeight = 5;
        var spawnRadians = radianStep * avatarsOnLocation.Length + 1;
        var spawnPoint = new Vector3(Mathf.Sin(spawnRadians), spawnHeight, Mathf.Cos(spawnRadians));
        var player = Instantiate(playerPrefab, spawnPoint, Quaternion.identity);
        player.GetComponent<AvatarItem>().source = item;
    }

    public void OnItemUnsubscribed(Item item) {
        Debug.Log($"OnItemUnsubscribed {item}");
    }

    public void OnItemDestroyed(Item item) {
        Debug.Log($"OnItemDestroyed {item.Id}");
        var target = FindObjectsOfType<AvatarItem>().First(i => i.source.Id == item.Id);
        Destroy(target.gameObject); 
    }

    public void OnItemLocationChanged(Item item) {
        Debug.Log($"OnItemLocationChanged {item.Id}");
    }

    public void OnLog(DebugLevel debugLevel, string message) {
        logs.Add($"{debugLevel}: {message}");
        Debug.Log($"{debugLevel}: {message}");
    }
    
}