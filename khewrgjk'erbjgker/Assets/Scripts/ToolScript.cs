using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolScript : MonoBehaviour
{
    public void DealDamage(int damage)
    {
        if (GameManager.instance.selectedHarvestableScript != null)
        {
            GameManager.instance.selectedHarvestableScript.health -= damage;
        }
    }
}
