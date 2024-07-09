using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityUtils;

public class GameObjectSpawner : Singleton<GameObjectSpawner>
{
    public void SpawnObjectAfterDelay(GameObject @object, Vector3 position, Quaternion rotation, float delay)
    {
        if (@object != null) // Check if the prefab is still valid
        {
            StartCoroutine(SpawnObjectAfterDelayCoroutine(@object, position, rotation, delay));
        }
    }

    IEnumerator SpawnObjectAfterDelayCoroutine(GameObject @object, Vector3 position, Quaternion rotation, float delay)
    {
        yield return new WaitForSeconds(delay);
        Instantiate(@object, position, rotation);
    }
}
