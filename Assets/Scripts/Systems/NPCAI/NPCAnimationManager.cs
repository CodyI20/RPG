using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[DisallowMultipleComponent]
public class NPCAnimationManager : MonoBehaviour
{
    Animator _animator;

    protected virtual void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    protected void SetBool(string name, bool value)
    {
        _animator.SetBool(name, value);
    }

    protected void SetFloat(string name, float value)
    {
        _animator.SetFloat(name, value);
    }

    protected void SetInteger(string name, int value)
    {
        _animator.SetInteger(name, value);
    }

    protected void SetTrigger(string name)
    {
        _animator.SetTrigger(name);
    }
}
