using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpScript : MonoBehaviour
{

    [SerializeField] float maxPickUpDistance;
    [SerializeField] float pickUpDistance;
    [SerializeField] LayerMask layerMask;
    [SerializeField] GameObject pickedUpObject;
    [SerializeField] Transform target;

    [SerializeField] Color pickupOutlineColor;
    [SerializeField] Color selectionOutlineColor;
    [SerializeField] float outlineWidth;

    Outline outline;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, maxPickUpDistance, layerMask))
        {
            if (hit.collider.gameObject.CompareTag("Grabbable"))
            {
                pickedUpObject = hit.collider.gameObject;
                if (outline == null)
                {
                    outline = pickedUpObject.AddComponent<Outline>();
                    outline.OutlineMode = Outline.Mode.OutlineVisible;
                    outline.OutlineColor = selectionOutlineColor;
                    outline.OutlineWidth = outlineWidth;
                }
                if (Input.GetKey(KeyCode.Mouse1))
                {
                    pickedUpObject.GetComponent<Rigidbody>().drag = 2.5f;
                    pickedUpObject.GetComponent<Outline>().OutlineColor = pickupOutlineColor;
                    target.position = hit.point;
                    pickedUpObject.layer = 6;
                }
            }
        }
        else
        {
            if (pickedUpObject != null && !Input.GetKey(KeyCode.Mouse1))
            {
                Destroy(pickedUpObject.GetComponent<Outline>());
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
            if (Input.GetKeyUp(KeyCode.Mouse1))
            {
                Destroy(pickedUpObject.GetComponent<Outline>());
                pickedUpObject.GetComponent<Rigidbody>().useGravity = true;
                pickedUpObject.GetComponent<Rigidbody>().drag = 0;
                pickedUpObject.layer = 0;
                pickedUpObject = null;
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
