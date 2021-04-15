using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderMove : MonoBehaviour
{
    public float speed = 1f;

    private Rigidbody rb;
    private Animator anim;
    private GravityObject gravObj;

    private StateMachine fsm;

    private float boundsAdjust = 0;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        anim.SetFloat("Speed", speed);

        gravObj = GetComponent<GravityObject>();
        gravObj.planet = FindObjectOfType<SceneData>().planet;
        Collider coll = GetComponent<Collider>();
        gravObj.SetCorners(
            new Vector3[4]{
                new Vector3(coll.bounds.extents.x - boundsAdjust, 0, 0),
                new Vector3(0, 0, coll.bounds.extents.z - boundsAdjust),
                new Vector3(-coll.bounds.extents.x + boundsAdjust, 0, 0),
                new Vector3(0, 0, -coll.bounds.extents.z + boundsAdjust)
            }
        );
    }

    // Update is called once per frame
    void Update()
    {
        rb.MovePosition(rb.position + transform.forward * speed * Time.deltaTime);
    }
}
