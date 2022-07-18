using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InGameHealthbar : MonoBehaviour
{
    public Image healthbarImage;
    public TextMeshProUGUI healthbarText;
    public Vector3 rotationOffset;
    
    [HideInInspector]
    public HarvestableScript harvestableScript;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Canvas>().worldCamera = GameManager.instance.UICamera;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.LookAt(new Vector3(GameManager.instance.player.transform.position.x, transform.position.y, GameManager.instance.player.transform.position.z));
        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x + rotationOffset.x, transform.localEulerAngles.y + rotationOffset.y, transform.localEulerAngles.z + rotationOffset.z);
        healthbarImage.fillAmount = (float)harvestableScript.health / (float)harvestableScript.maxHealth;
        healthbarText.text = harvestableScript.health + " / " + harvestableScript.maxHealth;
        if (harvestableScript.health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
