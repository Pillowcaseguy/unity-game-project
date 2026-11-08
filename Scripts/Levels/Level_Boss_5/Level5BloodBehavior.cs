using UnityEngine;

public class Level5BloodBehavior : MonoBehaviour
{
    public SpriteRenderer sprite;

    public Sprite blood1;
    public Sprite blood2;
    public Sprite blood3;
    public Sprite blood4;
    public Sprite blood5;

    public float time_before_fade;
    void Awake()
    {
        time_before_fade = Random.Range(20f, 25f);
        sprite = GetComponent<SpriteRenderer>();

        int type = Random.Range(0, 5);
        if (type == 0)
        {
            sprite.sprite = blood1;
        }
        else if (type == 1)
        {
            sprite.sprite = blood2;
        }
        else if (type == 2)
        {
            sprite.sprite = blood3;
        }
        else if (type == 3)
        {
            sprite.sprite = blood4;
        }
        else if (type == 4)
        {
            sprite.sprite = blood5;
        }
        sprite.color = new Vector4(Random.Range(0.75f, 1), 1, 1, Random.Range(0.75f, 1));
    }
    void FixedUpdate()
    {
        time_before_fade -= Time.deltaTime;
        if (time_before_fade <= 0)
        {
            sprite.color = new Vector4(1, 1, 1, sprite.color.a - 1 / 510f);
        }
        if (sprite.color.a <= 0f)
        {
            Destroy(gameObject);
        }
    }
}
