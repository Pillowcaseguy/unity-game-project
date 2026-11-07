using UnityEngine;

public class TestBoss5_NinjaThrowableGroundCheck : MonoBehaviour
{
    public TestBoss5_NinjaThrowableBehavior throwable;
    public LayerMask ground_layer;
    public BoxCollider2D box;
    public bool first_set;
    void Awake()
    {
        ground_layer = LayerMask.GetMask("Ground");
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (first_set)
        {
            box.enabled = false;
            return; 
        }
        else if (!first_set && ((1 << col.gameObject.layer) & ground_layer) != 0)
        {
            throwable.start_deflection = true;
            throwable.attack.enabled = false;
            throwable.ani.SetBool("is_deflected", true);
            throwable.impact_angle = throwable.sprite.transform.rotation.eulerAngles.z;
            Instantiate(throwable.deflection_effect, throwable.sprite.transform.position, Quaternion.Euler(0, 0, 0));
            throwable.speed += 2;
            first_set = true;
        }
    }
}
