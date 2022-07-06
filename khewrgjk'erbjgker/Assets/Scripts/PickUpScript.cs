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

    [SerializeField] Color outlineColor;
    [SerializeField] float outlineWidth;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (pickedUpObject == null)
        {
            if (Input.GetKey(KeyCode.Mouse1))
            {
                RaycastHit hit;
                if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, maxPickUpDistance, layerMask))
                {
                    if (hit.collider.gameObject.CompareTag("Grabbable"))
                    {
                        pickedUpObject = hit.collider.gameObject;
                        pickedUpObject.GetComponent<Rigidbody>().drag = 2.5f;
                        target.position = hit.point;
                        Outline outline = pickedUpObject.AddComponent<Outline>();
                        outline.OutlineMode = Outline.Mode.OutlineVisible;
                        outline.OutlineColor = outlineColor;
                        outline.OutlineWidth = outlineWidth;
                        pickedUpObject.layer = 6;
                    }
                }
            }
        }
        else
        {
            float dist = Vector3.Distance(target.position, Camera.main.transform.position);
            if (Input.mouseScrollDelta.y != 0)
            {
                dist = Mathf.Clamp(dist + 0.25f * Input.mouseScrollDelta.y, 1.5f, 5);
                target.localPosition = new Vector3(target.localPosition.x, target.localPosition.y, dist);
            }
            if (Input.GetKeyUp(KeyCode.Mouse1))
            {
                if (pickedUpObject != null)
                {
                    Destroy(pickedUpObject.GetComponent<Outline>());
                    pickedUpObject.GetComponent<Rigidbody>().useGravity = true;
                    pickedUpObject.GetComponent<Rigidbody>().drag = 0;
                    pickedUpObject.layer = 0;
                    pickedUpObject = null;
                }
            }
        }

        if (pickedUpObject != null)
        {
            Rigidbody rb = pickedUpObject.GetComponent<Rigidbody>();
            rb.useGravity = false;
            rb.AddForce((target.position - pickedUpObject.transform.position).normalized * 1000 * Vector3.Distance(pickedUpObject.transform.position, target.position) * Time.deltaTime, ForceMode.Force);
        }
    }
}
