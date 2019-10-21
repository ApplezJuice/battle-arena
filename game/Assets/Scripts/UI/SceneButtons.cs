using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class SceneButtons : NetworkBehaviour
{


    public GameObject chatBox;

    public delegate void OpenChatBoxDelegate();
    public static event OpenChatBoxDelegate OnOpenChatBoxDelegate;

    [Scene]
    [Tooltip("Assign the sub-scene to load for this zone")]
    public string subScene;

    public void LoadMatchScene()
    {
        NetworkClient.connection.identity.GetComponent<playerBase>().CmdFindMatch(NetworkClient.connection.identity);
        //SceneLoader.Load(SceneLoader.Scene.match);
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game is exiting");
    }

    public void LoadMainMenu()
    {
        // menu between games, after login
        //SceneLoader.Load(SceneLoader.Scene.MainLobby);
        NetworkClient.connection.identity.GetComponent<playerBase>().CmdLoadMainMenuAfterMatch(NetworkClient.connection.identity);
    }

    public void LoadMainLoginMenu()
    {
        //SceneLoader.Load(SceneLoader.Scene.MainLoginMenu);
        NetworkClient.connection.identity.GetComponent<playerBase>().CmdLoadMainMenuAfterMatch(NetworkClient.connection.identity);
    }

    public void LoadLoginScreen()
    {
        SceneLoader.Load(SceneLoader.Scene.LoginScreen);
    }

    public void LoadRegisterScreen()
    {
        SceneLoader.Load(SceneLoader.Scene.RegisterMenu);
    }

    public void LoadDiscord()
    {
        Application.OpenURL("https://discord.gg/aAWZgAa");
    }

    public void OpenChat()
    {
        chatBox.SetActive(true);
        NetworkClient.connection.identity.GetComponent<ChatManager>().InitializeLobbyCanvas();
        //OnOpenChatBoxDelegate.Invoke();
    }

    public void HideChat()
    {
        chatBox.SetActive(false);
    }
}
