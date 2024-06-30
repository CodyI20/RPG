using System.Collections;
using UnityEngine;

public class SpiritWolfAttackVisuals : MonoBehaviour
{
    //This script will be changed later when the ability system is in place. For now this is used for testing purposes.
    [SerializeField] private float erodeRate = 0.03f;
    [SerializeField] private float erodeRefreshRate = 0.1f;
    [SerializeField, Tooltip("The time the ability will be alive for")] private float erodeDelay = 1.25f;


    [SerializeField] private SkinnedMeshRenderer[] _meshRenderers;

    void Start()
    {
        StartCoroutine(ErodeObject());
    }

    IEnumerator ErodeObject()
    {
        yield return new WaitForSeconds(erodeDelay);
        float t = 0;
        while (t < 1)
        {
            t += erodeRate;
            foreach (var _meshRenderer in _meshRenderers)
            {
                foreach (var material in _meshRenderer.materials)
                {
                    material.SetFloat("_Erode", t);
                }
            }
            yield return new WaitForSeconds(erodeRefreshRate);
        }
        Destroy(gameObject);
    }

}

