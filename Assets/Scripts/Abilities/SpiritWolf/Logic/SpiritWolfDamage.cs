using System.Collections.Generic;
using UnityEngine;

public class SpiritWolfDamage : MonoBehaviour
{
    [SerializeField] private int damage = 120;
    [SerializeField] private float damageRadius = 3f;

    List<NPCStats> enemiesDamaged = new List<NPCStats>();

    private void Start()
    {
        enemiesDamaged.Clear();
    }

    private void Update()
    {
        FindAndDamageEnemiesAround();
    }

    private void FindAndDamageEnemiesAround()
    {
        // Get all colliders within the damage radius
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, damageRadius);

        // Loop through all colliders
        foreach (Collider collider in hitColliders)
        {
            // Check if the collider's gameObject is on the "HostileNPC" layer
            if (collider.gameObject.layer == LayerMask.NameToLayer("HostileNPC"))
            {
                // Get the NPCStats component
                NPCStats npcStats = collider.GetComponent<NPCStats>();

                // If the NPCStats component exists, deal damage
                if (npcStats != null && !enemiesDamaged.Contains(npcStats))
                {
                    npcStats.TakeDamage(damage);
                    enemiesDamaged.Add(npcStats);
                }
            }
        }
    }

    // This method is for visualizing the damage area in the editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, damageRadius);
    }
}
