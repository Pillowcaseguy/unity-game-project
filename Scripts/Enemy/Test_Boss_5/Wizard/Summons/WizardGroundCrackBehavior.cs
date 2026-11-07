using UnityEngine;

public class WizardGroundCrackBehavior : MonoBehaviour
{
    public SpriteRenderer sprite;

    public float time_before_fade = 15f;
    void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
    }
    void FixedUpdate()
    {
        time_before_fade -= Time.deltaTime;
        if (time_before_fade <= 0)
        {
            sprite.color = new Vector4(1,1,1, sprite.color.a - 1 / 510f);
        }
        if (sprite.color.a <= 0f)
        {
            Destroy(gameObject);
        }
    }
}
