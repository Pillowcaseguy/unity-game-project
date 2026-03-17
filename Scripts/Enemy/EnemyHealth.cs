using UnityEngine;

public class EnemyHealth : MonoBehaviour
{

    public bool isDead = false;
    public int health;
    public int max_health;
    public float deathTimer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        max_health = 5;
        health = 5;
    }

    // Update is called once per frame
    void Update()
    {

        if (deathTimer > 0f)
        {

            deathTimer -= Time.deltaTime;

            if (deathTimer < 0f)
            {
                deathTimer = 0f;
            }

        }

        if (health < 0 && !isDead)
        {
            isDead = true;
            deathTimer = 2.0f;
            Die();
        }

    }
    public void Hit(Collider2D col)
    {
        PlayerAttack contact = col.gameObject.GetComponent<PlayerAttack>();
        contact.isHit();
        Debug.Log("nothing yet");
    }
    public void Die()
    {
        transform.Rotate(0, 0, 90); // Replace with more complex death later
    }

}
