using UnityEngine;

public class ShooterMelee : MonoBehaviour
{

    private BoxCollider2D col;
    private GameObject parentObj;
    private ShooterAI myAI;

    private bool attackStarted = false;

    public LayerMask playerDamage;

    private EnemyAttack attackControl;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        col = GetComponent<BoxCollider2D>();
        parentObj = transform.parent.gameObject;
        attackControl = GetComponent<EnemyAttack>();
        myAI = parentObj.GetComponent<ShooterAI>();
    }

    // Update is called once per frame
    void Update()
    {

        if (attackControl.wasParried && attackStarted)
        {
            myAI.stunTime = 2.0f;
            endAttack();
        }

    }

    public void startAttack()
    {
        attackStarted = true;
        attackControl.EnableHitbox();
    }

    public void endAttack()
    {
        attackControl.clearFlags();
        attackControl.DisableHitbox();
        attackStarted = false;
        attackControl.disableAttackCycle = false;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (((1 << col.gameObject.layer) & playerDamage) != 0)
        {
            myAI.playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (((1 << col.gameObject.layer) & playerDamage) != 0)
        {
            myAI.playerInRange = false;
        }
    }

}
