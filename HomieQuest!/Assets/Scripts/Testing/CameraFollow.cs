using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] GameObject target;
    [SerializeField] float smoothSpeed = 0.125f;
    [SerializeField] Vector3 offset;

    // Update is called once per frame
    void FixedUpdate()
    {
        //Vector3 vel = new Vector3(target.GetComponent<Rigidbody2D>().velocity.x, .y);
        //Vector3 desiredPosition = target.transform.position + offset;
        //Vector3 smoothedPosition = Vector3.SmoothDamp(transform.position, desiredPosition, , smoothSpeed);
        //transform.position = smoothedPosition;
    }
}
