﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    private void Awake()
    {
        transform.Find("playButton").GetComponent<Button>().onClick.AddListener(() =>
        {
            GameSceneManager.Load(GameSceneManager.Scene.NegationScene);
            SceneManager.LoadScene("StaticScene", LoadSceneMode.Additive);
        });
        transform.Find("quitButton").GetComponent<Button>().onClick.AddListener(() =>
        {
            Application.Quit();
        });
    }
}
