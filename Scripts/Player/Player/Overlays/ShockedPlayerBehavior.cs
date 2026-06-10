using UnityEngine;

public class ShockedPlayerBehavior : MonoBehaviour
{

    GameObject owner;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void initialize(GameObject hi)
    {
        owner = hi;
    }

    // Update is called once per frame
    void Update()
    {
        if (owner != null)
        {
            transform.position = owner.transform.position;
        }
    }
}
