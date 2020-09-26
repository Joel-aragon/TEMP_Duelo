using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcceptanceUI : MonoBehaviour
{
    public static AcceptanceUI Instance { get; private set; }

    private void Awake()
    {
        Instance = this;

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
