using UnityEngine;

public class SwordSlashAttack : MonoBehaviour
{

    private BoxCollider2D col;
    private Transform transform;
    private GameObject parentObj;
    private SwordsmanAI myAI;
    private float rotLowerLimit = 45f;
    private float rotHigherLimit = 135f;

    private EnemyAttack attackControl;

    //private int active_layer;
    //private int inactive_layer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        col = GetComponent<BoxCollider2D>();
        transform = GetComponent<Transform>();
        parentObj = transform.parent.gameObject;
        attackControl = GetComponent<EnemyAttack>();
        myAI = parentObj.GetComponent<SwordsmanAI>();
        //active_layer = LayerMask.NameToLayer("Active_Enemy_Attack");
        //inactive_layer = LayerMask.NameToLayer("Inactive_Enemy_Attack");
    }

    // Update is called once per frame
    void Update()
    {
        if (myAI.attackTimer > 0)
        {
            if (myAI.attackTimer < 0.34)
            {
                attackControl.EnableHitbox();
            }

            if (transform.rotation.eulerAngles.z > rotHigherLimit)
            {
                transform.rotation = Quaternion.Euler(0, 0, rotHigherLimit);
            }
            else
            {
                transform.Rotate(new Vector3(0, 0, 2f));
            }
            
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, rotLowerLimit);
            attackControl.DisableHitbox();
            attackControl.disableAttackCycle = false;
        }
        //Debug.Log(parentObj.name);
    }
}
