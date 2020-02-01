﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

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

    private float fireRateCounter;

    private void Update()
    {
        fireRateCounter += Time.deltaTime;

        if (Input.GetButtonDown("Fire1"))
        {
            //Camera.main.DOShakePosition(weaponProperties.cameraShakeDuration, weaponProperties.cameraShakeForce);
            weaponProperties.weaponTransform.DOLocalMoveZ(weaponProperties.weaponTransform.localPosition.z - weaponProperties.gunKickAmount, weaponProperties.gunKickDuration);
            Shoot();
        }
        else if (Input.GetButton("Fire1") && fireRateCounter > weaponProperties.fireRate)
        {
            Shoot();
        }
        else if (Input.GetButtonUp("Fire1"))
        {
            weaponProperties.weaponTransform.DOLocalMoveZ(weaponProperties.weaponTransform.localPosition.z + weaponProperties.gunKickAmount, weaponProperties.gunKickDuration);
        }
    }

    public void Shoot()
    {
        MuzzleFlashVFX();
        SpawnProjectile();

        fireRateCounter = 0f;
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
        projectile.movementDirection = -weaponProperties.weaponBarrel.transform.forward;
        projectile.speed = weaponProperties.projectileSpeed;
        projectile.lifetime = weaponProperties.projectileLifetime;
        projectile.SetMass(weaponProperties.projectileMass);
    }

}