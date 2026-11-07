using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class LevelManager_TestBoss5 : MonoBehaviour
{
    public GameObject boss_ninja;
    public TestBoss5Behavior_Ninja boss_life_ninja;
    public bool has_boss_ninja;

    public GameObject boss_soldier;
    public TestBoss5Behavior_Soldier boss_life_soldier;
    public bool has_boss_soldier;

    public GameObject boss_wizard;
    public TestBoss5Behavior_Wizard boss_life_wizard;
    public bool has_boss_wizard;

    public int number_of_dead_bosses;

    public bool has_all_bosses;

    public GameObject jumping_up_soldier;
    public GameObject jumping_down_soldier;

    public GameObject jumping_up_wizard;
    public GameObject jumping_down_wizard;

    public GameObject player;
    public PlayerDamageAndHealth player_life;
    public bool has_player;

    public bool level_loading;

    public GameObject spawn_point;

    public int current_boss_in_play; // 0 = ninja, 1 = soldier, 2 = wizard
    public int temporary_stored_swap_value = -1;
    public int old_boss_in_play = -1;

    public Image ninja_portrait;
    public Image soldier_portrait;
    public Image wizard_portrait;

    public Sprite bloodstains_stage0;
    public Sprite bloodstains_stage1;
    public Sprite bloodstains_stage2;
    public Sprite bloodstains_stage3;

    public bool just_started = true;
    public float time_since_swap;

    public GameObject knocked_player;
    public GameObject knockout_wave;

    public TestBoss5PortraitUIBehavior extra_enemy_UI;

    public bool current_boss_died;
    public bool current_boss_died_invoke_called;

    public GameObject Knockout_Text;
    public GameObject Victory_Text;
    public GameObject Burn_Tip_Text;
    public bool first_burn;

    public bool first_high_charge;
    public bool first_low_charge;

    public float super_burn_timer;
    public float super_burn_damage_timer;
    public float super_burn_damage_timer_max;
    public float delay_to_extinguish_super_burn;
    public float ground_fire_hit_cooldown;

    public GameObject sticky_burn_effect;
    public StickyFireEffectBehavior current_sticky_burn_effect;
    public EnemyAttack sticky_burn_damage;
    public GameObject sticky_burn_damage_particle;

    public GameObject hit_particle;

    public float tiny_heal;

    public int index_of_ninja;
    public int index_of_soldier;
    public int index_of_wizard;

    public Level5AreaEffectText second_opponent;
    public Level5AreaEffectText third_opponent;

    public float area_effect_fade_cooldown;
    private void Awake()
    {
        tiny_heal = 25f;
        super_burn_damage_timer_max = 2.5f;
        index_of_ninja = 0;
        index_of_soldier = 1;
        index_of_wizard = 2;
    }
    void Update()
    {
        time_since_swap += Time.deltaTime;

        if (player_life == null)
        {
            GameManager.Instance.playerUI.set_health_bar = false;
            GameManager.Instance.instantiate_player(spawn_point.transform.position, 16, 16);
            player = GameManager.Instance.current_player;
            player_life = GameManager.Instance.player_health;
            has_player = true;
        }

        if (!has_all_bosses)
        {
            has_boss_ninja = boss_ninja != null && boss_life_ninja != null;
            has_boss_soldier = boss_soldier != null && boss_life_soldier != null;
            has_boss_wizard = boss_wizard != null && boss_life_wizard != null;

            has_all_bosses = has_boss_ninja && has_boss_soldier && has_boss_wizard;
            current_boss_in_play = 0;
        }

        if (has_all_bosses && has_player)
        {
            if (boss_life_ninja.is_dead && boss_life_soldier.is_dead && boss_life_wizard.is_dead)
            {
                player_life.add_true_invincibility_time(1);
                if (!level_loading)
                {
                    var hi = Instantiate(Knockout_Text).GetComponent<Level5KnockoutText>().rect;
                    hi.position = new Vector2(4.3f, 4.5f);

                    var hi2 = Instantiate(Victory_Text).GetComponent<Level5VictoryText>();
                    hi2.rect.position = new Vector2(-4f, 3.2f);
                    hi2.begin_fadeout_count = 3;

                    Invoke("load_next_level", 7f);
                    level_loading = true;
                }
            }

            if (player_life.health <= 0)
            {
                if (!level_loading)
                {
                    var hi = Instantiate(Knockout_Text).GetComponent<Level5KnockoutText>().rect;
                    hi.position = new Vector2(-8.8f, 4.5f);
                    if (current_boss_in_play == 2)
                        Invoke("load_next_level", 8f);
                    else
                        Invoke("load_next_level", 5f);
                    level_loading = true;
                }
            }
            else
            {
                if (player_life.movement.ani != null && player_life.movement.isSuperAttacking && player_life.player_attacking.currentWeapon == PlayerAttacking.WeaponType.Ice)
                {
                    extinguish_super_burn();
                }
            }

            if (just_started && has_all_bosses)
            {
                boss_life_ninja.summon_falling_ninja();
                just_started = false;
            }

            if (boss_ninja.transform.position.y <= -30f)
            {
                boss_life_ninja.end_attack();
                boss_life_ninja.rb.linearVelocity = new Vector2(0, 0);
                boss_life_ninja.transform.position = new Vector2(0, 0);
                //boss_life_ninja.TakeDamage(99999);
            }
            if (boss_life_soldier.transform.position.y <= -30f)
            {
                boss_life_soldier.end_attack();
                boss_life_soldier.rb.linearVelocity = new Vector2(0, 0);
                boss_life_soldier.transform.position = new Vector2(0, 0);
                //boss_life_soldier.TakeDamage(99999);
            }
            if (Mathf.Abs(boss_life_wizard.transform.position.y) >= 20f || Mathf.Abs(boss_life_wizard.transform.position.x) >= 25f)
            {
                boss_life_wizard.end_attack();
                boss_life_wizard.rb.linearVelocity = new Vector2(0, 0);
                boss_life_wizard.transform.position = new Vector2(0, 0);
                //boss_life_wizard.TakeDamage(99999);
            }
            if (player.transform.position.y <= -30)
            {
                player_life.Hit_No_Collider(gameObject);
            }

            if (current_boss_in_play == 0 && boss_life_ninja.is_dead)
            {
                current_boss_died = true;
            }
            else if (current_boss_in_play == 1 && boss_life_soldier.is_dead)
            {
                current_boss_died = true;
            }
            else if (current_boss_in_play == 2 && boss_life_wizard.is_dead)
            {
                current_boss_died = true;
            }

            if (number_of_dead_bosses < 3 && current_boss_died && !current_boss_died_invoke_called)
            {
                if (temporary_stored_swap_value != -1)
                {
                    current_boss_died_invoke_called = true;
                    Invoke("call_new_boss", 0.5f);
                    handle_death();
                }
                else
                {
                    perform_swap();
                }
            }

            if (time_since_swap >= 55f && !current_boss_died && number_of_dead_bosses < 2 && temporary_stored_swap_value == -1)
            {
                if (current_boss_in_play == 0 && !boss_life_ninja.is_dead)
                {
                    boss_life_ninja.called_to_swap = true;
                }
                else if (current_boss_in_play == 1 && !boss_life_soldier.is_dead)
                {
                    boss_life_soldier.called_to_swap = true;
                }
                else if (current_boss_in_play == 2 && !boss_life_wizard.is_dead)
                {
                    boss_life_wizard.called_to_swap = true;
                }
                perform_swap();
            }

            if (super_burn_timer > 0)
            {
                super_burn_timer -= Time.deltaTime;
                super_burn_damage_timer -= Time.deltaTime;
                if (super_burn_damage_timer <= 0)
                {
                    super_burn_damage_timer = super_burn_damage_timer_max;
                    sticky_burn_damage.clearFlags();
                    player_life.Hit_No_Collider(sticky_burn_damage.gameObject);
                    for (int i = 0; i < 50; i++)
                    {
                        Instantiate(sticky_burn_damage_particle, player.transform.position, Quaternion.Euler(0, 0, 0));
                    }
                    player_life.freezeTime = 0;
                }
                if (super_burn_timer <= 0)
                {
                    extinguish_super_burn();
                }
            }

            if (delay_to_extinguish_super_burn > 0)
            {
                delay_to_extinguish_super_burn -= Time.deltaTime;
                if (delay_to_extinguish_super_burn < 0)
                {
                    delay_to_extinguish_super_burn = 0;
                }
            }

            if (ground_fire_hit_cooldown > 0)
            {
                ground_fire_hit_cooldown -= Time.deltaTime;
                if (ground_fire_hit_cooldown < 0)
                {
                    ground_fire_hit_cooldown = 0;
                }
            }
        }

        if (player_life != null && (float)player_life.health / player_life.maximum_health <= 0.3f)
        {
            tiny_heal -= Time.deltaTime;
        }

        if (tiny_heal <= 0)
        {
            player_life.Heal(1);
            tiny_heal = 25f;
        }
    }
    public void hide_player(bool b)
    {
        if (has_player)
        {
            if (b)
            {
                player_life.movement.sprite_render.enabled = false;
            }
            else
            {
                player_life.movement.sprite_render.enabled = true;
            }
        }
    }
    public void spawn_knockout_wave()
    {
        if (has_player)
        {
            if (!(player_life.movement.anything_super_related || Time.timeScale == 0.1f))
            {
                Time.timeScale = 0.3f;
                Invoke("fix_time_scale", 0.6f);
            }
        }

        EnemyActiveBossComponent[] bosses = FindObjectsByType<EnemyActiveBossComponent>(FindObjectsSortMode.None);
        GameObject target = null;
        foreach (EnemyActiveBossComponent test in bosses)
        {
            if (test.isPrimaryTarget)
            {
                target = test.gameObject;
                bool flipped = test.GetComponent<SpriteRenderer>().flipX;
                float distance = flipped ? 0.22f : -0.22f;
                var wave = Instantiate(knockout_wave, new Vector2(target.transform.position.x + distance, target.transform.position.y + 0.25f), Quaternion.Euler(0, 0, 0));
            }
        }
    }
    public void fix_time_scale()
    {
        if (has_player)
        {
            if (player_life.movement.anything_super_related || Time.timeScale == 0.1f)
            {
                return;
            }
            else
            {
                Time.timeScale = 1f;
            }
        }
    }
    public void perform_swap()
    {
        int new_boss = -1;
        if (current_boss_in_play == 0)
        {  
            new_boss = Random.Range(1, 3);
            if (new_boss == 1)
            {
                if (!boss_life_soldier.is_dead)
                {
                    //boss_life_ninja.called_to_swap = true;
                    temporary_stored_swap_value = new_boss;
                }
                else if (!boss_life_wizard.is_dead)
                {
                    new_boss = 2;
                    temporary_stored_swap_value = new_boss;
                }
                else
                {
                    new_boss = -1;
                    temporary_stored_swap_value = new_boss;
                }
            }
            if (new_boss == 2)
            {
                if (!boss_life_wizard.is_dead)
                {
                    //boss_life_ninja.called_to_swap = true;
                    temporary_stored_swap_value = new_boss;
                }
                else if (!boss_life_soldier.is_dead)
                {
                    new_boss = 1;
                    temporary_stored_swap_value = new_boss;
                }
                else
                {
                    new_boss = -1;
                    temporary_stored_swap_value = new_boss;
                }
            }

        }
        else if (current_boss_in_play == 1)
        {
            new_boss = Random.Range(1, 5);
            if (new_boss == 1 || new_boss == 3)
            {
                new_boss = 0;
            }
            else
            {
                new_boss = 2;
            }

            if (new_boss == 0)
            {
                if (!boss_life_ninja.is_dead)
                {
                    temporary_stored_swap_value = new_boss;
                }
                else if (!boss_life_wizard.is_dead)
                {
                    new_boss = 2;
                    temporary_stored_swap_value = new_boss;
                }
            }
            if (new_boss == 2)
            {
                if (!boss_life_wizard.is_dead)
                {
                    temporary_stored_swap_value = new_boss;
                }
                else if (!boss_life_ninja.is_dead)
                {
                    new_boss = 0;
                    temporary_stored_swap_value = new_boss;
                }
                else
                {
                    new_boss = -1;
                    temporary_stored_swap_value = new_boss;
                }
            }
        }
        else if (current_boss_in_play == 2)
        {
            new_boss = Random.Range(0, 2);
            if (new_boss == 0)
            {
                if (!boss_life_ninja.is_dead)
                {
                    //boss_life_wizard.called_to_swap = true;
                    temporary_stored_swap_value = new_boss;
                }
                else if (!boss_life_soldier.is_dead)
                {
                    new_boss = 1;
                    temporary_stored_swap_value = new_boss;
                }
                else
                {
                    new_boss = -1;
                    temporary_stored_swap_value = new_boss;
                }
            }
            if (new_boss == 1)
            {
                if (!boss_life_soldier.is_dead)
                {
                    //boss_life_wizard.called_to_swap = true;
                    temporary_stored_swap_value = new_boss;
                }
                else if (!boss_life_ninja.is_dead)
                {
                    new_boss = 0;
                    temporary_stored_swap_value = new_boss;
                }
                else
                {
                    new_boss = -1;
                    temporary_stored_swap_value = new_boss;
                }
            }
        }

        if (new_boss == 0 && boss_life_ninja.is_dead)
        {
            new_boss = -1;
        }
        if (new_boss == 1 && boss_life_soldier.is_dead)
        {
            new_boss = -1;
        }
        if (new_boss == 2 && boss_life_wizard.is_dead)
        {
            new_boss = -1;
        }

        bool result = extra_enemy_UI.validate_swap(current_boss_in_play, new_boss);
        if (result)
        {
            old_boss_in_play = current_boss_in_play;
            temporary_stored_swap_value = new_boss;
        }
    }
    public void complete_handshake()
    {
        if (current_boss_in_play == 0)
        {
            Invoke("call_new_boss", 1f);
            boss_life_ninja.GetComponent<EnemyActiveBossComponent>().notPrimary();
        }
        else if (current_boss_in_play == 1)
        {
            Invoke("call_new_boss", 4f);
            boss_life_soldier.GetComponent<EnemyActiveBossComponent>().notPrimary();
        }
        else if (current_boss_in_play == 2)
        {
            Invoke("call_new_boss", 0.5f);
            boss_life_wizard.GetComponent<EnemyActiveBossComponent>().notPrimary();
        }
        current_boss_in_play = temporary_stored_swap_value;
        time_since_swap = 0;
        temporary_stored_swap_value = -1;

        if (current_boss_in_play == 0)
        {
            boss_life_ninja.GetComponent<EnemyActiveBossComponent>().isPrimary();
        }
        else if (current_boss_in_play == 1)
        {
            boss_life_soldier.GetComponent<EnemyActiveBossComponent>().isPrimary();
        }
        else if (current_boss_in_play == 2)
        {
            boss_life_wizard.all_actions_cooldown = 5f;
            boss_life_wizard.GetComponent<EnemyActiveBossComponent>().isPrimary();
        }
    }
    public void call_new_boss()
    {
        current_boss_died = false;
        current_boss_died_invoke_called = false;

        if (old_boss_in_play == 0)
        {
            int temp = index_of_ninja;
            if (current_boss_in_play == 1)
            {
                extra_enemy_UI.do_swap(index_of_ninja, index_of_soldier);
                index_of_ninja = index_of_soldier;
                index_of_soldier = temp;
            }
            else if (current_boss_in_play == 2)
            {
                extra_enemy_UI.do_swap(index_of_ninja, index_of_wizard);
                index_of_ninja = index_of_wizard;
                index_of_wizard = temp;
            }
            //index_of_ninja = (current_boss_in_play == 1) ? index_of_soldier : index_of_wizard;
        }
        else if (old_boss_in_play == 1)
        {
            int temp = index_of_soldier;
            if (current_boss_in_play == 0)
            {
                extra_enemy_UI.do_swap(index_of_soldier, index_of_ninja);
                index_of_soldier = index_of_ninja;
                index_of_ninja = temp;
            }
            else if (current_boss_in_play == 2)
            {
                extra_enemy_UI.do_swap(index_of_soldier, index_of_wizard);
                index_of_soldier = index_of_wizard;
                index_of_wizard = temp;
            }
        }
        else if (old_boss_in_play == 2)
        {
            int temp = index_of_wizard;
            if (current_boss_in_play == 0)
            {
                extra_enemy_UI.do_swap(index_of_wizard, index_of_ninja);
                index_of_wizard = index_of_ninja;
                index_of_ninja = temp;
            }
            else if (current_boss_in_play == 1)
            {
                extra_enemy_UI.do_swap(index_of_wizard, index_of_soldier);
                index_of_wizard = index_of_soldier;
                index_of_soldier = temp;
            }
        }

        //extra_enemy_UI.do_swap(old_boss_in_play, current_boss_in_play);
        old_boss_in_play = -1;

        if (current_boss_in_play == 0)
        {
            float newX = player_life.movement.dash_time > 0 ? (player.transform.position.x + player_life.movement.dash_time * player_life.movement.dashSpeed * (player_life.movement.sprite_render.flipX ? -1 : 1)) : player.transform.position.x;
            boss_life_ninja.transform.position = new Vector2(newX, 12);
            boss_life_ninja.summon_falling_ninja();
        }
        else if (current_boss_in_play == 1)
        {
            float newX = player_life.movement.dash_time > 0 ? (player.transform.position.x + player_life.movement.dash_time * player_life.movement.dashSpeed * (player_life.movement.sprite_render.flipX ? -1 : 1)) : player.transform.position.x;
            boss_life_soldier.transform.position = new Vector2(newX, 12);
            boss_life_soldier.ani.SetBool("wind_blowing", false);
            boss_life_soldier.summon_falling_soldier();
        }
        else if (current_boss_in_play == 2)
        {
            boss_life_wizard.coming_in = true;
            boss_life_wizard.all_actions_cooldown = 5f;
            boss_life_wizard.ani.SetBool("teleporting_enter", true);
        }
    }
    public void handle_death()
    {
        var hi = Instantiate(Knockout_Text).GetComponent<Level5KnockoutText>().rect;
        hi.position = new Vector2(4.3f, 4.5f);

        current_boss_in_play = temporary_stored_swap_value;
        time_since_swap = 0;
        temporary_stored_swap_value = -1;

        if (current_boss_in_play == 0)
        {
            boss_life_ninja.GetComponent<EnemyActiveBossComponent>().isPrimary();
            boss_life_soldier.GetComponent<EnemyActiveBossComponent>().notPrimary();
            boss_life_wizard.GetComponent<EnemyActiveBossComponent>().notPrimary();
        }
        else if (current_boss_in_play == 1)
        {
            boss_life_ninja.GetComponent<EnemyActiveBossComponent>().notPrimary();
            boss_life_soldier.GetComponent<EnemyActiveBossComponent>().isPrimary();
            boss_life_wizard.GetComponent<EnemyActiveBossComponent>().notPrimary();
        }
        else if (current_boss_in_play == 2)
        {
            boss_life_ninja.GetComponent<EnemyActiveBossComponent>().notPrimary();
            boss_life_soldier.GetComponent<EnemyActiveBossComponent>().notPrimary();
            boss_life_wizard.all_actions_cooldown = 5f;
            boss_life_wizard.GetComponent<EnemyActiveBossComponent>().isPrimary();
        }
    }
    public void apply_super_burn(float f)
    {
        if (!first_burn)
        {
            var hi = Instantiate(Burn_Tip_Text).GetComponent<Level5KnockoutText>().rect;
            hi.position = new Vector2(-4.5f, 3f);
            first_burn = true;
        }
        super_burn_timer = 12f;
        delay_to_extinguish_super_burn = 3f;
        if (current_sticky_burn_effect == null)
        {
            current_sticky_burn_effect = Instantiate(sticky_burn_effect, player.transform.position, Quaternion.Euler(0,0,0)).GetComponent<StickyFireEffectBehavior>();
            current_sticky_burn_effect.manager = this;
        }
    }
    public void extinguish_super_burn()
    {
        super_burn_timer = 0f;
        super_burn_damage_timer = super_burn_damage_timer_max;
        if (current_sticky_burn_effect)
        {
            Destroy(current_sticky_burn_effect.gameObject);
        }
    }
    public void inflict_area_damage(int damage)
    {
        if (has_all_bosses)
        {
            extra_enemy_UI.area_effect_fade_cooldown = 1.25f;
            if (!boss_life_ninja.is_dead)
            {
                int damage_dealt = 0;

                /*if (current_boss_in_play != 0 && old_boss_in_play != 0)
                {
                    damage_dealt = Mathf.Min(damage, boss_life_ninja.health - 1);
                    boss_life_ninja.TakeDamage(damage_dealt);
                }
                else if (current_boss_in_play == 0 && temporary_stored_swap_value != -1)
                {
                    damage_dealt = Mathf.Min(damage, boss_life_ninja.health - 1);
                    boss_life_ninja.TakeDamage(damage_dealt);
                }*/

                if (current_boss_in_play != 0)
                {
                    damage_dealt = Mathf.Min(damage, boss_life_ninja.health - 1);
                    boss_life_ninja.TakeDamage(damage_dealt);
                }

                if (damage_dealt > 0)
                {
                    extra_enemy_UI.inflict_area_effect_effect(0);
                    if (index_of_ninja == 1)
                    {
                        second_opponent.set_visible(damage_dealt);
                    }
                    else if (index_of_ninja == 2)
                    {
                        third_opponent.set_visible(damage_dealt);
                    }
                }
            }
            if (!boss_life_soldier.is_dead)
            {
                int damage_dealt = 0;

                /*if (current_boss_in_play != 1 && old_boss_in_play != 1)
                {
                    damage_dealt = Mathf.Min(damage, boss_life_soldier.health - 1);
                    boss_life_soldier.TakeDamage(damage_dealt);
                }
                else if (current_boss_in_play == 1 && temporary_stored_swap_value != -1)
                {
                    damage_dealt = Mathf.Min(damage, boss_life_soldier.health - 1);
                    boss_life_soldier.TakeDamage(damage_dealt);
                }*/
                if (current_boss_in_play != 1)
                {
                    damage_dealt = Mathf.Min(damage, boss_life_soldier.health - 1);
                    boss_life_soldier.TakeDamage(damage_dealt);
                }

                if (damage_dealt > 0)
                {
                    extra_enemy_UI.inflict_area_effect_effect(1);
                    if (index_of_soldier == 1)
                    {
                        second_opponent.set_visible(damage_dealt);
                    }
                    else if (index_of_soldier == 2)
                    {
                        third_opponent.set_visible(damage_dealt);
                    }
                }
            }
            if (!boss_life_wizard.is_dead)
            {
                int damage_dealt = 0;

                /*if (current_boss_in_play != 2 && old_boss_in_play != 2)
                {
                    damage_dealt = Mathf.Min(damage, boss_life_wizard.health - 1);
                    boss_life_wizard.TakeDamage(damage_dealt);
                }
                else if (current_boss_in_play == 2 && temporary_stored_swap_value != -1)
                {
                    damage_dealt = Mathf.Min(damage, boss_life_wizard.health - 1);
                    boss_life_wizard.TakeDamage(damage_dealt);
                }*/

                if (current_boss_in_play != 2)
                {
                    damage_dealt = Mathf.Min(damage, boss_life_wizard.health - 1);
                    boss_life_wizard.TakeDamage(damage_dealt);
                }

                if (damage_dealt > 0)
                {
                    extra_enemy_UI.inflict_area_effect_effect(2);
                    if (index_of_wizard == 1)
                    {
                        second_opponent.set_visible(damage_dealt);
                    }
                    else if (index_of_wizard == 2)
                    {
                        third_opponent.set_visible(damage_dealt);
                    }
                }
            }
        }
    }
    public void load_next_level()
    {
        SceneManager.LoadScene("Main Menu");
    }
}
