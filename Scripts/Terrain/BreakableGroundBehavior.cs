using UnityEngine;

public class BreakableGroundBehavior : MonoBehaviour
{

    SpriteRenderer sprite;
    BoxCollider2D box;
    Animator ani;
    public float break_time = 0;
    public float restore_time = 0;

    public bool is_destroying;

    public bool is_restoring;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        box = GetComponent<BoxCollider2D>();
        ani = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (break_time > 0)
        {
            break_time -= Time.deltaTime;
            is_destroying = true;
            if (break_time <= 0)
            {
                restoreGround();
                break_time = 0;
                is_destroying = false;
            }
        }

        if (restore_time > 0)
        {
            restore_time -= Time.deltaTime;
            is_restoring = true;
            sprite.color = new Color(1f,1f,1f, 1 - restore_time / 5f);
            if (restore_time <= 0)
            {
                sprite.color = new Color(1f, 1f, 1f, 1f);
                restore_time = 0;
                is_restoring = false;
            }
        }

        if (sprite.color.a < 0.8f)
        {
            if (box.enabled)
            {
                box.enabled = false;
            }
        }
        else
        {
            if (!box.enabled)
            {
                gameObject.layer = LayerMask.NameToLayer("BreakableGround");
                box.enabled = true;
            }
        }

        ani.SetBool("is_breaking", is_destroying);

    }
    public void breakGround()
    {
        break_time = 12;
    }
    public void finished_breaking()
    {
        sprite.color = new Color(1.0f,1.0f,1.0f,0.0f);
        gameObject.layer = LayerMask.NameToLayer("Player_Damage");
    }
    public void restoreGround()
    {
        restore_time = 5;
    }
}
