using UnityEngine;
using UnityEngine.UI;

public class MainMenuCreditsUI : MonoBehaviour
{
    public static MainMenuCreditsUI Instance;

    private void Awake()
    {
        Instance = this;

        transform.Find("mainMenuButton").GetComponent<Button>().onClick.AddListener(() =>
        {
            MainMenuUI.Instance.Show();
        });

        Hide();
    }

    public void Show()
    {
        MainMenuUI.Instance.Hide();
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}