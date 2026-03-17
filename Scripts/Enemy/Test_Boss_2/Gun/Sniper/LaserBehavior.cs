using UnityEngine;

public class LaserBehavior : MonoBehaviour
{

    GameObject player;
    SniperFirePoint firing_point;
    bool initialized = false;

    public void initialize(GameObject the_player, SniperFirePoint firePt)
    {
        player = the_player;
        firing_point = firePt;
        initialized = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!initialized)
        {
            return;
        }

        Vector2 snipe_position = firing_point.transform.position;
        Vector2 player_position = player.transform.position;

        Vector2 ray_to_player = new Vector2(player_position.x - snipe_position.x, player_position.y - snipe_position.y);
        float calc_rotation = Mathf.Atan2(ray_to_player.y, ray_to_player.x) * Mathf.Rad2Deg;
        //Debug.Log(calc_rotation);
        float hypotenuse = Mathf.Sqrt(Mathf.Pow(ray_to_player.x,2) + Mathf.Pow(ray_to_player.y,2));
        transform.rotation = Quaternion.Euler(0, 0, calc_rotation);
        transform.position = snipe_position;
        transform.localScale = new Vector2(hypotenuse * 6, 1);

    }
}
