using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    [System.Serializable]
    public struct WeaponProperties
    {
        [Header("Positioning Properties")]
        public Transform weaponTransform;
        public Transform weaponBarrel;

        [Space]

        [Header("Weapon properties")]
        public float fireRate;

        [Space]

        [Header("Projectile Properties")]
        public ParticleSystem projectile;
        public ParticleSystem underWaterprojectile;
        public float projectileSpeed;
        public float projectileLifetime;
        public float projectileMass;

        [Space]

        [Header("VFX Properties")]
        public GameObject muzzleFlash;
        public float cameraShakeForce;
        public float cameraShakeDuration;
        public float gunKickAmount;
        public float gunKickDuration;
    }

    public WeaponProperties weaponProperties;
    public Renderer[] gunVials;
    public Renderer[] gunCords;

    private float fireRateCounter;
    private PlayerInputHandler inputHandler;
    private PlayerController playerController;
    public float maxCharge;
    [SerializeField]
    public float currentCharge;
    private bool Depleted;

    public bool HasInfiniteBullets;

    private Color myColor;

    private void Start()
    {
        playerController = GetComponent<PlayerController>();
        inputHandler = GetComponent<PlayerInputHandler>();

        maxCharge = GameController.gunMaxCharge;

        SetCharge(maxCharge);
    }

    private void Update()
    {
        fireRateCounter += Time.deltaTime;

        if (fireRateCounter > weaponProperties.fireRate)
        {
            if (inputHandler.GetFireInput())
            {
                Shoot();
                playerController.SetAnimatorBool("shooting", true);
            }
            else
                playerController.SetAnimatorBool("shooting", false);
        }

        Recharge();
    }

    public void Shoot()
    {
        if (Depleted)
            return;

        if (currentCharge <= 0)
            return;

        MuzzleFlashVFX();
        SpawnProjectile();

        fireRateCounter = 0f;

        if (!HasInfiniteBullets)
            Deplete();
    }

    public void Deplete()
    {
        if (currentCharge <= 0)
            return;

        SetCharge(currentCharge - GameController.gunDepleteRate);
    }

    public void Recharge()
    {
        if (currentCharge >= maxCharge)
            return;

        float charge = currentCharge + GameController.gunRechargeRate;

        if(Depleted) {
            charge *= 3;

            playerController.SetAnimatorTrigger("reload");
        }

        SetCharge(currentCharge + GameController.gunRechargeRate);
    }

    private void SetCharge(float charge)
    {
        charge = Mathf.Clamp(charge, 0, maxCharge);

        currentCharge = charge;

        if (currentCharge == maxCharge && Depleted)
            Depleted = false;
        else
            if (currentCharge == 0)
            Depleted = true;

        UpdateGunMaterial();
    }

    public void SetColor(Color color) {

        myColor = color;



        foreach(Renderer gunVial in gunVials) {

            gunVial.material.SetColor("_Tint", color);

            gunVial.material.SetColor("_TopColor", color * 1.25f);

            gunVial.material.SetColor("_FoamColor", color * 2);
        }


        foreach(Renderer gunCord in gunCords) {
            gunCord.materials[2].SetColor("_Color", color);
        }



        ParticleSystem.MainModule main;



        foreach(ParticleSystem system in weaponProperties.projectile.gameObject.GetComponentsInChildren<ParticleSystem>(true)) {

            main = system.main;



            main.startColor = color * 2;

        }

    }
    private bool isUnderWater;
    public void SetUnderWater(bool isUnderWater) {
        this.isUnderWater = isUnderWater;
    }

    private void UpdateGunMaterial() {

        float vialPercentage = currentCharge / maxCharge;



        float vialValue = Mathf.Lerp(0.546f, 0.45f, vialPercentage);



        foreach(Renderer gunVial in gunVials) {
            gunVial.material.SetFloat("_FillAmount", vialValue);
        }

    }

    private void MuzzleFlashVFX()
    {
        if (weaponProperties.muzzleFlash == null)
            return;

        GameObject muzzleFlashEffect = Instantiate(weaponProperties.muzzleFlash, weaponProperties.weaponBarrel.position, Quaternion.identity);
        Destroy(muzzleFlashEffect, 0.5f);
    }

    private void SpawnProjectile()
    {

        Projectile projectile;
        if(!isUnderWater) {
            projectile = Instantiate(weaponProperties.projectile, weaponProperties.weaponBarrel.position, Quaternion.identity, null).GetComponent<Projectile>();
            projectile.movementDirection = weaponProperties.weaponBarrel.transform.forward;
            projectile.speed = weaponProperties.projectileSpeed;
            projectile.lifetime = weaponProperties.projectileLifetime;
            projectile.SetMass(weaponProperties.projectileMass);
            projectile.SetColor(myColor);
        } else {

            projectile = Instantiate(weaponProperties.underWaterprojectile, weaponProperties.weaponBarrel.position, Quaternion.identity, null).GetComponent<Projectile>();
            projectile.transform.SetParent(weaponProperties.weaponBarrel.transform);


            projectile.transform.localEulerAngles = Vector3.zero;
            projectile.transform.SetParent(null);
            projectile.speed = 0;
        }
    }

    

    public static GameController GameController { get { return GameController.Instance; } }
}
