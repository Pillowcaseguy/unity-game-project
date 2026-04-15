using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
    public GameObject the_player;
    public float camera_size;

    void Start()
    {
        camera_size = Camera.main.orthographicSize;
    }
    void Update()
    {
        if (the_player == null) {
            the_player = GameManager.Instance.current_player;
        }

        if (the_player != null)
        {
            //Debug.Log(transform.position - the_player.transform.position);
            //Debug.Log(Camera.main.orthographicSize);

            Vector3 temp_thing = Vector3.Lerp(transform.position, the_player.transform.position, Time.deltaTime * 2);
            temp_thing.z = -10;
            transform.position = temp_thing;
        }
    }
}
