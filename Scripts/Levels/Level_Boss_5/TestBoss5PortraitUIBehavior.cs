using UnityEngine;
using UnityEngine.UI;

public class TestBoss5PortraitUIBehavior : MonoBehaviour
{
    public GameObject portrait_ninja;
    public Image portrait_ninja_bloodstains;
    public RectTransform portrait_ninja_rt;

    public GameObject portrait_soldier;
    public Image portrait_soldier_bloodstains;
    public RectTransform portrait_soldier_rt;

    public GameObject portrait_wizard;
    public Image portrait_wizard_bloodstains;
    public RectTransform portrait_wizard_rt;

    public Image portrait_ninja_death_x;
    public Image portrait_soldier_death_x;
    public Image portrait_wizard_death_x;

    public Image portrait_ninja_area_effect;
    public Image portrait_soldier_area_effect;
    public Image portrait_wizard_area_effect;

    public LevelManager_TestBoss5 manager;

    public bool is_swapping;
    public int[] order = new int[3];
    public float[] positions = new float[3];

    public bool test_swapping;
    public float area_effect_fade_cooldown;
    private void Awake()
    {
        order[0] = 1;
        order[1] = 2;
        order[2] = 3;
        positions[0] = 34.1f;
        positions[1] = -15.4f;
        positions[2] = -65.3f;
    }
    void Update()
    {
        if (!manager.has_all_bosses)
        {
            return;

        }
        update_bloodstains();
        if (is_swapping)
        {
            is_swapping = check_swap();
        }
        if (test_swapping)
        {
            int x = Random.Range(0, 3);
            int y = Random.Range(0, 3);
            Debug.Log("Swapping " + (x+1).ToString() + " with " + (y+1).ToString());
            do_swap(x, y);
            test_swapping = false;
        }

        area_effect_fade_cooldown -= Time.unscaledDeltaTime;
        if (area_effect_fade_cooldown <= 0)
        {
            area_effect_fade_cooldown = 0;
            if (portrait_ninja_area_effect.color.a > 0)
            {
                portrait_ninja_area_effect.color = new Vector4(portrait_ninja_area_effect.color.r, portrait_ninja_area_effect.color.g, portrait_ninja_area_effect.color.b, portrait_ninja_area_effect.color.a - 1 / 255f);
            }
            if (portrait_soldier_area_effect.color.a > 0)
            {
                portrait_soldier_area_effect.color = new Vector4(portrait_soldier_area_effect.color.r, portrait_soldier_area_effect.color.g, portrait_soldier_area_effect.color.b, portrait_soldier_area_effect.color.a - 1 / 255f);
            }
            if (portrait_wizard_area_effect.color.a > 0)
            {
                portrait_wizard_area_effect.color = new Vector4(portrait_wizard_area_effect.color.r, portrait_wizard_area_effect.color.g, portrait_wizard_area_effect.color.b, portrait_wizard_area_effect.color.a - 1 / 255f);
            }
        }
    }
    public void update_bloodstains()
    {
        float ninja_ratio = (float)manager.boss_life_ninja.health / manager.boss_life_ninja.max_health;
        float soldier_ratio = (float)manager.boss_life_soldier.health / manager.boss_life_soldier.max_health;
        float wizard_ratio = (float)manager.boss_life_wizard.health / manager.boss_life_wizard.max_health;

        if (manager.boss_life_ninja.health <= 0f)
        {
            portrait_ninja_bloodstains.sprite = manager.bloodstains_stage3;
            portrait_ninja_death_x.enabled = true;
        }
        else if (ninja_ratio >= 0.75f)
        {
            portrait_ninja_bloodstains.sprite = manager.bloodstains_stage0;
        }
        else if (ninja_ratio >= 0.5f && ninja_ratio < 0.75f)
        {
            portrait_ninja_bloodstains.sprite = manager.bloodstains_stage1;
        }
        else if (ninja_ratio >= 0.25f && ninja_ratio < 0.5f)
        {
            portrait_ninja_bloodstains.sprite = manager.bloodstains_stage2;
        }
        else if (ninja_ratio > 0f && ninja_ratio < 0.25f)
        {
            portrait_ninja_bloodstains.sprite = manager.bloodstains_stage3;
        }


        if (manager.boss_life_soldier.health <= 0f)
        {
            portrait_soldier_bloodstains.sprite = manager.bloodstains_stage3;
            portrait_soldier_death_x.enabled = true;
        }
        else if (soldier_ratio >= 0.75f)
        {
            portrait_soldier_bloodstains.sprite = manager.bloodstains_stage0;
        }
        else if (soldier_ratio >= 0.5f && soldier_ratio < 0.75f)
        {
            portrait_soldier_bloodstains.sprite = manager.bloodstains_stage1;
        }
        else if (soldier_ratio >= 0.25f && soldier_ratio < 0.5f)
        {
            portrait_soldier_bloodstains.sprite = manager.bloodstains_stage2;
        }
        else if (soldier_ratio > 0f && soldier_ratio < 0.25f)
        {
            portrait_soldier_bloodstains.sprite = manager.bloodstains_stage3;
        }


        if (manager.boss_life_wizard.health <= 0f)
        {
            portrait_wizard_bloodstains.sprite = manager.bloodstains_stage3;
            portrait_wizard_death_x.enabled = true;
        }
        else if (wizard_ratio >= 0.75f)
        {
            portrait_wizard_bloodstains.sprite = manager.bloodstains_stage0;
        }
        else if (wizard_ratio >= 0.5f && wizard_ratio < 0.75f)
        {
            portrait_wizard_bloodstains.sprite = manager.bloodstains_stage1;
        }
        else if (wizard_ratio >= 0.25f && wizard_ratio < 0.5f)
        {
            portrait_wizard_bloodstains.sprite = manager.bloodstains_stage2;
        }
        else if (wizard_ratio > 0f && wizard_ratio < 0.25f)
        {
            portrait_wizard_bloodstains.sprite = manager.bloodstains_stage3;
        }

    }
    public bool validate_swap(int index_1, int index_2)
    {
        if (index_1 < 0 || index_1 > 2 || index_2 < 0 || index_2 > 2 || index_1 == index_2)
        {
            return false;
        }

        return true;
    }
    public bool do_swap(int index_1, int index_2) // First index is the top one
    {
        if (!validate_swap(index_1, index_2))
        {
            return false;
        }

        if (!is_swapping)
        {
            is_swapping = true;
            int temp = order[index_1];
            order[index_1] = order[index_2];
            order[index_2] = temp;
            return true;
        }
        return false;
    }
    public bool check_swap()
    {
        bool something_moved = false;

        int ninja_position = System.Array.IndexOf(order, 1);
        if (portrait_ninja_rt.anchoredPosition.y != positions[ninja_position])
        {
            if (Mathf.Abs(portrait_ninja_rt.anchoredPosition.y - positions[ninja_position]) < 2.0f)
            {
                portrait_ninja_rt.anchoredPosition = new Vector2(portrait_ninja_rt.anchoredPosition.x, positions[ninja_position]);
            }
            else if (portrait_ninja_rt.anchoredPosition.y < positions[ninja_position])
            {
                portrait_ninja_rt.anchoredPosition = new Vector2(portrait_ninja_rt.anchoredPosition.x, portrait_ninja_rt.anchoredPosition.y + 2);
                something_moved = true;
            }
            else if (portrait_ninja_rt.anchoredPosition.y > positions[ninja_position])
            {
                portrait_ninja_rt.anchoredPosition = new Vector2(portrait_ninja_rt.anchoredPosition.x, portrait_ninja_rt.anchoredPosition.y - 2);
                something_moved = true;
            }
        }

        int soldier_position = System.Array.IndexOf(order, 2);
        if (portrait_soldier_rt.anchoredPosition.y != positions[soldier_position])
        {
            if (Mathf.Abs(portrait_soldier_rt.anchoredPosition.y - positions[soldier_position]) < 2.0f)
            {
                portrait_soldier_rt.anchoredPosition = new Vector2(portrait_soldier_rt.anchoredPosition.x, positions[soldier_position]);
            }
            else if (portrait_soldier_rt.anchoredPosition.y < positions[soldier_position])
            {
                portrait_soldier_rt.anchoredPosition = new Vector2(portrait_soldier_rt.anchoredPosition.x, portrait_soldier_rt.anchoredPosition.y + 2);
                something_moved = true;
            }
            else if (portrait_soldier_rt.anchoredPosition.y > positions[soldier_position])
            {
                portrait_soldier_rt.anchoredPosition = new Vector2(portrait_soldier_rt.anchoredPosition.x, portrait_soldier_rt.anchoredPosition.y - 2);
                something_moved = true;
            }
        }

        int wizard_position = System.Array.IndexOf(order, 3);
        if (portrait_wizard_rt.anchoredPosition.y != positions[wizard_position])
        {
            if (Mathf.Abs(portrait_wizard_rt.anchoredPosition.y - positions[wizard_position]) < 2.0f)
            {
                portrait_wizard_rt.anchoredPosition = new Vector2(portrait_wizard_rt.anchoredPosition.x, positions[wizard_position]);
            }
            else if (portrait_wizard_rt.anchoredPosition.y < positions[wizard_position])
            {
                portrait_wizard_rt.anchoredPosition = new Vector2(portrait_wizard_rt.anchoredPosition.x, portrait_wizard_rt.anchoredPosition.y + 2);
                something_moved = true;
            }
            else if (portrait_wizard_rt.anchoredPosition.y > positions[wizard_position])
            {
                portrait_wizard_rt.anchoredPosition = new Vector2(portrait_wizard_rt.anchoredPosition.x, portrait_wizard_rt.anchoredPosition.y - 2);
                something_moved = true;
            }
        }
        return something_moved;
    }
    public void inflict_area_effect_effect(int i)
    {
        if (i == 0)
        {
            portrait_ninja_area_effect.color = portrait_ninja_area_effect.color = new Vector4(portrait_ninja_area_effect.color.r, portrait_ninja_area_effect.color.g, portrait_ninja_area_effect.color.b, 220 / 255f);
        }
        if (i == 1)
        {
            portrait_soldier_area_effect.color = portrait_soldier_area_effect.color = new Vector4(portrait_soldier_area_effect.color.r, portrait_soldier_area_effect.color.g, portrait_soldier_area_effect.color.b, 220 / 255f);
        }
        if (i == 2)
        {
            portrait_wizard_area_effect.color = portrait_wizard_area_effect.color = new Vector4(portrait_wizard_area_effect.color.r, portrait_wizard_area_effect.color.g, portrait_wizard_area_effect.color.b, 220 / 255f);
        }
    }
}
