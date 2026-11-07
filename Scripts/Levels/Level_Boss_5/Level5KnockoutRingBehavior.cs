using UnityEngine;

public class Level5KnockoutRingBehavior : MonoBehaviour
{
    public SpriteRenderer sprite;
    public GameObject knockout_spark;
    void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        var temp = Instantiate(knockout_spark, transform.position, Quaternion.Euler(0, 0, 0));
        Destroy(temp, 0.5f);
    }

    void FixedUpdate()
    {
        sprite.color = new Vector4(sprite.color.r, sprite.color.g, sprite.color.b, sprite.color.a - 0.0067f);        
        transform.localScale = new Vector2(transform.localScale.x + 0.1f, transform.localScale.y + 0.08f);
        if (sprite.color.a <= 0.0f)
        {
            Destroy(gameObject);
        }
    }
}
