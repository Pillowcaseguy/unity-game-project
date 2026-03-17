using UnityEngine;

public class SmallHotDogTrail : MonoBehaviour
{
    private SpriteRenderer sprite;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sprite = GetComponentInParent<SpriteRenderer>();
    }

    public void show_sprite(bool show)
    {
        if (show)
            sprite.enabled = true;
        else
            sprite.enabled = false;
    }
}
