using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class playerBase : NetworkBehaviour
{

    [Scene]
    public string[] subScene;

    // Start is called before the first frame update
    void Start()
    {
        if (!isLocalPlayer && !isServer)
        {
            return;
        }

        
        //playerCanvas.SetActive(true);        
    }

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
        // do logic to load main scene
        CmdLoadMainMenu(netIdentity);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    [Command]
    public void CmdLoadMainMenu(NetworkIdentity networkIdentity)
    {
        LoadMainScene(networkIdentity);
    }

    [Server]
    void LoadMainScene(NetworkIdentity networkIdentity)
    {
        NetworkServer.SendToClientOfPlayer(networkIdentity, new SceneMessage { sceneName = subScene[0], sceneOperation = SceneOperation.LoadAdditive });
    }

    [Command]
    public void CmdLoadMainMenuAfterMatch(NetworkIdentity networkIdentity)
    {
        LoadMainSceneAfterMatch(networkIdentity);
    }

    [Server]
    void LoadMainSceneAfterMatch(NetworkIdentity networkIdentity)
    {
        NetworkServer.SendToClientOfPlayer(networkIdentity, new SceneMessage { sceneName = subScene[1], sceneOperation = SceneOperation.UnloadAdditive });
        NetworkServer.SendToClientOfPlayer(networkIdentity, new SceneMessage { sceneName = subScene[0], sceneOperation = SceneOperation.LoadAdditive });
    }

    [Command]
    public void CmdFindMatch(NetworkIdentity networkIdentity)
    {
        LoadMatchScene(networkIdentity);
    }

    [Server]
    void LoadMatchScene(NetworkIdentity networkIdentity)
    {
        NetworkServer.SendToClientOfPlayer(networkIdentity, new SceneMessage { sceneName = subScene[0], sceneOperation = SceneOperation.UnloadAdditive });
        NetworkServer.SendToClientOfPlayer(networkIdentity, new SceneMessage { sceneName = subScene[1], sceneOperation = SceneOperation.LoadAdditive });
    }

}
