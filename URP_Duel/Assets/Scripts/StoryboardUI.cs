using System;
using UnityEngine;
using UnityEngine.UI;

public class StoryboardUI : MonoBehaviour
{
    public static StoryboardUI Instance { get; private set; }

    public EventHandler OnInactiveStoryboardUI;

    private Image storyboardImage;

    private void Awake()
    {
        Instance = this;

        storyboardImage = transform.Find("storyboardImage").GetComponent<Image>();

        gameObject.SetActive(false);
    }

    public void Show(Sprite sprite)
    {
        gameObject.SetActive(true);
        storyboardImage.sprite = sprite;
    }

    public void Hide()
    {
        OnInactiveStoryboardUI?.Invoke(this, EventArgs.Empty);

        gameObject.SetActive(false);
    }
}