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
        public GameObject projectile;
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
    public Renderer gunVial;

    private float fireRateCounter;
    private PlayerInputHandler inputHandler;
    private float maxCharge;
    [SerializeField]
    private float currentCharge;
    private bool Depleted;
    
    private void Start()
    {
        inputHandler = GetComponent<PlayerInputHandler>();

        maxCharge = GameController.gunMaxCharge;

        SetCharge(maxCharge);
    }

    private void Update()
    {
        fireRateCounter += Time.deltaTime;

        if(fireRateCounter > weaponProperties.fireRate) {
            if(inputHandler.GetFireInput()) {
                Shoot();
            }
        }

        Recharge();
    }

    public void Shoot()
    {
        if(Depleted)
            return;

        if(currentCharge <= 0)
            return;

        MuzzleFlashVFX();
        SpawnProjectile();

        fireRateCounter = 0f;

        Deplete();
    }

    public void Deplete() {
        if(currentCharge <= 0)
            return;

        SetCharge(currentCharge - GameController.gunDepleteRate);
    }

    public void Recharge() {
        if(currentCharge >= maxCharge)
            return;

        float charge = currentCharge + GameController.gunRechargeRate;

        if(Depleted)
            charge *= 3;

        SetCharge(currentCharge + GameController.gunRechargeRate);
    }

    private void SetCharge(float charge) {
        charge = Mathf.Clamp(charge, 0, maxCharge);

        currentCharge = charge;

        if(currentCharge == maxCharge && Depleted)
            Depleted = false;
        else
            if(currentCharge == 0)
                Depleted = true;

        UpdateGunMaterial();
    }

    private void UpdateGunMaterial() {
        float vialPercentage = currentCharge / maxCharge;

        float vialValue = Mathf.Lerp(0.546f, 0.45f, vialPercentage);

        gunVial.material.SetFloat("_FillAmount", vialValue);
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
        Projectile projectile = Instantiate(weaponProperties.projectile, weaponProperties.weaponBarrel.position, Quaternion.identity, null).GetComponent<Projectile>();
        projectile.movementDirection = weaponProperties.weaponBarrel.transform.forward;
        projectile.speed = weaponProperties.projectileSpeed;
        projectile.lifetime = weaponProperties.projectileLifetime;
        projectile.SetMass(weaponProperties.projectileMass);
    }
    
    public static GameController GameController { get { return GameController.Instance; } }
}
