using UnityEngine;

[CreateAssetMenu(fileName = "AbilityData", menuName = "ScriptableObjects/AbilityData", order = 0)]
public class AbilityData : ScriptableObject
{
    public AnimationClip animationClip;
    public AbilityEventRaiser eventRaiser;
    [HideInInspector] public int animationHash;
    public float cooldown;
    public Sprite icon;
    public string fullName;


    void OnValidate()
    {
        animationHash = Animator.StringToHash(animationClip.name);
    }
}
