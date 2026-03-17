using UnityEngine;

public class SwordsmanAI : MonoBehaviour
{

    public bool isStanding = false;
    public bool isAttacking = false;

    public float attackTimer;
    public float timeSinceLastAttack;
    public float deathTimer;

    private BoxCollider2D col;
    private Transform transform;
    private Animator ani;
    private EnemyHealth enemyHealth;

    //Temp
    private bool shutup = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        isStanding = true;
        col = GetComponent<BoxCollider2D>();
        transform = GetComponent<Transform>();
        ani = GetComponent<Animator>();
        enemyHealth = GetComponent<EnemyHealth>();
        timeSinceLastAttack = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (!enemyHealth.isDead)
        {
            if (!isAttacking)
            {

                /*if (timeSinceLastAttack > 4.0f && !shutup)
                {
                    Debug.Log("WILL ATTACK IN A SECOND!");
                    shutup = true;
                }*/

                if (timeSinceLastAttack > 5.0f)
                {
                    isAttacking = true;
                    isStanding = false;
                    attackTimer = 0.5f;
                }

            }

            if (attackTimer > 0f)
            {

                attackTimer -= Time.deltaTime;

                if (attackTimer < 0f)
                {
                    attackTimer = 0f;
                }

            }

            if (attackTimer == 0f && isAttacking)
            {
                stopAttacking();
            }

        }

        timeSinceLastAttack += Time.deltaTime;

        ani.SetBool("isStanding", isStanding);
        ani.SetBool("isAttacking", isAttacking);
        ani.SetBool("isDead", enemyHealth.isDead);

    }
    void stopAttacking()
    {
        isAttacking = false;
        isStanding = true;
        timeSinceLastAttack = 0f;
        //shutup = false;
    }

}
