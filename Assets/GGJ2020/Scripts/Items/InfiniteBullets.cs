using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfiniteBullets : AItemEffect
{
    private PlayerWeapon playerWeapon;
    
    public override void ApplyItemEffect()
    {
        playerWeapon = player.GetComponent<PlayerWeapon>();
        playerWeapon.HasInfiniteBullets = true;
    }

    public override void RemoveItemEffect()
    {
        playerWeapon.HasInfiniteBullets = false;
    }
}
