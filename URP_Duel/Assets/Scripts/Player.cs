using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private HealthSystem healthSystem;
    private PlayerController playerController;

    private void Awake()
    {
        healthSystem = GetComponent<HealthSystem>();
        playerController = GetComponent<PlayerController>();

        healthSystem.OnDied += HealthSystem_OnDied;
    }

    private void HealthSystem_OnDied(object sender, System.EventArgs e)
    {
        playerController.SetState(PlayerController.State.Dead);
        GameOverUI.Instance.Show();
    }
}
