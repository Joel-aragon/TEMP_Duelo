using UnityEngine;
using UnityEngine.UI;

public class CreditsUI : MonoBehaviour
{
    public static CreditsUI Instance;

    private void Awake()
    {
        Instance = this;

        transform.Find("mainMenuButton").GetComponent<Button>().onClick.AddListener(() =>
        {
            GameSceneManager.Load(GameSceneManager.Scene.MainMenuScene);
        });

        Hide();
    }

    public void Show()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}