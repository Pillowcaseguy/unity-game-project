using UnityEngine;

public class SwordIdleAttack : MonoBehaviour
{

    private BoxCollider2D col;
    private Transform transform;
    private GameObject parentObj;
    private SwordsmanAI myAI;

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
        if (myAI.attackTimer == 0)
        {
            attackControl.EnableHitbox();
        }
        else
        {
            attackControl.DisableHitbox();
        }
        //Debug.Log(parentObj.name);
    }
}
