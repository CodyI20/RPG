using UnityEngine;

public class PressTQuest : QuestLogic
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            CompleteQuest(this);
        }
    }
}
