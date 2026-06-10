using UnityEngine;

public class LightSuperBehavior : MonoBehaviour
{
    public GameObject start;
    public GameObject target;

    public SpriteRenderer sprite;
    public Animator ani;
    public PlayerAttack attack;

    public GameObject light_particle;
    public GameObject hit_impact;

    public EnemyDamageable found_1;
    public void initialize(GameObject starty, GameObject targety, EnemyDamageable target1)
    {
        start = starty;
        target = targety;

        Vector2 starto = start.transform.position;
        Vector2 targeto = target.transform.position;

        Vector2 ray_to_player = new Vector2(targeto.x - starto.x, targeto.y - starto.y);
        float calc_rotation = Mathf.Atan2(ray_to_player.y, ray_to_player.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, calc_rotation);
        transform.position = targeto + (starto - targeto) / 2f;
        sprite.size = new Vector2(Vector2.Distance(targeto, starto), sprite.size.y);

        found_1 = target1;
    }
    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        attack = GetComponent<PlayerAttack>();
        ani = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        ani.speed = 1 / Time.timeScale;
        Vector2 starto = start.transform.position;
        Vector2 targeto = start.transform.position;
        if (target != null)
        {
            targeto = target.transform.position;
        }

        Vector2 ray_to_player = new Vector2(targeto.x - starto.x, targeto.y - starto.y);
        float calc_rotation = Mathf.Atan2(ray_to_player.y, ray_to_player.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, calc_rotation);
        transform.position = targeto + (starto - targeto) / 2f;
        sprite.size = new Vector2(Vector2.Distance(targeto, starto), sprite.size.y);
    }
    public void apply_hit()
    {
        if (found_1 != null)
        {
            attack.damage = 55;
            found_1.Hit(attack);
            float random_offset_x = Random.Range(-1f, 1f);
            float random_offset_y = Random.Range(-1f, 1f);
            if (target == null)
                return;
            Instantiate(hit_impact, new Vector2(target.transform.position.x, target.transform.position.y) + new Vector2(random_offset_x, random_offset_y), Quaternion.Euler(0, 0, 0));
            for (int i = 0; i < 5; i++)
            {
                Instantiate(light_particle, target.transform.position, Quaternion.Euler(0, 0, 0));
            }
        }
    }
    public void apply_blindness()
    {
        attack.blindDuration = 6f;
    }
    public void destroy_me()
    {
        Destroy(gameObject);
    }
}
