using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class GravityObject : MonoBehaviour
{
    public enum CornerSettings
    {
        Center,
        Corners,
        Manual
    }
    public CornerSettings cornerSettings;

    public GravitySource planet;

    private Rigidbody rb;   

    public bool snapToNormal = false;
    public bool checkGrounded = false;
    [System.NonSerialized]
    public bool grounded = false;

    private Collider coll;
    private float distToGround;

    private Vector3[] corners;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        coll = GetComponent<Collider>();
        distToGround = coll.bounds.extents.y;

        switch(cornerSettings)
        {
            case CornerSettings.Center:
                corners = new Vector3[1]{Vector3.zero};
                break;
            case CornerSettings.Corners:
                corners = new Vector3[4]{
                    new Vector3(coll.bounds.extents.x, 0, -coll.bounds.extents.z),
                    new Vector3(coll.bounds.extents.x, 0, coll.bounds.extents.z),
                    new Vector3(-coll.bounds.extents.x, 0, coll.bounds.extents.z),
                    new Vector3(-coll.bounds.extents.x, 0, -coll.bounds.extents.z)
                };
                break;
            case CornerSettings.Manual:
                break;
        }

        rb.useGravity = false;
        //rb.freezeRotation = true;
    }

    void Update()
    {
        foreach(Vector3 corner in corners)
        {
            Debug.DrawRay(corner, -transform.up, Color.red);
        }
        if (checkGrounded)
        {
            grounded = IsGrounded();
        }
    }

    void FixedUpdate()
    {
        ApplyGravity(planet.gravity, planet.transform.position);
    }

    public void SetCorners(Vector3[] newCorners)
    {
        corners = newCorners;
    }

    void ApplyGravity(float gravity, Vector3 planetPos)
    {
        Vector3 normalDir = (transform.position - planetPos).normalized;
        Vector3 localUp = transform.up;

        if (snapToNormal)
        {
            Vector3 avgNormal = Vector3.zero;
            int normalCounter = 0;
            foreach(Vector3 corner in corners)
            {
                RaycastHit hit;
                //assuming 10 is better than Mathf.Infinity, just has to be a suitably big number
                if (Physics.Raycast(transform.position + (transform.rotation * corner), -normalDir, out hit, 10f, LayerMask.GetMask("Planet")))
                {
                    avgNormal += hit.normal;
                    normalCounter++;
                }
            }
            if (normalCounter > 0)
            {
                Vector3 upDir = (avgNormal/normalCounter).normalized;
                transform.rotation = Quaternion.FromToRotation(localUp, upDir) * transform.rotation;
            }
            else
            {
                transform.rotation = Quaternion.FromToRotation(localUp, normalDir) * transform.rotation;
            }
        }
        else
        {
            //any physics object needs this disabled
            //for now physics objects have rb rotation disabled so they don't spin like crazy
            transform.rotation = Quaternion.FromToRotation(localUp, normalDir) * transform.rotation;
        }

        if (grounded)
        {
            //local velocity is zero
            Vector3 localVelocity = transform.InverseTransformDirection(rb.velocity);
            localVelocity =  new Vector3(0, localVelocity.y, 0);
            rb.velocity = transform.TransformDirection(localVelocity);
        }
        else
        {
            rb.AddForce(normalDir * gravity);
        }
    }

    public bool IsGrounded()
    {
        bool anyTrue = false;
        foreach(Vector3 corner in corners)
        {
            Debug.DrawRay(transform.position + (transform.rotation * corner), -transform.up * (distToGround + 0.1f), Color.green, Time.deltaTime);
            int layers =~ LayerMask.GetMask("NonGround");
            if (Physics.Raycast(transform.position + (transform.rotation * corner), -transform.up, distToGround + 0.1f, layers))
            {
                anyTrue = true;
            }
        }
        return anyTrue;
    }
}
