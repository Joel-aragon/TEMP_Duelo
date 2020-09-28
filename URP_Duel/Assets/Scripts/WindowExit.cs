using UnityEngine;

public class WindowExit : MonoBehaviour
{
    private void OnTriggerExit(Collider other)
    {
        GameSceneManager.Load(GameSceneManager.Scene.AngerScene);
    }
}