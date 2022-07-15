using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject inGameHealthbar;
    public GameObject player;
    public Camera UICamera;

    public int playerReach = 3;
    public LayerMask harvestableLayer;

    public HarvestableScript selectedHarvestableScript;

    public float renderDistance;

    [HideInInspector]
    public List<HarvestableScript> allHarvestableObjects;


    void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, playerReach, harvestableLayer))
        {
            if (hit.collider.gameObject.TryGetComponent<HarvestableScript>(out HarvestableScript harvestableScript))
            {
                selectedHarvestableScript = harvestableScript;
                selectedHarvestableScript.EnterLook();
            }
        }

        foreach (HarvestableScript harvestableScript in allHarvestableObjects)
        {
            if (Vector3.Distance(player.transform.position, harvestableScript.transform.position) <= renderDistance) harvestableScript.gameObject.SetActive(true); else harvestableScript.gameObject.SetActive(false);
        }

    }
}
