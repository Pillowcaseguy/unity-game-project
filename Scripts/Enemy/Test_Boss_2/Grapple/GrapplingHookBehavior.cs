using UnityEngine;

public class GrapplingHookBehavior : MonoBehaviour
{
    public bool initialized = false;

    public GameObject owner;

    public void initialize(GameObject owna)
    {
        owner = owna;
        initialized = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!initialized)
        {
            return;
        }
        if (owner == null)
        {
            Destroy(gameObject);
        }
    }
}
