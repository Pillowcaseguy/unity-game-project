using UnityEngine;

public class ShootBlast : MonoBehaviour
{
    public GameObject shotgun_blast;
    public GameObject shotgun_bullet;
    public void shoot()
    {
        Instantiate(shotgun_blast,transform.position,transform.rotation);
        int target_platform = Random.Range(0, 2);
        for (int i = 0; i < 3; i++)
        {
            GameObject bullety = Instantiate(shotgun_bullet,transform.position,Quaternion.Euler(0,0, 15 - 15 * i - 5 * target_platform));
            ShotgunBullet hi = bullety.GetComponent<ShotgunBullet>();
            hi.Initialize(5, 195 - 15 * i - 5 * target_platform, 5);
        }
        /*for (int i = 0; i < 60; i++)
        {
            GameObject bullety = Instantiate(shotgun_bullet, new Vector2(transform.position.x, 0.1f * (i - 30)), Quaternion.Euler(0, 0, 0));
            ShotgunBullet hi = bullety.GetComponent<ShotgunBullet>();
            hi.Initialize(3, 180, 10);
        }*/
    }
}
