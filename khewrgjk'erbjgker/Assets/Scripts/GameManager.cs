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

    void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
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
    }
}
