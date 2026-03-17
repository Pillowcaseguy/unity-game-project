using UnityEngine;

public class ShooterShootAttack : MonoBehaviour
{
    private ShooterAI myAI;
    public Transform firePoint;
    public GameObject player;

    public GameObject bullet;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        myAI = gameObject.GetComponent<ShooterAI>();
    }
    public void shootBullet()
    {
        if (player == null)
        {
            if (myAI.player == null)
            {
                return;
            }

            else
            {
                player = myAI.player;
            }
        }

        Vector2 start_position = firePoint.transform.position;
        Vector2 end_position = player.gameObject.transform.position;

        Vector2 ray_to_player = new Vector2(end_position.x - start_position.x, end_position.y - start_position.y);
        float calc_rotation = Mathf.Atan2(ray_to_player.y, ray_to_player.x) * Mathf.Rad2Deg;
        float hypotenuse = Mathf.Sqrt(Mathf.Pow(ray_to_player.x, 2) + Mathf.Pow(ray_to_player.y, 2));
        firePoint.transform.rotation = Quaternion.Euler(0, 0, calc_rotation);

        GameObject boolet = Instantiate(bullet, firePoint.position, firePoint.rotation);
        BulletBehavior script = boolet.GetComponent<BulletBehavior>();
        script.Initialize(myAI.bullet_speed, 9);

    }
}
