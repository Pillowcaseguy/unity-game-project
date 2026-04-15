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
     * 
     */

    public WeaponType currentWeapon = WeaponType.Ice;
    public WeaponType lastWeapon = WeaponType.Fire;
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
                maxWeaponCooldown = 0.4f;
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
                maxWeaponCooldown = 0.4f;
                if (weaponCooldown <= 0f)
                {
                    attackGem();
                    weaponCooldown = maxWeaponCooldown;
                }
            }
            else if (currentWeapon == WeaponType.Light)
            {
                maxWeaponCooldown = 1.6f;
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

    }
    private void attackIce()
    {
        GameObject shootyshootshoot = Instantiate(Ice, firePoint.position, firePoint.rotation);
        IceBehavior initiating = shootyshootshoot.GetComponent<IceBehavior>();
        initiating.Initialize(10, firePoint.eulerAngles.z, 9, gameObject);
        if (playerHealth.weaknessTime > 0)
        {
            initiating.playerAttack.damage = Mathf.CeilToInt(initiating.playerAttack.damage * (1 - playerHealth.weaknessPotency));
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
        GameObject shootyshootshoot1 = Instantiate(Gem, firePoint.position, Quaternion.Euler(0,0,firePoint.rotation.eulerAngles.z));
        GameObject shootyshootshoot2 = Instantiate(Gem, firePoint.position, Quaternion.Euler(0, 0, firePoint.rotation.eulerAngles.z + 8));
        GameObject shootyshootshoot3 = Instantiate(Gem, firePoint.position, Quaternion.Euler(0, 0, firePoint.rotation.eulerAngles.z - 8));

        GemBehavior initiating1 = shootyshootshoot1.GetComponent<GemBehavior>();
        GemBehavior initiating2 = shootyshootshoot2.GetComponent<GemBehavior>();
        GemBehavior initiating3 = shootyshootshoot3.GetComponent<GemBehavior>();

        initiating1.Initialize(10, firePoint.eulerAngles.z, 9, gameObject);
        initiating2.Initialize(10, firePoint.eulerAngles.z + 8, 9, gameObject);
        initiating3.Initialize(10, firePoint.eulerAngles.z - 8, 9, gameObject);
        if (playerHealth.weaknessTime > 0)
        {
            initiating1.playerAttack.damage = Mathf.CeilToInt(initiating1.playerAttack.damage * (1 - playerHealth.weaknessPotency));
            initiating2.playerAttack.damage = Mathf.CeilToInt(initiating2.playerAttack.damage * (1 - playerHealth.weaknessPotency));
            initiating3.playerAttack.damage = Mathf.CeilToInt(initiating3.playerAttack.damage * (1 - playerHealth.weaknessPotency));
        }
    }
    private void attackLight()
    {
        GameObject shootyshootshoot = Instantiate(Light, firePoint.position, firePoint.rotation);
        LightBehavior initiating = shootyshootshoot.GetComponent<LightBehavior>();
        initiating.Initialize(30, firePoint.eulerAngles.z, 9, gameObject);
        if (playerHealth.weaknessTime > 0)
        {
            initiating.playerAttack.damage = Mathf.CeilToInt(initiating.playerAttack.damage * (1 - playerHealth.weaknessPotency));
        }
    }
    public void switchWeapon(WeaponType newWep)
    {
        lastWeapon = currentWeapon;
        currentWeapon = newWep;
        loadWeapon();
        GameManager.Instance.update_weapon();
    }
    public void loadWeapon()
    {
        if (currentWeapon == WeaponType.Ice)
        {
            if (Ice == null)
                Ice = Resources.Load<GameObject>("Player_Weapons/Ice");
            selectedWeapon = Ice;
        }
        else if (currentWeapon == WeaponType.Fire)
        {
            if (Fire == null)
                Fire = Resources.Load<GameObject>("Player_Weapons/Fire");
            selectedWeapon = Fire;
        }
        else if (currentWeapon == WeaponType.Gem)
        {
            if (Gem == null)
                Gem = Resources.Load<GameObject>("Player_Weapons/Gem");
            selectedWeapon = Gem;
        }
        else if (currentWeapon == WeaponType.Earth)
        {
            if (Earth == null)
                Earth = Resources.Load<GameObject>("Player_Weapons/Earth");
            selectedWeapon = Earth;
        }
        else if (currentWeapon == WeaponType.Water)
        {
            if (Water == null)
                Water = Resources.Load<GameObject>("Player_Weapons/Water");
            selectedWeapon = Water;
        }
        else if (currentWeapon == WeaponType.Light)
        {
            if (Light == null)
                Light = Resources.Load<GameObject>("Player_Weapons/Light");
            selectedWeapon = Light;
        }
        else if (currentWeapon == WeaponType.Dark)
        {
            if (Dark == null)
                Dark = Resources.Load<GameObject>("Player_Weapons/Dark");
            selectedWeapon = Dark;
        }
        else // How?
        {
            currentWeapon = WeaponType.Ice;
            Ice = Resources.Load<GameObject>("Player_Weapons/Ice");
            selectedWeapon = Ice;
        }
    }
}
