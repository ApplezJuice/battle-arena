using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneButtons : MonoBehaviour
{
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
        SceneLoader.Load(SceneLoader.Scene.Menu);
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
}
