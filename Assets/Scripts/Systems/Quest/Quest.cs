using UnityEngine;

[CreateAssetMenu(fileName = "New Quest", menuName = "Quest System/Quest")]
public class Quest : ScriptableObject
{
    [HideInInspector] public int questHash; // To avoid spelling mistakes
    public string questName;
    [Tooltip("This field should explain to the player exactly what to do in order to complete the quest")] public string objective;
    [TextArea(1,20),Tooltip("This field is for story purposes and can contain anything related to the quest")] public string description;

    public float experienceReward;
    public float goldReward;
    public GameObject itemReward = null; // Change to Item class later when the Inventory system is implemented

    private void OnValidate()
    {
        questName = this.name;
        UnityEditor.EditorUtility.SetDirty(this);
        questHash = questName.GetHashCode();
    }
}
