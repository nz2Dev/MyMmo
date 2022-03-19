using System;
using System.Linq;
using MyMmo.Client;
using MyMmo.Commons.Scripts;
using UnityEngine;
using Object = UnityEngine.Object;

public class ChangeLocationScriptExecutor {

    private readonly ChangeLocationScript script;
    private readonly Game game;

    private AvatarItem targetAvatar;
    private Location targetLocation;

    public ChangeLocationScriptExecutor(ChangeLocationScript script, Game game) {
        this.script = script;
        this.game = game;
        targetAvatar = Object.FindObjectsOfType<AvatarItem>().FirstOrDefault(avatar => avatar.source.Id == script.ItemId);
        if (targetAvatar == null) {
            throw new Exception("can't find script target item with id: " + script.ItemId);
        }

        targetLocation = Object.FindObjectsOfType<Location>().FirstOrDefault(location => location.Id == script.ToLocation);
        if (targetLocation == null) {
            throw new Exception("can't find script's target location with id: " + script.ToLocation);
        }
    }

    public bool IsRunning { get; private set; }

    public void Update() {
        targetAvatar.transform.position =
            Vector3.Lerp(targetAvatar.transform.position, targetLocation.transform.position, Time.deltaTime * 5);
        var arrived = (targetLocation.transform.position - targetAvatar.transform.position).magnitude < 0.5f;
        if (arrived) {
            targetAvatar.transform.position = targetLocation.transform.position;
            
            game.ApplyPerformedScript(script);
        }
        IsRunning = !arrived;
    }
    
}