using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private Transform cameraArm;
    [SerializeField]
    private Transform playerBody;


    [SerializeField]
    private LayerMask layerMask;
    [SerializeField]
    private float minDistance = 0.5f;
    [SerializeField]
    private float maxDistance = 15f;
    [SerializeField]
    private float finalDistance;
    private Vector3 rayVector;
    private float mouseWheelSpeed = 500f;
    [SerializeField]
    private float zoomDistance;

    [SerializeField]
    private Transform aimPos;

    [SerializeField]
    private Transform cameraArmOriginPos;

    public bool isOnShake { set; get; } 
    private void Awake()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        finalDistance=Vector3.Distance(transform.position, cameraArm.position);

        zoomDistance = maxDistance / 2.0f;
    }

    private void Update()
    {
        LookAround();
        Zoom();
    }
    private void LookAround()
    {
        if (isOnShake == true) return;
        
        Vector2 mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        Vector3 camAngle = cameraArm.rotation.eulerAngles;
        float x = camAngle.x - mouseDelta.y;
        if (x < 180)
        {
            x = Mathf.Clamp(x, -1f, 70f);
        }
        else
        {
            x = Mathf.Clamp(x,310f, 361f);
        }

        cameraArm.rotation = Quaternion.Euler(x, camAngle.y + mouseDelta.x, camAngle.z);
    }
    private void Zoom()
    {
        zoomDistance -= Input.GetAxis("Mouse ScrollWheel") * mouseWheelSpeed * Time.deltaTime;
        zoomDistance = Mathf.Clamp(zoomDistance, minDistance, maxDistance);
    }
    private void LateUpdate()
    {
        if (isOnShake == true) return;

        RaycastHit hit;
        rayVector = transform.position - cameraArm.position;
        Debug.DrawRay(cameraArm.position, rayVector.normalized * finalDistance, Color.black);
        if (Physics.Raycast(cameraArm.position, rayVector, out hit, layerMask))
        {
            finalDistance = Mathf.Clamp(hit.distance, minDistance, zoomDistance);

        }
        else
        {
            finalDistance = zoomDistance;
        }

        transform.position = transform.rotation * new Vector3(0, 0, -finalDistance) + cameraArm.position;
        //transform.position = Vector3.Lerp(transform.position, transform.rotation * new Vector3(0, 0, -finalDistance) + cameraArm.position, 0.1f);
    }
}
