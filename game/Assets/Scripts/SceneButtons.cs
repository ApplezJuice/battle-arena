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
        SceneLoader.Load(SceneLoader.Scene.Menu);
    }
}
