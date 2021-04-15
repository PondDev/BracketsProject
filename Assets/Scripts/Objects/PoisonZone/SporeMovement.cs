using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SporeMovement : MonoBehaviour
{
    public float speed = 1f;
    public float rotSpeed = 1f;
    public AnimationCurve verticalWobble;

    private float vertWobble;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0,rotSpeed,0, Space.Self);
        Vector3 dir = Quaternion.Euler(0,0,verticalWobble.Evaluate(Time.time)) * transform.forward;
        transform.position = transform.position + dir * speed * Time.deltaTime;
    }
}
