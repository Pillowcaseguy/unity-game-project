using UnityEngine;

public class SoldierFlashbangBackgroundBehavior : MonoBehaviour
{
    public SpriteRenderer sprite;
    public Camera cam;

    public float width;
    public float height;
    void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        cam = Camera.main;
        height = cam.orthographicSize * 2f;
        width = height * cam.aspect;

        Vector2 spriteSize = sprite.sprite.bounds.size;

        transform.localScale = new Vector2(width / spriteSize.x, height / spriteSize.y);
        transform.position = new Vector2(cam.transform.position.x, cam.transform.position.y);
    }

    void Update()
    {
        if (sprite.color.a <= 0f)
        {
            Destroy(gameObject);
        }
        transform.position = new Vector2(cam.transform.position.x, cam.transform.position.y);
    }
    void FixedUpdate()
    {
        sprite.color = new Vector4(sprite.color.r, sprite.color.g, sprite.color.b, sprite.color.a - 5 / 255f);
    }
}
