using UnityEngine;
using UnityEngine.UI;

public class PlayerCutout : MonoBehaviour
{

    public Image white_blank;
    public Material open_spot;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        open_spot = Instantiate(white_blank.material);
        white_blank.material = open_spot;
        
        open_spot.SetFloat("_Feather", 0.05f);
        open_spot.SetFloat("_AspectRatio", (float)Screen.width / Screen.height);
        open_spot.SetColor("_Color", Color.black);
    }

    // Update is called once per frame
    void Update()
    {
        GameObject player = GameManager.Instance.current_player;
        Camera cam = GameManager.Instance.current_camera;

        open_spot.SetFloat("_CutoutRadius", 0.15f);

        if (player != null && cam != null)
        {
            Vector3 camera_view = cam.WorldToViewportPoint(player.transform.position);
            open_spot.SetVector("_CutoutPos", new Vector4(camera_view.x, camera_view.y, 0, 0));
        }
        else
        {
            open_spot.SetVector("_CutoutPos", new Vector4(0, 0, 0, 0));
            open_spot.SetFloat("_CutoutRadius", 0.0f);
        }
    }
}
