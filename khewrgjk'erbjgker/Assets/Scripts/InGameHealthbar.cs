using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InGameHealthbar : MonoBehaviour
{
    public Image healthbarImage;
    public TextMeshProUGUI healthbarText;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Canvas>().worldCamera = GameManager.instance.UICamera;
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(new Vector3(GameManager.instance.player.transform.position.x, transform.position.y, GameManager.instance.player.transform.position.z));
        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y + 180, transform.localEulerAngles.z);
    }
}
