using UnityEngine;

public class FloatingImages : MonoBehaviour
{
    public LevelManager_Tutorial level_manager;
    public SpriteRenderer sprite;
    void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
    }
    void Update()
    {
        if (level_manager.has_player)
        {
            if (level_manager.player.transform.position.y > 11)
            {
                sprite.enabled = true;
            }
            else
            {
                sprite.enabled = false;
            }
        }
    }
}
