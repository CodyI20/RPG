using UnityEngine;

public class PressPQuest : QuestLogic
{
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            CompleteQuest(this);
        }
    }
}
