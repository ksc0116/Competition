using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingHook : MonoBehaviour
{
    public CheckGround checkGround;
    public GameObject hook;
    public GameObject hookHolder;

    public float hookTravelSpeed;
    public float playerTravelSpeed;

    public static bool fired;
    public  bool hooked;
    public GameObject hookedObj;

    public float maxDistance;
    float currentDistance;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.LeftShift) && fired == false)
            fired = true;

        if (fired == true)
        {
            LineRenderer rope=hook.GetComponent<LineRenderer>();
            rope.positionCount = 2;
            rope.SetPosition(0,hookHolder.transform.position);
            rope.SetPosition(1, hook.transform.position);
        }

        if (fired == true && hooked==false)
        {
            Ray ray = Camera.main.ViewportPointToRay(Vector2.one * 0.5f);
            hook.transform.Translate(ray.direction*Time.deltaTime*hookTravelSpeed);
            currentDistance=Vector3.Distance(transform.position,hook.transform.position);

            if (currentDistance >= maxDistance)
                ReturnHook();
        }

        if (hooked == true && fired==true)
        {
            hook.transform.parent=hookedObj.transform;
            transform.position = Vector3.MoveTowards(transform.position, hook.transform.position, playerTravelSpeed * Time.deltaTime);
            float distanceToHook = Vector3.Distance(transform.position, hook.transform.position);

            this.GetComponent<Rigidbody>().useGravity = false;

            if (distanceToHook < 1)
            {
                if (checkGround.isGround == false)
                {
                    this.transform.Translate(Vector3.forward * Time.deltaTime * 13f);
                    this.transform.Translate(Vector3.up * Time.deltaTime * 18f);
                }
                StartCoroutine(Climb());
            }
        }
        else
        {
            hook.transform.parent = hookHolder.transform;
            this.GetComponent<Rigidbody>().useGravity = true;
        }
    }

    private IEnumerator Climb()
    {
        yield return new WaitForSeconds(0.1f);
        ReturnHook();
    }

    private void ReturnHook()
    {
        hook.transform.rotation=hookHolder.transform.rotation;
        hook.transform.position = hookHolder.transform.position;
        fired = false;
        hooked = false;

        LineRenderer rope = hook.GetComponent<LineRenderer>();
        rope.positionCount=0;
    }
}
