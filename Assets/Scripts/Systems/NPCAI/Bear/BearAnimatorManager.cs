using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearAnimatorManager : NPCAnimationManager
{
    private readonly string[] _attackTrigger = { "Attack1", "Attack2", "Attack3", "Attack5" };

    private string GetRandomAttackTrigger()
    {
        return _attackTrigger[Random.Range(0, _attackTrigger.Length)];
    }

    protected override void HandleAttackAnimation() 
    {
        SetBool("Run Forward", false);
        SetBool("Idle", false);
        SetTrigger(GetRandomAttackTrigger());
    }

    protected override void HandleDeathAnimation()
    {
        SetBool("Run Forward", false);
        SetBool("Idle", false);
        SetBool("Death",true);
    }

    protected override void HandleRunAnimation()
    {
        SetBool("Idle", false);
        SetBool("Run Forward", true);
    }

    protected override void HandleIdleAnimation()
    {
        SetBool("Run Forward", false);
    }
}
