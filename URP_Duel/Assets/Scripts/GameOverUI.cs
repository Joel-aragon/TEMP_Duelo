using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    public static GameOverUI Instance { get; private set; }

    private TextMeshProUGUI infoText;

    private void Awake()
    {
        Instance = this;

        transform.Find("retryButton").GetComponent<Button>().onClick.AddListener(() =>
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            SceneManager.LoadScene("StaticScene", LoadSceneMode.Additive);
        });
        transform.Find("mainMenuButton").GetComponent<Button>().onClick.AddListener(() =>
        {
            GameSceneManager.Load(GameSceneManager.Scene.MainMenuScene);
        });

        infoText = transform.Find("infoText").GetComponent<TextMeshProUGUI>();

        Hide();
    }

    public void Show()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        gameObject.SetActive(true);

        int activeSceneBuildIndex = SceneManager.GetActiveScene().buildIndex;

        string gameOverText = "";
        switch (activeSceneBuildIndex)
        {
            case 1:
                gameOverText = "Dani siempre negó la realidad.";
                break;
            case 2:
                gameOverText = "Dani se pasó la vida enfadado.";
                break;
            case 4:
                gameOverText = "Dani tuvo una depresión de caballo.";
                break;
        }

        infoText.SetText(gameOverText);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}