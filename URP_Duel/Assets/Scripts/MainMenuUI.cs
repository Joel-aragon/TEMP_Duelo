using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    public static MainMenuUI Instance;

    private void Awake()
    {
        Instance = this;

        transform.Find("playButton").GetComponent<Button>().onClick.AddListener(() =>
        {
            GameSceneManager.Load(GameSceneManager.Scene.NegationScene);
        });
        transform.Find("creditsButton").GetComponent<Button>().onClick.AddListener(() =>
        {
            MainMenuCreditsUI.Instance.Show();
        });
        transform.Find("quitButton").GetComponent<Button>().onClick.AddListener(() =>
        {
            Application.Quit();
        });
    }

    public void Show()
    {
        MainMenuCreditsUI.Instance.Hide();
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}