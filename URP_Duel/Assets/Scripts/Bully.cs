using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bully : MonoBehaviour
{
    private HealthSystem healthSystem;

    private void Awake()
    {
        healthSystem = GetComponent<HealthSystem>();

        healthSystem.OnDied += HealthSystem_OnDied;
    }

    private void HealthSystem_OnDied(object sender, System.EventArgs e)
    {
        GameSceneManager.Load(GameSceneManager.Scene.NegotiationScene);
    }
}
