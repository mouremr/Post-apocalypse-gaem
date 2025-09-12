using UnityEngine;

public class DontDestroy : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private static GameObject[] persistentObjects = new GameObject[5]; // list is shared across every object with this script
    public int objectIndex; // each object gets its slot number
    void Awake() //awake is called before start 
    {

        if (persistentObjects[objectIndex] == null)
        {
            persistentObjects[objectIndex] = gameObject;
            DontDestroyOnLoad(gameObject);
        }
        else if  (persistentObjects[objectIndex]!=gameObject){
            Destroy(gameObject);
        }
    }


}
