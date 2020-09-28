using System.Collections.Generic;
using UnityEngine;

public class StoryboardManager : MonoBehaviour
{
    public static StoryboardManager Instance { get; private set; }

    public enum Storyboard
    {
        negationStoryboard,
        angerStoryboard,
        negotiationStoryboard,
        depressionStoryboard,
        acceptanceStoryboard
    }

    private Dictionary<Storyboard, Sprite> storyboardSpriteDictionary;

    private void Awake()
    {
        Instance = this;

        storyboardSpriteDictionary = new Dictionary<Storyboard, Sprite>();
        foreach (Storyboard storyboard in System.Enum.GetValues(typeof(Storyboard)))
        {
            storyboardSpriteDictionary[storyboard] = Resources.Load<Sprite>(storyboard.ToString());
        }
    }

    public void ShowStoryboard(Storyboard storyboard)
    {
        StoryboardUI.Instance.Show(storyboardSpriteDictionary[storyboard]);
    }

    public void HideStoryboard()
    {
        StoryboardUI.Instance.Hide();
    }
}