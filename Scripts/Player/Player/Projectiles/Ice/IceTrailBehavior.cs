using UnityEngine;

public class IceTrailBehavior : MonoBehaviour
{

    private SpriteRenderer sprite;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
    }
    public void disableSprite()
    {
        sprite.enabled = false;
    }
}
