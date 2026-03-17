using UnityEngine;

public class woof_behavior : MonoBehaviour
{

    public float lifetime = 0.1f;
    public float time_limit = 3.5f;
    public GameObject player;
    public bool foundPlayer = false;

    private EnemyAttack attack;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        attack = GetComponent<EnemyAttack>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null)
        {
            return;
        }
        else if (!foundPlayer)
        {
            foundPlayer = true;
            lifetime = 2.0f;
        }

        lifetime -= Time.deltaTime;
        time_limit -= Time.deltaTime;
        transform.position = player.transform.position;

        if (lifetime <= 0.5 && !attack.isDestroyed)
        {
            attack.EnableHitbox();
            transform.localScale = new Vector2(0.4f, 0.4f);
        }
        else if (attack.isDestroyed)
        {
            attack.DisableHitbox();
        }

        if (time_limit <= 0.0)
        {
            Destroy(gameObject);
        }

    }
}
