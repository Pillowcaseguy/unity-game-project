using System.Collections.Generic;
using UnityEngine;

public class TestBoss1Behavior : MonoBehaviour, EnemyDamageable
{
    public GameObject throwing_point;
    public GameObject grenade;
    public GameObject sad_bomb;
    public GameObject sword_wave;

    public ShootBlast shotgun;
    public BlockingSword bs;
    public SwordSlash ss;
    public Kick k;

    private Animator ani;

    public bool threwGrenades = false;
    public float attackTime;
    public bool phase_change = false;
    public bool phase_changed = false;
    public bool is_dead;

    public bool is_attacking;
    public bool is_slashing;
    public bool is_drawing;
    public bool is_throwing;
    public bool is_shooting;
    public bool is_kicking;

    public int health;
    public int starting_health = 1800;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        shotgun = GetComponentInChildren<ShootBlast>();
        bs = GetComponentInChildren<BlockingSword>();
        ss = GetComponentInChildren<SwordSlash>();
        ani = GetComponent<Animator>();
        health = starting_health;
        k = GetComponentInChildren<Kick>();
    }

    // Update is called once per frame
    private void Update()
    {

        if (health <= 0)
        {
            is_dead = true;
        }

        if (is_dead)
        {
            ani.SetBool("isDead", is_dead);
            return;
        }

        attackTime += Time.deltaTime;

        if (attackTime > 5.0f && !is_attacking)
        {
            do_attack();
        }

        if (!is_attacking && phase_change && !phase_changed && attackTime > 0.5f)
        {
            is_attacking = true;
            is_throwing = true;
            phase_changed = true;
            phase_change = false;
            Invoke("throw_three",0.8f);
        }

        ani.SetBool("isSlashing", is_slashing);
        ani.SetBool("isDrawing", is_drawing);
        ani.SetBool("isThrowing", is_throwing);
        ani.SetBool("isShooting", is_shooting);
        ani.SetBool("isKicking", is_kicking);

    }
    public void throw_something()
    {

        if (threwGrenades)
        {
            throw_sad_bomb(UnityEngine.Random.Range(1, 4));
            threwGrenades = false;
        }
        else
        {
            throw_grenade();
            threwGrenades = true;
        }
    }
    public void throw_grenade(int type = 0)
    {
        GameObject gwenade = Instantiate(grenade, throwing_point.transform.position, transform.rotation);
        GrenadeBehavior boom = gwenade.GetComponent<GrenadeBehavior>();
        if (type == 0)
        {
            type = UnityEngine.Random.Range(1, 5);
            if (k.player_in_range)
            {
                type = 4;
            }
        }
        if (type == 1)
        {
            boom.Initialize(-100f, 200f);
        }
        else if (type == 2)
        {
            boom.Initialize(-220f, 200f);
        }
        else if (type == 3)
        {
            boom.Initialize(-150f, 200f);
        }
        else if (type == 4)
        {
            boom.Initialize(0f, 0f);
        }
        else
        {
            boom.Initialize(-200f, 200f);
        }
    }
    public void throw_sad_bomb(int times = 1)
    {
        if (times < 1)
        {
            times = 1;
        }

        else if (times > 3)
        {
            times = 3;
        }

        float bomb_life;

        if (times == 1)
        {
            bomb_life = 9f;
        }
        else if (times == 2)
        {
            bomb_life = 14f;
        }
        else if (times == 3)
        {
            bomb_life = 20f;
        }
        else
        {
            bomb_life = 10f;
        }

        List<int> positions = new List<int> { 1, 2, 3 };
        for (int x = 2; x > 0; x--)
        {
            int swap_index = Random.Range(0, x + 1);
            int temp = positions[x];
            positions[x] = positions[swap_index];
            positions[swap_index] = temp;
        }

        for (int x = 0; x < times; x++)
        {
            GameObject woof = Instantiate(sad_bomb, throwing_point.transform.position, transform.rotation);
            SadBombBehavior boom = woof.GetComponent<SadBombBehavior>();
            boom.initialize(positions[x],bomb_life);
        }
    }

    public void Hit(PlayerAttack attack)
    {
        if (is_dead)
        {
            return;
        }
        attack.isHit();
        TakeDamage(attack.damage);
    }
    public void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log("Current health: " + health);
        if (health <= starting_health / 2 && !phase_changed)
        {
            phase_change = true;
        }
    }
    public void do_attack()
    {
        is_attacking = true;
        int attack = UnityEngine.Random.Range(0, 3);
        if (attack == 0)
        {
            is_drawing = true;
            Invoke("start_slash", 2f);
        }
        else if (attack == 1)
        {
            is_shooting = true;
        }
        else if (attack == 2)
        {
            is_throwing = true;
        }
    }
    public void start_slash()
    {
        is_drawing = false;
        is_slashing = true;
    }
    public void end_attack()
    {
        is_slashing = false;
        is_throwing = false;
        is_shooting = false;
        is_attacking = false;
        attackTime = 0f;
        if (is_kicking)
        {
            k.attack.disableAttackCycle = false;
        }
        is_kicking = false;
    }
    public void talk_to_blocking_sword(int yes)
    {
        if (yes == 1)
        {
            bs.enable();
        }

        else
        {
            bs.disable();
        }

    }
    public void talk_to_slashing_sword(int yes)
    {
        if (yes == 1)
        {
            ss.enable();
        }

        else
        {
            ss.disable();
        }
    }
    public void do_sword_wave()
    {
        if (ss.attack.disableAttackCycle)
        {
            ss.attack.disableAttackCycle = false;
            return;
        }
        GameObject woosh = Instantiate(sword_wave, new Vector2(ss.transform.position.x, ss.transform.position.y + 0.2f), transform.rotation);
        sword_wave_behavior swash = woosh.GetComponent<sword_wave_behavior>();
        swash.Initialize(10,180f,5);
    }
    public void talk_to_kick(int yes)
    {
        if (yes == 1)
        {
            k.enable();
        }

        else
        {
            k.disable();
        }
    }
    public void throw_three() // Four lol
    {
        throw_grenade(1);
        throw_grenade(2);
        throw_grenade(3);
        throw_grenade(4);
    }
    public void check_kick_range()
    {
        if (k.player_in_range == true)
        {
            is_shooting = false;
            is_kicking = true;
        }
    }
    public void shoot()
    {
        shotgun.shoot();
    }
}
