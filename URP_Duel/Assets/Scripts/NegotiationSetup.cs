using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NegotiationSetup : MonoBehaviour
{
    private HealthSystem healthSystem;

    private void Awake()
    {
        healthSystem = GetComponent<HealthSystem>();
    }

    private void Start()
    {
        healthSystem.OnDied += HealthSystem_OnDied;

        NegotiationUI.Instance.OnRightAnswer += NegotiationUI_OnRightAnswer;
        NegotiationUI.Instance.OnWrongAnswer += Instance_OnWrongAnswer;

        NegotiationUI.Instance.Show();
    }

    private void HealthSystem_OnDied(object sender, System.EventArgs e)
    {
        NegotiationUI.Instance.Hide();
        GameOverUI.Instance.Show();
    }

    private void Instance_OnWrongAnswer(object sender, System.EventArgs e)
    {
        healthSystem.Damage(25);
    }

    private void NegotiationUI_OnRightAnswer(object sender, System.EventArgs e)
    {
        GameSceneManager.Load(GameSceneManager.Scene.DepressionScene);
    }
}
