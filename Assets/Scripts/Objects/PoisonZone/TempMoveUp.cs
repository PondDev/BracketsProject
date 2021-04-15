using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempMoveUp : MonoBehaviour
{
    public float initSpeed = 3f;
    public float decayTime = 1f;
    public float speed = 0.4f;
    public float life = 5f;
    public float zipOutSpeed = 0.05f;

    private bool dying = false;
    private float curSpeed;
    private float timer = 0;

    // Update is called once per frame
    void Start()
    {
        curSpeed = initSpeed;
    }

    void Update()
    {
        if (!dying)
        {
            if (timer < life)
            {
                if (timer < decayTime)
                {
                    float fraction = timer/decayTime;
                    fraction = 1 - fraction;
                    curSpeed = fraction * (initSpeed - speed) + speed;
                }
                else
                {
                    curSpeed = speed;
                }
                timer += Time.deltaTime;
            }
            else
            {
                dying = true;
            }
        }
        else
        {
            transform.localScale = transform.localScale - new Vector3(zipOutSpeed,zipOutSpeed,zipOutSpeed);
            if (transform.localScale.magnitude <= 0.1f)
            {
                Destroy(gameObject);
            }
        }
        transform.position = transform.position + transform.up * curSpeed * Time.deltaTime;
    }
}
