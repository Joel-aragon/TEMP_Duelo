using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class OptionsUI : MonoBehaviour
{
    /* CHANGES:
     * Made Singleton
     */

    public static OptionsUI Instance { get; private set; }

    [SerializeField] private SoundManager soundManager;
    [SerializeField] private MusicManager musicManager;

    private TextMeshProUGUI soundVolumeText;
    private TextMeshProUGUI musicVolumeText;

    private void Awake()
    {
        Instance = this;

        soundVolumeText = transform.Find("soundVolumeText").GetComponent<TextMeshProUGUI>();
        musicVolumeText = transform.Find("musicVolumeText").GetComponent<TextMeshProUGUI>();

        transform.Find("soundIncreaseButton").GetComponent<Button>().GetComponent<Button>().onClick.AddListener(() =>
        {
            soundManager.IncreaseVolume();
            UpdateSoundVolumeText();
        });
        transform.Find("soundDecreaseButton").GetComponent<Button>().GetComponent<Button>().onClick.AddListener(() =>
        {
            soundManager.DecreaseVolume();
            UpdateSoundVolumeText();
        });
        transform.Find("musicIncreaseButton").GetComponent<Button>().GetComponent<Button>().onClick.AddListener(() =>
        {
            musicManager.IncreaseVolume();
            UpdateMusicVolumeText();
        });
        transform.Find("musicDecreaseButton").GetComponent<Button>().GetComponent<Button>().onClick.AddListener(() =>
        {
            musicManager.DecreaseVolume();
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
        gameObject.SetActive(false);
    }

    private void UpdateSoundVolumeText()
    {
        soundVolumeText.SetText(Mathf.RoundToInt(soundManager.GetVolume() * 10).ToString());
    }

    private void UpdateMusicVolumeText()
    {
        musicVolumeText.SetText(Mathf.RoundToInt(musicManager.GetVolume() * 10).ToString());
    }

    public void ToggleVisible()
    {
        gameObject.SetActive(!gameObject.activeSelf);

        if (gameObject.activeSelf)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
        }
    }
}
