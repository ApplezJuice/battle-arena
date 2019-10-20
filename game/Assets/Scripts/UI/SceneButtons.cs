using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneButtons : MonoBehaviour
{


    public GameObject chatBox;

    public delegate void OpenChatBoxDelegate();
    public static event OpenChatBoxDelegate OnOpenChatBoxDelegate;

    public void LoadMatchScene()
    {
        SceneLoader.Load(SceneLoader.Scene.match);
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game is exiting");
    }

    public void LoadMainMenu()
    {
        // menu between games, after login
        SceneLoader.Load(SceneLoader.Scene.MainLobby);
    }

    public void LoadMainLoginMenu()
    {
        SceneLoader.Load(SceneLoader.Scene.MainLoginMenu);
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
        //OnOpenChatBoxDelegate.Invoke();
    }

    public void HideChat()
    {
        chatBox.SetActive(false);
    }
}
