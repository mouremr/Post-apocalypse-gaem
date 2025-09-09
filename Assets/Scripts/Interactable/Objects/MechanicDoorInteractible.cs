using UnityEngine;
using UnityEngine.SceneManagement;
public class MechanicDoorInteractible:Interactible
{

    [SerializeField] private string sceneToLoad = "ContainerInside";

    public override void Interact(GameObject player)
    {
        SceneManager.LoadScene(sceneToLoad);

    }

}
