using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DanceSpider : MonoBehaviour
{
    Animator anim;
    bool dying = false;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        
    }
    void Update()
    {
        if (dying)
        {
            transform.position = transform.position - transform.forward * 2f * Time.deltaTime;
            transform.localScale = transform.localScale  - new Vector3(0.02f, 0.02f, 0.02f);
            if (transform.localScale.magnitude <= 0.1f)
            {
                Destroy(gameObject);
            }
        }
    }

    // Update is called once per frame
    public void Prepare()
    {
        anim.Play("Raise");
    }

    public void Dance()
    {
        anim.Play("Dance");
    }
    public void Leave()
    {
        dying = true;
        anim.Play("Run");
    }
}
