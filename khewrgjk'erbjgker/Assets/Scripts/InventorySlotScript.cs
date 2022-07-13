using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventorySlotScript : MonoBehaviour
{
    public Item item;
    public int count;
    [SerializeField] TextMeshProUGUI countText;
    [SerializeField] Transform displayAnchor;
    GameObject displayObject;
    Item oldItem;
    // Start is called before the first frame update
    void Start()
    {
        item = null;
    }

    // Update is called once per frame
    void Update()
    {
        if (count != 0)
        {
            countText.text = "x" + count.ToString();
        }
        else
        {
            countText.text = "";
        }
        if (item.id == "" || oldItem != item)
        {
            if (item.id != "" && item.gameObject != null)
            {
                if (displayObject == null)
                {
                    displayObject = Instantiate(item.gameObject, displayAnchor.position, Quaternion.identity);
                    displayObject.transform.SetParent(displayAnchor.transform);
                    displayObject.transform.localEulerAngles = item.UIRotation;
                    displayObject.transform.position += item.UIPosition;
                    displayObject.transform.localScale = item.UIScale;
                }
            }
            else
            {
                Destroy(displayObject);
                displayObject = null;
                count = 0;
            }
            oldItem = item;
        }
    }
}
