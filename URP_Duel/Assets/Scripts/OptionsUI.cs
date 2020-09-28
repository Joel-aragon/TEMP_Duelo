using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System;

public class OptionsUI : MonoBehaviour
{
    public static OptionsUI Instance { get; private set; }

    public EventHandler OnActiveOptionsUI;
    public EventHandler OnInactiveOptionsUI;

    private TextMeshProUGUI soundVolumeText;
    private TextMeshProUGUI musicVolumeText;

    private void Awake()
    {
        Instance = this;

        soundVolumeText = transform.Find("soundVolumeText").GetComponent<TextMeshProUGUI>();
        musicVolumeText = transform.Find("musicVolumeText").GetComponent<TextMeshProUGUI>();

        transform.Find("soundIncreaseButton").GetComponent<Button>().GetComponent<Button>().onClick.AddListener(() =>
        {
            SoundManager.Instance.IncreaseVolume();
            UpdateSoundVolumeText();
        });
        transform.Find("soundDecreaseButton").GetComponent<Button>().GetComponent<Button>().onClick.AddListener(() =>
        {
            SoundManager.Instance.DecreaseVolume();
            UpdateSoundVolumeText();
        });
        transform.Find("musicIncreaseButton").GetComponent<Button>().GetComponent<Button>().onClick.AddListener(() =>
        {
            MusicManager.Instance.IncreaseVolume();
            UpdateMusicVolumeText();
        });
        transform.Find("musicDecreaseButton").GetComponent<Button>().GetComponent<Button>().onClick.AddListener(() =>
        {
            MusicManager.Instance.DecreaseVolume();
            UpdateMusicVolumeText();
        });
        transform.Find("replayButton").GetComponent<Button>().GetComponent<Button>().onClick.AddListener(() =>
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            SceneManager.LoadScene("StaticScene", LoadSceneMode.Additive);
        });
        transform.Find("mainMenuButton").GetComponent<Button>().GetComponent<Button>().onClick.AddListener(() =>
        {
            Time.timeScale = 1f;
            GameSceneManager.Load(GameSceneManager.Scene.MainMenuScene);
        });
        transform.Find("exitButton").GetComponent<Button>().GetComponent<Button>().onClick.AddListener(() =>
        {
            Application.Quit();
        });
    }

    private void Start()
    {
        UpdateSoundVolumeText();
        UpdateMusicVolumeText();
        gameObject.SetActive(false);
    }

    private void UpdateSoundVolumeText()
    {
        soundVolumeText.SetText(Mathf.RoundToInt(SoundManager.Instance.GetVolume() * 10).ToString());
    }

    private void UpdateMusicVolumeText()
    {
        musicVolumeText.SetText(Mathf.RoundToInt(MusicManager.Instance.GetVolume() * 10).ToString());
    }

    public void ToggleVisible()
    {
        gameObject.SetActive(!gameObject.activeSelf);

        if (gameObject.activeSelf)
        {
            OnActiveOptionsUI?.Invoke(this, EventArgs.Empty);
            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            OnInactiveOptionsUI?.Invoke(this, EventArgs.Empty);
            Time.timeScale = 1f;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}