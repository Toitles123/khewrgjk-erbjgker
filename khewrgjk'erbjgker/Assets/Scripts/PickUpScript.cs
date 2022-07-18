using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PickUpScript : MonoBehaviour
{

    [Header("Pickup Settings")]

    [SerializeField] float maxPickUpDistance;
    [SerializeField] float pickUpDistance;
    [SerializeField] LayerMask layerMask;
    [SerializeField] Transform target;

    [Header("Graphical Settings")]

    [SerializeField] TextMeshProUGUI pickupText;
    [SerializeField] Color pickupOutlineColor;
    [SerializeField] Color selectionOutlineColor;
    [SerializeField] float outlineWidth;

    GameObject pickedUpObject;
    GameObject oldPickedUpObject;
    
    Outline outline;

    [Header("Other Settings")]
    [SerializeField]InventoryScript inventoryScript;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, maxPickUpDistance, layerMask))
        {
            if (pickedUpObject != oldPickedUpObject || pickedUpObject == null)
            {
                pickedUpObject = hit.collider.gameObject;
                if (outline == null)
                {
                    outline = pickedUpObject.AddComponent<Outline>();
                    outline.OutlineMode = Outline.Mode.OutlineVisible;
                    outline.OutlineColor = selectionOutlineColor;
                    outline.OutlineWidth = outlineWidth;
                }

                if (pickedUpObject.GetComponent<GrabbableScript>() != null && pickedUpObject.GetComponent<GrabbableScript>().carryable)
                {
                    pickupText.text = "[E] Pickup " + pickedUpObject.GetComponent<GrabbableScript>().item.name;
                    pickupText.gameObject.SetActive(true);
                }
                else
                {
                    pickupText.gameObject.SetActive(false);
                }

                oldPickedUpObject = pickedUpObject;
            }
            if (Input.GetKeyDown(KeyCode.E) && pickedUpObject.GetComponent<GrabbableScript>() != null && pickedUpObject.GetComponent<GrabbableScript>().carryable)
            {
                bool foundItem = false;
                GrabbableScript grabbableScript = pickedUpObject.GetComponent<GrabbableScript>();
                foreach (InventoryScript.InventoryItem item in inventoryScript.inventory)
                {
                    if (item.id == pickedUpObject.GetComponent<GrabbableScript>().item.id)
                    {
                        foundItem = true;
                        item.count++;
                        break;
                    }
                }
                if (!foundItem)
                {
                    foreach (InventoryScript.InventoryItem item in inventoryScript.inventory)
                    {
                        if (item.id == "")
                        {
                            item.id = grabbableScript.item.id;
                            item.count = 1;
                            item.gameObject = grabbableScript.item.gameObject;
                            item.maxStack = grabbableScript.item.maxStack;
                            item.UIScale = grabbableScript.item.UIScale;
                            item.UIPosition = grabbableScript.item.UIPosition;
                            item.UIRotation = grabbableScript.item.UIRotation;
                            break;
                        }
                    }
                }
                Destroy(pickedUpObject);
                pickedUpObject = null;
                pickupText.gameObject.SetActive(false);
            }
            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                pickupText.gameObject.SetActive(false);
                pickedUpObject.GetComponent<Rigidbody>().drag = 2.5f;
                pickedUpObject.GetComponent<Outline>().OutlineColor = pickupOutlineColor;
                target.position = hit.point;
                pickedUpObject.layer = 6;
            }
            if (Input.GetKeyUp(KeyCode.Mouse1))
            {
                Destroy(pickedUpObject.GetComponent<Outline>());
                pickedUpObject.GetComponent<Rigidbody>().useGravity = true;
                pickedUpObject.GetComponent<Rigidbody>().drag = 0;
                pickedUpObject.layer = 8;
                pickedUpObject = null;
            }
        }
        else
        {
            if (!Input.GetKey(KeyCode.Mouse1))
            {
                if (pickedUpObject != null)
                {
                    pickupText.gameObject.SetActive(false);
                    Destroy(pickedUpObject.GetComponent<Outline>());
                    pickedUpObject.GetComponent<Rigidbody>().useGravity = true;
                    pickedUpObject.GetComponent<Rigidbody>().drag = 0;
                    pickedUpObject.layer = 8;
                    pickedUpObject = null;
                }
            }
        }
        if (pickedUpObject != null)
        {
            float dist = Vector3.Distance(target.position, Camera.main.transform.position);
            if (Input.mouseScrollDelta.y != 0)
            {
                dist = Mathf.Clamp(dist + 0.25f * Input.mouseScrollDelta.y, 1.5f, 5);
                target.localPosition = new Vector3(target.localPosition.x, target.localPosition.y, dist);
            }
        }

        if (pickedUpObject != null && Input.GetKey(KeyCode.Mouse1))
        {
            Rigidbody rb = pickedUpObject.GetComponent<Rigidbody>();
            rb.useGravity = false;
            rb.AddForce((target.position - pickedUpObject.transform.position).normalized * 1000 * Vector3.Distance(pickedUpObject.transform.position, target.position) * Time.deltaTime, ForceMode.Force);
        }
    }
}
