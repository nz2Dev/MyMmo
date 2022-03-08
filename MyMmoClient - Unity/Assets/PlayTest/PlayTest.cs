using ExitGames.Client.Photon;
using MyMmo.Client;
using MyMmo.Client.Params;
using UnityEngine;

public class PlayTest : MonoBehaviour, IGameListener {

    private const string WorldName = "UnityWorld";

    private Game game;

    private bool isInTheWorld;
    
    private void Awake() {
        Application.runInBackground = true;
    }

    private void Start() {
        game = new Game(this);
        var peer = new PhotonPeer(game, ConnectionProtocol.Tcp);
        game.Initialize(peer);
        game.ConnectDefault();
    }

    private string enteredUserName;

    private void OnGUI() {
        if (!isInTheWorld) {
            GUILayout.BeginHorizontal();
            GUILayout.Label("UserName:");
            enteredUserName = GUILayout.TextField(enteredUserName, GUILayout.MinWidth(100));
            GUILayout.EndHorizontal();
            if (GUILayout.Button("Enter") && !string.IsNullOrEmpty(enteredUserName)) {
                CreateWorld();
            }
        } else {
            var currentItems = game.Items;
            GUILayout.BeginVertical();
            foreach (var item in currentItems) {
                GUILayout.Label($"+id={item.Id} location={item.LocationId}");
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
    }

    public void OnWorldCreated() {
        EnterWorld();
    }

    public void OnWorldEntered() {
        Debug.Log("OnWorldEntered");
        isInTheWorld = true;
    }

    public void OnItemEnter(Item item) {
        Debug.Log($"OnItemEnter {item.Id}");
    }

    public void OnItemExit(string itemId) {
        Debug.Log($"OnItemExit {itemId}");
    }

    public void OnItemLocationChanged(Item item) {
        Debug.Log($"OnItemLocationChanged {item.Id}");
    }

}