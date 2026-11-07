using UnityEngine;

public class PlayerAttacking : MonoBehaviour
{

    private PlayerMovement playerMovement;
    private PlayerDamageAndHealth playerHealth;
    private SpriteRenderer sprite;
    private Rigidbody2D rb;
    public Transform firePoint;

    public GameObject Ice;
    public GameObject Fire;
    public GameObject Gem;
    public GameObject Earth;
    public GameObject Water;
    public GameObject Light;
    public GameObject Dark;

    public GameObject IceLesserSuper;
    public GameObject FireLesserSuper;
    public GameObject GemLesserSuper;
    public GameObject EarthLesserSuper;
    public GameObject WaterLesserSuper;
    public GameObject LightLesserSuper;
    public GameObject DarkLesserSuper;

    private GameObject selectedWeapon;

    public bool disableAttack = false;
    public bool disableAttackDebug = false;

    public float maxWeaponCooldown = 0f;
    public float weaponCooldown = 0f;

    public bool firingBeam_first_time = false;
    public enum WeaponType { Ice, Fire, Gem, Earth, Water, Light, Dark }
    /* 
     * Ice = Normal Shot
     * Fire = Instant Beam
     * Gem = Triple Shot
     * Earth = Gravity/Terrain AOE
     * Water = Rapid + Weak
     * Light = Fast + Slow Recharge
     * Dark = Lingering AOE zone upon impact
     */

    public WeaponType currentWeapon = WeaponType.Ice;
    public WeaponType lastWeapon = WeaponType.Fire;

    public GameObject current_gem_shield;
    public GemShieldBehavior current_gem_shield_behavior;
    void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerHealth = GetComponent<PlayerDamageAndHealth>();
        sprite = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }
    void Start()
    {
        loadWeapon();
        GameManager.Instance.update_weapon();
    }

    // Update is called once per frame
    void Update()
    {

        /*if (currentWeapon != lastWeapon)
        {
            lastWeapon = currentWeapon;
            switchWeapon(currentWeapon);
        }*/

        if (selectedWeapon == null)
        {
            disableAttackDebug = true;
            Debug.Log("Invalid Weapon Assets!");
        }

        else
        {
            disableAttackDebug = false;
        }

        if (weaponCooldown > 0f)
        {
            weaponCooldown -= Time.deltaTime;
            if (weaponCooldown < 0f)
            {
                weaponCooldown = 0f;
            }
        }

        disableAttack = (playerHealth.parryTime > 0f) || !playerMovement.canAttack;

        if (playerMovement.isAttacking && !(disableAttack || disableAttackDebug)) // Cannot attack while parrying!
        {
            if (currentWeapon == WeaponType.Ice)
            {
                if (weaponCooldown <= 0f)
                {
                    attackIce();
                    weaponCooldown = maxWeaponCooldown;
                }
            }
            else if (currentWeapon == WeaponType.Fire)
            {
                if (!firingBeam_first_time)
                {
                    firingBeam_first_time = true;
                    attackFire();
                }
            }
            else if (currentWeapon == WeaponType.Gem)
            {
                if (weaponCooldown <= 0f)
                {
                    attackGem();
                    weaponCooldown = maxWeaponCooldown;
                }
            }
            else if (currentWeapon == WeaponType.Light)
            {
                if (weaponCooldown <= 0f)
                {
                    attackLight();
                    weaponCooldown = maxWeaponCooldown;
                }
            }
        }

        if (firingBeam_first_time && !playerMovement.isAttacking)
        {
            fireDeactivated();
        }

        playerMovement.ani.SetInteger("weaponType", (int) currentWeapon);
    }
    private void attackIce()
    {
        GameObject shootyshootshoot = Instantiate(Ice, firePoint.position, firePoint.rotation);
        IceBehavior initiating = shootyshootshoot.GetComponent<IceBehavior>();
        initiating.Initialize(10, firePoint.eulerAngles.z, 9, gameObject, playerHealth);
        if (playerHealth.weaknessTime > 0)
        {
            initiating.playerAttack.damage = Mathf.RoundToInt(initiating.playerAttack.damage * (1 - playerHealth.weaknessPotency));
        }
    }
    private void attackFire()
    {
        GameObject fiahBeam = Instantiate(Fire, firePoint.position, firePoint.rotation);
        FireBehavior initiating = fiahBeam.GetComponent<FireBehavior>();
        initiating.Initialize(gameObject, firePoint);
    }
    public void fireDeactivated()
    {
        firingBeam_first_time = false;
    }
    public void fireAttacked()
    {
        weaponCooldown = 0.15f;
    }
    private void attackGem()
    {
        GameObject shootyshootshoot1 = Instantiate(Gem, firePoint.position, Quaternion.Euler(0, 0, firePoint.rotation.eulerAngles.z));
        GameObject shootyshootshoot2 = Instantiate(Gem, firePoint.position, Quaternion.Euler(0, 0, firePoint.rotation.eulerAngles.z + 8));
        GameObject shootyshootshoot3 = Instantiate(Gem, firePoint.position, Quaternion.Euler(0, 0, firePoint.rotation.eulerAngles.z - 8));

        GemBehavior initiating1 = shootyshootshoot1.GetComponent<GemBehavior>();
        GemBehavior initiating2 = shootyshootshoot2.GetComponent<GemBehavior>();
        GemBehavior initiating3 = shootyshootshoot3.GetComponent<GemBehavior>();

        initiating1.Initialize(10, firePoint.eulerAngles.z, 9, gameObject, playerHealth);
        initiating2.Initialize(10, firePoint.eulerAngles.z + 8, 9, gameObject, playerHealth);
        initiating3.Initialize(10, firePoint.eulerAngles.z - 8, 9, gameObject, playerHealth);
        if (playerHealth.weaknessTime > 0)
        {
            initiating1.playerAttack.damage = Mathf.RoundToInt(initiating1.playerAttack.damage * (1 - playerHealth.weaknessPotency));
            initiating2.playerAttack.damage = Mathf.RoundToInt(initiating2.playerAttack.damage * (1 - playerHealth.weaknessPotency));
            initiating3.playerAttack.damage = Mathf.RoundToInt(initiating3.playerAttack.damage * (1 - playerHealth.weaknessPotency));
        }
    }
    private void attackLight()
    {
        GameObject shootyshootshoot = Instantiate(Light, firePoint.position, firePoint.rotation);
        LightBehavior initiating = shootyshootshoot.GetComponent<LightBehavior>();
        initiating.Initialize(30, firePoint.eulerAngles.z, 9, gameObject, playerHealth);
        if (playerHealth.weaknessTime > 0)
        {
            initiating.playerAttack.damage = Mathf.RoundToInt(initiating.playerAttack.damage * (1 - playerHealth.weaknessPotency));
        }
    }
    public void switchWeapon(WeaponType newWep)
    {
        lastWeapon = currentWeapon;
        currentWeapon = newWep;
        loadWeapon();
        GameManager.Instance.update_weapon();
        weaponCooldown = maxWeaponCooldown / 2f;
    }
    public void loadWeapon()
    {
        if (currentWeapon == WeaponType.Ice)
        {
            if (Ice == null)
                Ice = Resources.Load<GameObject>("Player_Weapons/Ice");
            if (IceLesserSuper == null)
                IceLesserSuper = Resources.Load<GameObject>("Player_Weapons/Super/IceLesser");
            selectedWeapon = Ice;
            maxWeaponCooldown = 0.4f;
        }
        else if (currentWeapon == WeaponType.Fire)
        {
            if (Fire == null)
                Fire = Resources.Load<GameObject>("Player_Weapons/Fire");
            if (FireLesserSuper == null)
                FireLesserSuper = Resources.Load<GameObject>("Player_Weapons/Super/FireLesser");
            selectedWeapon = Fire;
        }
        else if (currentWeapon == WeaponType.Gem)
        {
            if (Gem == null)
                Gem = Resources.Load<GameObject>("Player_Weapons/Gem");
            if (GemLesserSuper == null)
                GemLesserSuper = Resources.Load<GameObject>("Player_Weapons/Super/GemLesser");
            selectedWeapon = Gem;
            maxWeaponCooldown = 0.4f;
        }
        else if (currentWeapon == WeaponType.Earth)
        {
            if (Earth == null)
                Earth = Resources.Load<GameObject>("Player_Weapons/Earth");
            if (EarthLesserSuper == null)
                EarthLesserSuper = Resources.Load<GameObject>("Player_Weapons/Super/EarthLesser");
            selectedWeapon = Earth;
        }
        else if (currentWeapon == WeaponType.Water)
        {
            if (Water == null)
                Water = Resources.Load<GameObject>("Player_Weapons/Water");
            if (WaterLesserSuper == null)
                WaterLesserSuper = Resources.Load<GameObject>("Player_Weapons/Super/WaterLesser");
            selectedWeapon = Water;
        }
        else if (currentWeapon == WeaponType.Light)
        {
            if (Light == null)
                Light = Resources.Load<GameObject>("Player_Weapons/Light");
            if (LightLesserSuper == null)
                LightLesserSuper = Resources.Load<GameObject>("Player_Weapons/Super/LightLesser");
            selectedWeapon = Light;
            maxWeaponCooldown = 1.6f;
        }
        else if (currentWeapon == WeaponType.Dark)
        {
            if (Dark == null)
                Dark = Resources.Load<GameObject>("Player_Weapons/Dark");
            if (DarkLesserSuper == null)
                DarkLesserSuper = Resources.Load<GameObject>("Player_Weapons/Super/DarkLesser");
            selectedWeapon = Dark;
        }
        else // How?
        {
            currentWeapon = WeaponType.Ice;
            Ice = Resources.Load<GameObject>("Player_Weapons/Ice");
            IceLesserSuper = Resources.Load<GameObject>("Player_Weapons/Super/IceLesser");
            selectedWeapon = Ice;
            maxWeaponCooldown = 0.4f;
        }
    }
    public void doSuperAttack()
    {
        if (currentWeapon == WeaponType.Ice)
        {
            super_attack_ice();
        }
        else if (currentWeapon == WeaponType.Fire)
        {
            super_attack_fire();
        }
        else if (currentWeapon == WeaponType.Gem)
        {
            super_attack_gem();
        }
        else if (currentWeapon == WeaponType.Light)
        {
            super_attack_light();
        }
        else
        {
            super_attack_ice();
        }
    }
    public void super_attack_ice()
    {
        if (playerHealth.current_burn)
        {
            Destroy(playerHealth.current_burn);
            playerHealth.burnTime = 0;
        }
        GameObject iceBG = Instantiate(Resources.Load<GameObject>("Player_Weapons/Super/IceBackground"), transform.position, Quaternion.Euler(0,0,0));
        Destroy(iceBG, 0.2f);
        CameraShake attempt = Camera.main.GetComponent<CameraShake>();
        if (attempt != null)
        {
            attempt.shakeOnHurt(0.5f);
        }

        EnemyActiveBossComponent[] potential_targets = FindObjectsByType<EnemyActiveBossComponent>(FindObjectsSortMode.None);
        GameObject target = null;
        foreach (EnemyActiveBossComponent test in potential_targets)
        {
            if (test.isPrimaryTarget)
            {
                target = test.gameObject;
                break;
            }
        }

        if (target == null)
        {
            float min_distance = 99f;
            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, 10f, LayerMask.GetMask("Enemy","Active_Enemy_Attack"));
            foreach (Collider2D hit in hits)
            {
                EnemyDamageable damageable = hit.GetComponent<EnemyDamageable>();
                if (damageable == null) continue;
                EnemyMainBodyImplement further_target = hit.GetComponent<EnemyMainBodyImplement>();
                if (further_target == null) continue;
                float distance = Vector2.Distance(hit.transform.position, transform.position);
                if (distance < min_distance)
                {
                    min_distance = distance;
                    target = hit.gameObject;
                }
            }
        }
        
        if (target != null)
        {
            if (target.transform.position.x > transform.position.x)
            {
                sprite.flipX = false;
            }
            else
            {
                sprite.flipX = true;
            }
        }

        GameObject ice_spike = Resources.Load<GameObject>("Player_Weapons/Super/IceGreater");

        GameObject ice_spike1 = (target != null) ? Instantiate(ice_spike, target.transform.position + transform.up * 20, Quaternion.Euler(0, 0, 0)) : Instantiate(ice_spike, new Vector2(transform.position.x + 5,transform.position.y + 10), Quaternion.Euler(0, 0, 0));
        GameObject ice_spike2 = (target != null) ? Instantiate(ice_spike, target.transform.position + transform.up * 20, Quaternion.Euler(0, 0, 10)) : Instantiate(ice_spike, new Vector2(transform.position.x + 5,transform.position.y + 12), Quaternion.Euler(0, 0, 0));
        GameObject ice_spike3 = (target != null) ? Instantiate(ice_spike, target.transform.position + transform.up * 20, Quaternion.Euler(0, 0, 350)) : Instantiate(ice_spike, new Vector2(transform.position.x + 5,transform.position.y + 14), Quaternion.Euler(0, 0, 0));

        ice_spike1.GetComponent<IceSuperBehavior>().Initialize(7, 0, 5, 0, target, gameObject, playerHealth);
        ice_spike2.GetComponent<IceSuperBehavior>().Initialize(7, 10, 5, 1, target, gameObject, playerHealth);
        ice_spike3.GetComponent<IceSuperBehavior>().Initialize(7, 350, 5, 2, target, gameObject, playerHealth);

        Invoke("endSuperAttack", 0.2f);
    }
    public void super_attack_fire()
    {
        GameObject fireBG = Instantiate(Resources.Load<GameObject>("Player_Weapons/Super/FireBackground"), transform.position, Quaternion.Euler(0, 0, 0));
        Destroy(fireBG, 0.2f);

        GameObject fireball = Resources.Load<GameObject>("Player_Weapons/Super/FireGreater");
        GameObject fiah = Instantiate(fireball, transform.position + transform.up * 20, Quaternion.Euler(0, 0, 0));

        fiah.GetComponent<FireSuperBehavior>().Initialize(10, 5, gameObject, playerHealth);

        Invoke("endSuperAttack", 0.2f);
    }
    public void super_attack_gem()
    {
        GameObject gemBG = Instantiate(Resources.Load<GameObject>("Player_Weapons/Super/GemBackground"), transform.position, Quaternion.Euler(0, 0, 0));
        Destroy(gemBG, 0.2f);

        Invoke("endSuperAttack", 0.2f);
    }
    public void super_attack_light()
    {
        EnemyDamageable result = null;
        EnemyActiveBossComponent[] potential_targets = FindObjectsByType<EnemyActiveBossComponent>(FindObjectsSortMode.None);
        GameObject target = null;
        foreach (EnemyActiveBossComponent test in potential_targets)
        {
            if (test.isPrimaryTarget)
            {
                target = test.gameObject;
                result = test.GetComponent<EnemyDamageable>();
                break;
            }
        }

        if (target == null)
        {
            float min_distance = 99f;
            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, 10f, LayerMask.GetMask("Enemy", "Active_Enemy_Attack"));
            foreach (Collider2D hit in hits)
            {
                EnemyDamageable damageable = hit.GetComponent<EnemyDamageable>();
                if (damageable == null) continue;
                EnemyMainBodyImplement further_target = hit.GetComponent<EnemyMainBodyImplement>();
                if (further_target == null) continue;
                float distance = Vector2.Distance(hit.transform.position, transform.position);
                if (distance < min_distance)
                {
                    min_distance = distance;
                    target = hit.gameObject;
                    result = damageable;
                }
            }
        }

        if (target != null)
        {
            Invoke("endSuperAttack", 0.2f);
            if (target.transform.position.x > transform.position.x)
            {
                sprite.flipX = false;
            }
            else
            {
                sprite.flipX = true;
            }
        }
        else
        {
            playerMovement.ani.SetBool("failed_light", true);
            Invoke("endSuperAttack", 0.2f);
            return;
        }

        GameObject lightBeam = Instantiate(Resources.Load<GameObject>("Player_Weapons/Super/LightGreater"), firePoint.transform.position, Quaternion.Euler(0, 0, 0));
        lightBeam.GetComponent<LightSuperBehavior>().initialize(firePoint.gameObject, target, result);
        //Invoke("endSuperAttack", 0.2f);
    }
    private void endSuperAttack()
    {
        if (currentWeapon == WeaponType.Gem)
        {
            GameObject gemArmor = Instantiate(Resources.Load<GameObject>("Player_Status_Effects/gem_armor"), transform.position, Quaternion.Euler(0, 0, 0));
            gemArmor.GetComponent<GemArmorBehavior>().initialize(gameObject);

            if (current_gem_shield != null && current_gem_shield_behavior != null)
            {
                current_gem_shield_behavior.lifetime = 1;
            }

            playerHealth.add_invincibility_time(9f);
            current_gem_shield = Instantiate(Resources.Load<GameObject>("Player_Status_Effects/gem_shield"), transform.position, Quaternion.Euler(0, 0, 0));

            current_gem_shield_behavior = current_gem_shield.GetComponent<GemShieldBehavior>();
            current_gem_shield_behavior.initialize(9f, gameObject);
        }

        playerMovement.ani.SetBool("failed_light", false);
        playerMovement.end_super_attacking();
    }
    public void doLesserSuperAttack()
    {
        if (currentWeapon == WeaponType.Ice)
        {
            GameObject shootyshootshoot = Instantiate(IceLesserSuper, firePoint.position, firePoint.rotation);
            LesserSuperIceBehavior initiating = shootyshootshoot.GetComponent<LesserSuperIceBehavior>();
            initiating.Initialize(10, firePoint.eulerAngles.z, 15, gameObject, playerHealth);
        }
        else if (currentWeapon == WeaponType.Fire)
        {
            GameObject shootyshootshoot = Instantiate(FireLesserSuper, firePoint.position, firePoint.rotation);
            LesserSuperFireBehavior initiating = shootyshootshoot.GetComponent<LesserSuperFireBehavior>();
            initiating.Initialize(10, firePoint.eulerAngles.z, 10, gameObject, playerHealth);
        }
        else if (currentWeapon == WeaponType.Gem)
        {
            EnemyActiveBossComponent[] potential_targets = FindObjectsByType<EnemyActiveBossComponent>(FindObjectsSortMode.None);
            GameObject target = null;
            foreach (EnemyActiveBossComponent test in potential_targets)
            {
                if (test.isPrimaryTarget)
                {
                    target = test.gameObject;
                    break;
                }
            }

            if (target == null)
            {
                float min_distance = 99f;
                Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, 8f, LayerMask.GetMask("Enemy", "Active_Enemy_Attack"));
                foreach (Collider2D hit in hits)
                {
                    EnemyDamageable damageable = hit.GetComponent<EnemyDamageable>();
                    if (damageable == null) continue;
                    EnemyMainBodyImplement further_target = hit.GetComponent<EnemyMainBodyImplement>();
                    if (further_target == null) continue;
                    float distance = Vector2.Distance(hit.transform.position, transform.position);
                    if (distance < min_distance)
                    {
                        min_distance = distance;
                        target = hit.gameObject;
                    }
                }
            }

            if (target == null)
            {
                return;
            }
            else
            {
                if (target.transform.position.x > transform.position.x)
                {
                    sprite.flipX = false;
                }
                else
                {
                    sprite.flipX = true;
                }
            }

            GameObject gem_spiky = Instantiate(GemLesserSuper, target.transform.position, Quaternion.Euler(0, 0, 0));
            LesserSuperGemBehavior initiating = gem_spiky.GetComponent<LesserSuperGemBehavior>();
            initiating.initialize(3, gameObject, playerHealth);
        }
        else if (currentWeapon == WeaponType.Light)
        {
            GameObject lightBG = Instantiate(Resources.Load<GameObject>("Player_Weapons/Super/LightBackground"), transform.position, Quaternion.Euler(0, 0, 0));
            //Destroy(lightBG, 0.2f);
        }
        else
        {

        }
    }
    public void endLesserSuperAttack()
    {
        playerMovement.rb.bodyType = RigidbodyType2D.Dynamic;
        playerMovement.ani.SetBool("isLesserSuperAttacking", false);
    }
}
