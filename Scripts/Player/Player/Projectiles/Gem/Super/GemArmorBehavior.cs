using UnityEngine;

public class GemArmorBehavior : MonoBehaviour
{
    public GameObject owner;
    
    public void initialize(GameObject owna)
    {
        owner = owna;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = owner.transform.position;
    }
    public void destroy_me()
    {
        Destroy(gameObject);
    }
}
