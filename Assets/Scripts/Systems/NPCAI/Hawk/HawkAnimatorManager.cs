using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HawkAnimatorManager : NPCAnimationManager
{
    protected override void HandleAttackAnimation()
    {
        SetBool("Running", false);
        SetBool("Idle", false);
        SetTrigger("Attack");
    }

    protected override void HandleDeathAnimation()
    {
        SetBool("Running", false);
        SetBool("Idle", false);
        SetBool("Dying", true);
    }

    protected override void HandleIdleAnimation()
    {
        SetBool("Running", false);
    }

    protected override void HandleRunAnimation()
    {
        SetBool("Idle", false);
        SetBool("Running", true);
    }
}
