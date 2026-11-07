using UnityEngine;

public class TestBoss5WizardIceBackgroundBehavior : MonoBehaviour
{
    Camera cam;
    public float width;
    public float height;

    public SpriteRenderer sprite;

    public float appearance_delay;
    public bool fading_out;

    void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        sprite.color = new Vector4(1, 1, 1, 0);
    }
    void Start()
    {
        cam = Camera.main;
        height = cam.orthographicSize * 2f;
        width = height * cam.aspect;
        Vector2 spriteSize = sprite.sprite.bounds.size;

        transform.localScale = new Vector2(width / spriteSize.x, height / spriteSize.y);
    }
    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector2(cam.transform.position.x, cam.transform.position.y);
    }
    private void FixedUpdate()
    {
        appearance_delay -= Time.deltaTime;
        if (appearance_delay <= 0)
        {
            appearance_delay = 0;
        }

        if (!fading_out)
        {
            if (sprite.color.a <= 60 / 255f && appearance_delay <= 0f)
            {
                sprite.color = new Vector4(1, 1, 1, sprite.color.a + 0.005f);
            }
        }
        else
        {
            if (sprite.color.a >= 0f)
            {
                sprite.color = new Vector4(1, 1, 1, sprite.color.a - 0.005f);
            }
            if (sprite.color.a <= 0f)
            {
                Destroy(gameObject);
            }
        }
    }
}
