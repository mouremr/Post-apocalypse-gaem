using UnityEngine;
using UnityEngine.SceneManagement;


public class SpawnHandler : MonoBehaviour
{
    void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, LoadSceneMode mode)
    {
        // Use the spawn point that the door told us
        GameObject spawnPoint = GameObject.Find("SpawnPoint");

        if (spawnPoint != null)
        {
            transform.position = spawnPoint.transform.position;
        
        }

    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
