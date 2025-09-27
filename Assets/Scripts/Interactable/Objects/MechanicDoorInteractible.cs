using UnityEngine;
using UnityEngine.SceneManagement;
public class MechanicDoorInteractible : Interactible
{

    [SerializeField] private string sceneToLoad = "ContainerInside";
    //[SerializeField] private string spawnPointName = "SpawnPoint"; //nameof target spawn point in new scene

    public override void Interact(GameObject player)
    {
        SceneManager.LoadScene(sceneToLoad);
    }
    

    

}
