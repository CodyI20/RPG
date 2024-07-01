using System.Collections.Generic;
using UnityEngine;

public static class CustomInputManager
{
    private static Dictionary<string, KeyCode> keyMappings = new Dictionary<string, KeyCode>();

    public static void SetKey(string action, KeyCode key)
    {
        keyMappings[action] = key;
    }

    public static bool GetKey(string action)
    {
        return keyMappings.ContainsKey(action) && Input.GetKey(keyMappings[action]);
    }

    public static bool GetKeyDown(string action)
    {
        return keyMappings.ContainsKey(action) && Input.GetKeyDown(keyMappings[action]);
    }

    public static bool GetKeyUp(string action)
    {
        return keyMappings.ContainsKey(action) && Input.GetKeyUp(keyMappings[action]);
    }
}
