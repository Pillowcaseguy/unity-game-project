using UnityEngine;

public class SoldierFlashbangBehavior : MonoBehaviour
{
    public SpriteRenderer sprite;
    public Rigidbody2D rb;
    public EnemyAttack attack;

    public LevelManager_TestBoss5 manager;
    public GameObject flashbang_screen;
    public float lifetime;
    public bool flashed;

    public LayerMask ground_layer;
    void Awake()
    {
        lifetime = 4;
        sprite = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0.7f;
        attack = GetComponent<EnemyAttack>();
        ground_layer = LayerMask.GetMask("Ground", "BreakableGround");
    }
    void Update()
    {
        lifetime -= Time.deltaTime;
        if (lifetime <= 0 && !flashed)
        {
            flash();
        }
    }
    private void FixedUpdate()
    {
        float sprite_rotate = rb.linearVelocityX > 0 ? -5 : 5;
        sprite.transform.rotation = Quaternion.Euler(0, 0, sprite.transform.rotation.eulerAngles.z + sprite_rotate);
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (((1 << col.gameObject.layer) & ground_layer) != 0)
        {
            flash();
        }
    }
    public void flash()
    {
        if (flashed)
        {
            return;
        }
        flashed = true;
        float distance = Mathf.Abs(transform.position.x - manager.player.transform.position.x);

        if (distance <= 2)
        {
            attack.stunDuration = 6f;
            attack.blindDuration = 4f;
        }
        else if ((distance > 2 && distance < 6) || distance_check())
        {
            attack.stunDuration = Mathf.Max(10f / distance, 3f);
            attack.blindDuration = 2f;
        }
        else
        {
            attack.stunDuration = 0;
            attack.blindDuration = 0;
        }

        var hi = GameManager.Instance.current_camera.GetComponent<CameraShake>();
        if (hi != null)
        {
            hi.shakeOnHurt(1f);
        }
        manager.player_life.Hit_No_Collider(gameObject);
        Instantiate(flashbang_screen);
        Destroy(gameObject);
    }
    public bool distance_check()
    {
        if (manager.player_life.movement.sprite_render.flipX && manager.player.transform.position.x >= transform.position.x)
            return true;
        else if (!manager.player_life.movement.sprite_render.flipX && manager.player.transform.position.x <= transform.position.x)
            return true;
        else
            return false;
    }
}
