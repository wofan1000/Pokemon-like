using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grabbable : MonoBehaviour
{

    [SerializeField] 
    Transform grabPoint;

    [SerializeField]
    private Transform rayPoint;

    [SerializeField]
    private float rayDis;

    private GameObject grabbedObj;

    private int layerIndex;

    // Start is called before the first frame update
    void Start()
    {
        layerIndex = LayerMask.NameToLayer("Grabbable");
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit2D hitinfo = Physics2D.Raycast(rayPoint.position, transform.right, rayDis);

        if(hitinfo.collider != null && hitinfo.collider.gameObject.layer == layerIndex) 
        {
                // grab
                if(Input.GetKeyDown(KeyCode.Z))
            {
                grabbedObj= hitinfo.collider.gameObject;
                grabbedObj.GetComponent<Rigidbody2D>().isKinematic= true;
                grabbedObj.transform.position= grabPoint.position;
                grabbedObj.transform.SetParent(transform);
            }
        }
    }
}
