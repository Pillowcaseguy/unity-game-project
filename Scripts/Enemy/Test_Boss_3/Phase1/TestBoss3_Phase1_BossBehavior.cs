using UnityEngine;

public class TestBoss3_Phase1_BossBehavior : MonoBehaviour
{
    public TestBoss3_Phase1Behavior the_boss;
    public void call_drones()
    {
        the_boss.summon_drone();
    }
    public void stop_attacking()
    {
        the_boss.end_attack();
    }
}