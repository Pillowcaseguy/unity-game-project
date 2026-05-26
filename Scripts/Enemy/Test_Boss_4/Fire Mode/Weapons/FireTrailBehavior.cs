using UnityEngine;

public class FireTrailBehavior : MonoBehaviour
{
    public BoxCollider2D box;
    public SpriteRenderer sprite;
    public EnemyAttack attack;

    public Vector2 start;
    public Vector2 target;
    void Awake()
    {
        box = GetComponent<BoxCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
        attack = GetComponent<EnemyAttack>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = start + (target - start) / 2;
        sprite.size = new Vector2(Mathf.Abs(target.x - start.x) / 2f, sprite.size.y);
        box.size = new Vector2(sprite.size.x, box.size.y);

        sprite.flipX = target.x < start.x;
    }
}
