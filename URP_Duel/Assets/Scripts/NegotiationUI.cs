using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NegotiationUI : MonoBehaviour
{
    public static NegotiationUI Instance { get; private set; }

    public event EventHandler OnWrongAnswer;
    public event EventHandler OnRightAnswer;

    private void Awake()
    {
        Instance = this;

        transform.Find("moneyButton").GetComponent<Button>().onClick.AddListener(() =>
        {
            OnWrongAnswer?.Invoke(this, EventArgs.Empty);
        });
        transform.Find("loveButton").GetComponent<Button>().onClick.AddListener(() =>
        {
            OnRightAnswer?.Invoke(this, EventArgs.Empty);
        });
        transform.Find("lifeButton").GetComponent<Button>().onClick.AddListener(() =>
        {
            OnRightAnswer?.Invoke(this, EventArgs.Empty);
        });
        transform.Find("deathButton").GetComponent<Button>().onClick.AddListener(() =>
        {
            OnWrongAnswer?.Invoke(this, EventArgs.Empty);
        });

        gameObject.SetActive(false);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
