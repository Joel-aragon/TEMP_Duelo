using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class GameSceneManager
{
    public enum Scene
    {
        StaticScene,
        MainMenuScene,
        NegationScene,
        AngerScene,
        NegotiationScene,
        DepressionScene,
        AcceptanceScene
    }

    public static void Load(Scene scene)
    {
        if(scene == Scene.MainMenuScene)
        {
            SceneManager.LoadScene(scene.ToString());
        }
        else
        {
            SceneManager.LoadScene(scene.ToString());
            SceneManager.LoadScene(Scene.StaticScene.ToString(), LoadSceneMode.Additive);
        }

    }
}
