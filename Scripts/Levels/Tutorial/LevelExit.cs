using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour
{
    public LayerMask player_layer;
    private void Awake()
    {
        player_layer = LayerMask.GetMask("Player");
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (((1 << col.gameObject.layer) & player_layer) != 0)
        {
            SceneManager.LoadScene("Main Menu");
        }
    }
}
