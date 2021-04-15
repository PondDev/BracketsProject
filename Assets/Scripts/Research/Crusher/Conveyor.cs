using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conveyor : MonoBehaviour
{

    public Crusher crusher;
    public Vector3 direction;
    public float speed;

    public bool on = false;
    List<Rigidbody> bodies = new List<Rigidbody>();

    private void OnTriggerEnter(Collider other)
    {
        bodies.Add(other.GetComponent<Rigidbody>());
    }
    private void OnTriggerExit(Collider other)
    {
        bodies.Remove(other.GetComponent<Rigidbody>());
        //ping crusher
        crusher.CrusherOff();
    }

    private void Update()
    {
        if (on)
        {
            foreach(Rigidbody body in bodies)
            {
                //temp
                if (body != null)
                {
                    body.MovePosition(body.position + direction * speed * Time.deltaTime);
                }
            } 
        }
    }
}
