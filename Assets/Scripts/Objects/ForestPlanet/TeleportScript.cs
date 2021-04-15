using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportScript : MonoBehaviour
{
    public Vector3[] teleportPositions = new Vector3[5];
    public float portTime = 2f;
    public GameObject particles;

    float timer;
    bool porting = false;
    Collider target;

    // Start is called before the first frame update
    void Start()
    {
        teleportPositions[0] = new Vector3(-3, 21, 1);
        teleportPositions[1] = new Vector3(18 , 9, -16);
        teleportPositions[2] = new Vector3(-13 , 17, 8);
        teleportPositions[3] = new Vector3(-5, -9, 28);
        teleportPositions[4] = new Vector3(14, -10, -15);
    }

    private void OnTriggerEnter(Collider other)
    {
        porting = true;
        timer = portTime;
        target = other;

        particles.SetActive(true);
    }

    void Update()
    {
        if (porting)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                //particles off
                particles.SetActive(false);
                target.transform.position = teleportPositions[Random.Range(0,teleportPositions.Length)];
                porting = false;
            }
        }
    }
}
