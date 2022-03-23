using System;
using System.Collections.Generic;
using System.Linq;
using ExitGames.Client.Photon;
using MyMmo.Client;
using MyMmo.Client.Params;
using MyMmo.Commons.Scripts;
using MyMmo.Commons.Snapshots;
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

    public void OnLocationEntered(LocationSnapshotData locationSnapshotData) {
        var targetLocation = FindObjectsOfType<Location>().FirstOrDefault(location => location.Id == locationSnapshotData.LocationId);
        if (targetLocation == null) {
            throw new Exception("Location not found: " + locationSnapshotData.LocationId);
        }
        foreach (var itemSnapshotData in locationSnapshotData.ItemsSnapshotData) {
            targetLocation.ReplaceAvatar(playerPrefab, itemSnapshotData);
        }
    }

    public void OnLocationExit(int locationId) {
        // for disposing unused resources, will have to implement some mechanism for that 
    }

    public void OnLog(DebugLevel debugLevel, string message) {
        logs.Add($"{debugLevel}: {message}");
        Debug.Log($"{debugLevel}: {message}");
    }

    public void OnRegionUpdate(int locationId, BaseScriptData[] scripts) {
        var targetLocation = FindObjectsOfType<Location>().FirstOrDefault(location => location.Id == locationId);
        if (targetLocation == null) {
            Debug.LogError("location: " + locationId + " not found for updateScript execution");
            return;
        }
        
        targetLocation.ExecuteScripts(scripts);
    }

}