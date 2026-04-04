using UnityEngine;

public class TestBoss3_Phase1_ChamberBehavior : MonoBehaviour
{
    public GameObject falling_boss;
    public TestBoss3_Phase1Behavior the_boss;

    public void become_reflective()
    {
        the_boss.set_reflective(1);
    }
    public void become_unreflective()
    {
        the_boss.set_reflective(0);
    }
    public void fall_boss()
    {
        Instantiate(falling_boss, transform.position, Quaternion.Euler(0, 0, 0));
    }
}