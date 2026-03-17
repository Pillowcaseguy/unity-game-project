using UnityEngine;

public class BlockingSword : MonoBehaviour
{
    private BoxCollider2D box;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        box = GetComponent<BoxCollider2D>();
    }

    public void enable()
    {
        box.enabled = true;
    }

    public void disable()
    {
        box.enabled = false;
    }

}
