using UnityEngine;

public class LaserShootSpotBehavior : MonoBehaviour
{

    public TestBoss3_Phase3Behavior test_boss;
    public SpriteRenderer parent_sprite;
    public bool flipped_once = false;
    public bool unflipped_once = true;

    public float fixed_x_facing_left;
    public float fixed_x_facing_right;

    public float fixed_y;

    public GameObject laser_projectile;
    void Start()
    {
        test_boss = GetComponentInParent<TestBoss3_Phase3Behavior>();
    }
    public void Flip()
    {

        if (parent_sprite.flipX)
        {
            if (!flipped_once)
            {
                transform.localPosition = new Vector2(fixed_x_facing_right, fixed_y);
                flipped_once = true;
                unflipped_once = false;
            }
        }
        else
        {
            if (!unflipped_once)
            {
                transform.localPosition = new Vector2(fixed_x_facing_left, fixed_y);
                flipped_once = false;
                unflipped_once = true;
            }
        }
    }
    public void shoot_lasers()
    {
        if (test_boss.level_manager_behavior.has_player)
        {
            Vector2 temp = test_boss.level_manager_behavior.player.transform.position;
            Instantiate(laser_projectile,transform.position,Quaternion.Euler(0,0, Mathf.Atan2(temp.y - transform.position.y,temp.x - transform.position.x) * Mathf.Rad2Deg));
        }
    }

}
