using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameManager Instance { get; private set; }

    private enum State
    {
        Storyboard,
        Game
    }
    private State state;

    private int activeSceneBuildIndex;

    private void Awake()
    {
        state = State.Storyboard;
        activeSceneBuildIndex = SceneManager.GetActiveScene().buildIndex;
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        MusicManager.Instance.PlayMusic(MusicManager.Music.storyboardMusic, true);

        switch (activeSceneBuildIndex)
        {
            case 1:
                StoryboardManager.Instance.ShowStoryboard(StoryboardManager.Storyboard.negationStoryboard);
                break;
            case 2:
                StoryboardManager.Instance.ShowStoryboard(StoryboardManager.Storyboard.angerStoryboard);
                break;
            case 3:
                StoryboardManager.Instance.ShowStoryboard(StoryboardManager.Storyboard.negotiationStoryboard);
                break;
            case 4:
                StoryboardManager.Instance.ShowStoryboard(StoryboardManager.Storyboard.depressionStoryboard);
                break;
            case 5:
                StoryboardManager.Instance.ShowStoryboard(StoryboardManager.Storyboard.acceptanceStoryboard);
                break;
        }
    }

    private void Update()
    {
        if (state == State.Storyboard)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                state = State.Game;
                StoryboardManager.Instance.HideStoryboard();

                switch (activeSceneBuildIndex)
                {
                    case 1:
                        MusicManager.Instance.PlayMusic(MusicManager.Music.negationMusic, true);
                        break;
                    case 2:
                        MusicManager.Instance.PlayMusic(MusicManager.Music.angerMusic, true);
                        break;
                    case 3:
                        GameSceneManager.Load(GameSceneManager.Scene.DepressionScene);
                        break;
                    case 4:
                        MusicManager.Instance.PlayMusic(MusicManager.Music.depressionMusic, true);
                        break;
                    case 5:
                        MusicManager.Instance.PlayMusic(MusicManager.Music.acceptanceMusic, true);
                        CreditsUI.Instance.Show();
                        break;
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            OptionsUI.Instance.ToggleVisible();

            // DEBUG
            //DebugUI.Instance.ToggleVisible();
        }
    }
}