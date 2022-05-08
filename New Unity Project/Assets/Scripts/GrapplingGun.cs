using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingGun : MonoBehaviour
{
    private LineRenderer lr;
    private Vector3 grapplePoint;
    public LayerMask whatIsgrappleable;
    public Transform grapPos;
    public Transform player;
    private float maxDistance = 50f;
    private SpringJoint joint;

    private float cognitionRange = 2.5f;
    private void Awake()
    {
        lr = GetComponent<LineRenderer>();
    }
    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            StartGrapple();
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            StopGrapple();
        }
    }
    private void LateUpdate()
    {
        DrawRope();
    }
    private void StartGrapple()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ViewportPointToRay(Vector2.one * 0.5f);
        if (Physics.SphereCast(ray, cognitionRange, out hit, maxDistance,whatIsgrappleable))
        {
            grapplePoint = hit.point;
            joint=player.gameObject.AddComponent<SpringJoint>();
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = grapplePoint;

            float distanceFromPoint = Vector3.Distance(player.position, grapplePoint);

            joint.maxDistance = distanceFromPoint*0.001f;
            joint.minDistance = distanceFromPoint * 0.0005f;

            joint.spring = 7f;
            joint.damper = 7f;
            joint.massScale = 4.5f;

            lr.positionCount = 2;
        }
    }

    void DrawRope()
    {
        if (!joint) return;
        lr.SetPosition(0, grapPos.position);
        lr.SetPosition(1, grapplePoint);
    }

    private void StopGrapple()
    {
        lr.positionCount = 0;
        Destroy(joint);
    }
}
