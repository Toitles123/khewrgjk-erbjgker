using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTools : MonoBehaviour
{
    [System.Serializable]
    public class Tool
    {
        public string name;
        public GameObject toolObject;
        public float toolSpeed;
    }

    [SerializeField] Tool[] tools;

    Tool equippedTool;
    float timer;

    // Start is called before the first frame update
    void Start()
    {
        equippedTool = tools[0];
    }

    // Update is called once per frame
    void Update()
    {
        foreach (Tool tool in tools)
        {
            if (tool == equippedTool)
            {
                tool.toolObject.SetActive(true);
            }
            else
            {
                tool.toolObject.SetActive(false);
            }
        }

        timer -= Time.deltaTime;
        if (Input.GetMouseButton(0))
        {
            if (timer <= 0 && !InventoryScript.inventoryOpen)
            {
                equippedTool.toolObject.GetComponent<Animator>().SetTrigger("Use");
                timer = equippedTool.toolSpeed;
            }
        }
    }
}
