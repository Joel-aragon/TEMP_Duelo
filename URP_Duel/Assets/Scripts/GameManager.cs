using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    /* CHANGES:
     * Toogle OptionsUI
     */

    public GameManager Instance { get; private set; }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OptionsUI.Instance.ToggleVisible();
            DebugUI.Instance.ToggleVisible();
        }
    }
}
