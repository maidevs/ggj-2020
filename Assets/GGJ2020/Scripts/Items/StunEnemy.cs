using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunEnemy : AItemEffect
{
    public override void ApplyItemEffect()
    {
        enemy.stunMultiplier = 2.5f;

        enemy.SetStun();
    }

    public override void RemoveItemEffect()
    {
        if (enemy != null)
            enemy.stunMultiplier = 1f;
    }
}
