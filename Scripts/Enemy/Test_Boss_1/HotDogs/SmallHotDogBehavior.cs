using Unity.VisualScripting;
using UnityEngine;
public class SmallHotDogBehavior : MonoBehaviour
{
    public GameObject bigHotDogOwner;
    public GameObject explosion;

    private Rigidbody2D rb;
    private BoxCollider2D box;
    private EnemyAttack attack;
    private SpriteRenderer sprite;
    private SmallHotDogTrail pofff;
    private SmallHotDogShadow shadow;

    public bool can_explode = true;
    public bool exploded = false;
    public bool aiming_downwards = false;
    public bool first_created = true;
    public int current_phase = 0;

    public float rotated_amount;
    public float starting_y;

    public float current_height_since_creation;

    public float time_since_phase_3 = 0;

    public BigHotDogBehavior bigHotDogComms;

    private CameraShake cameraShake;

    public LayerMask groundLayer;
    public LayerMask playerLayer;
    public LayerMask combinedLayer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        rb = GetComponent<Rigidbody2D>();
        box = GetComponent<BoxCollider2D>();
        attack = GetComponent<EnemyAttack>();
        sprite = GetComponent<SpriteRenderer>();
        pofff = GetComponentInChildren<SmallHotDogTrail>();
        shadow = GetComponentInChildren<SmallHotDogShadow>();
        cameraShake = GameObject.Find("Main Camera").GetComponent<CameraShake>();
        starting_y = transform.position.y;
        attack.destroyTimer = 0.5f;

        groundLayer = LayerMask.GetMask("Ground");

    }
    public void Initialize(GameObject owner)
    {
        bigHotDogOwner = owner;
        bigHotDogComms = owner.GetComponent<BigHotDogBehavior>();
    }

    // Update is called once per frame
    void Update()
    {
        if (bigHotDogOwner == null)
        {
            return;
        }

        current_height_since_creation = transform.position.y - starting_y;

        if (current_phase == 0)
        {
            if (first_created && current_height_since_creation < 0.95f)
            {
                rb.linearVelocityY = 0.2f;
            }
            else
            {
                first_created = false;
                rb.linearVelocityY = 0.0f;
                bigHotDogComms.has_a_small_hot_dog = true;
                sprite.enabled = false;
            }
        }

        if (current_phase == 1)
        {
            rotated_amount -= 0.1f;
            if (rotated_amount > -45)
            {
                sprite.enabled = true;
                rb.rotation += -0.1f;
            }
            else
            {
                current_phase += 1;
            }
        }

        if (current_phase == 2)
        {
            if (current_height_since_creation > 7.0f)
            {
                current_phase += 1;
                rb.linearVelocityX = 0;
                rb.linearVelocityY = 0;
            }
            else
            {
                pofff.show_sprite(true);
                rb.linearVelocityX = -2.0f;
                rb.linearVelocityY = 2.0f;
            }
        }

        if (current_phase == 3)
        {
            Transform playerAttempt = GameObject.Find("Player").GetComponent<Transform>();

            if (time_since_phase_3 == 0)
            {
                shadow.activated = true;
                shadow.detach_shadow();
            }

            time_since_phase_3 += Time.deltaTime;

            if (time_since_phase_3 > 4.0f)
            {
                rb.linearVelocityY = -10.0f;
                rb.rotation = 90;
                current_phase += 1;
            }
            else
            {
                transform.position = (playerAttempt == null) ? new Vector2(0f, transform.position.y) : new Vector2(playerAttempt.position.x, transform.position.y);
            }
        }

        if (current_phase == 4)
        {
            if (attack.wasHit || attack.wasBlocked)
            {
                if (!exploded)
                {
                    exploded = true;
                    explode();
                }
            }

            else if (attack.wasParried)
            {
                can_explode = false;
            }
        }

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & groundLayer) != 0)
        {
            if (collision.CompareTag("Doesn'tBlockProjectiles"))
            {
                shadow.hide_shadow();
                return;
            }
            else if (!can_explode)
            {
                if (!exploded)
                {
                    exploded = true;
                    rb.linearVelocityY = 0;
                }
                transform.rotation = Quaternion.Euler(0, 0, 180);
                rb.bodyType = RigidbodyType2D.Dynamic;
                attack.destroyTimer = 1.5f;
                attack.DisableHitbox();
                pofff.show_sprite(false);
                attack.startDestroy();
            }
            else
            {
                if (!exploded)
                {
                    exploded = true;
                    explode();
                }
            }
        }
    }
    void explode()
    {
        pofff.show_sprite(false);
        cameraShake.shakeOnHurt(1.8f);
        Instantiate(explosion,new Vector2(transform.position.x,transform.position.y - 1.2f),Quaternion.Euler(0,0,0));
        rb.linearVelocityY = 0;
        sprite.enabled = false;
        shadow.hide_shadow();
        attack.DisableHitbox();
        attack.startDestroy();
    }

}
