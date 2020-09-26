using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugUI : MonoBehaviour
{
    public static DebugUI Instance { get; private set; }

    private void Awake()
    {
        Instance = this;

        transform.Find("negationSceneButton").GetComponent<Button>().onClick.AddListener(() =>
        {
            Time.timeScale = 1f;
            GameSceneManager.Load(GameSceneManager.Scene.NegationScene);
        });
        transform.Find("angerSceneButton").GetComponent<Button>().onClick.AddListener(() =>
        {
            Time.timeScale = 1f;
            GameSceneManager.Load(GameSceneManager.Scene.AngerScene);
        });
        transform.Find("negotiationSceneButton").GetComponent<Button>().onClick.AddListener(() =>
        {
            Time.timeScale = 1f;
            GameSceneManager.Load(GameSceneManager.Scene.NegotiationScene);
        });
        transform.Find("depressionSceneButton").GetComponent<Button>().onClick.AddListener(() =>
        {
            Time.timeScale = 1f;
            GameSceneManager.Load(GameSceneManager.Scene.DepressionScene);
        });
        transform.Find("acceptanceSceneButton").GetComponent<Button>().onClick.AddListener(() =>
        {
            Time.timeScale = 1f;
            GameSceneManager.Load(GameSceneManager.Scene.AcceptanceScene);
        });
    }

    private void Start()
    {
        gameObject.SetActive(false);
    }

    public void ToggleVisible()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }
}
