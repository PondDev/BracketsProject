using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DanceSphere : MonoBehaviour, IInteractable
{

    public Color lineOnHover = Color.red;
    public float lineThickOnHover = 2f;

    private Material myMat;

    private Color initalCol;
    private float initalFloat;

    bool on = false;
    float timer = 4f;
    float rotSpeed = 0f;
    float rotThreshold = 10f;

    public Transform innerSpider;
    public GameObject dancersHolder;
    AudioSource sound;
    DanceSpider[] spiders;
    bool dying = false;
    bool ran = false;
    // Start is called before the first frame update
    void Start()
    {
        sound = GetComponent<AudioSource>();
        this.gameObject.layer = 8;
        myMat = GetComponent<MeshRenderer>().material;
        initalCol = myMat.GetColor("Color_70BF2FCC");
        initalFloat = myMat.GetFloat("Vector1_F5D76E9B");
        spiders = dancersHolder.GetComponentsInChildren<DanceSpider>();
        
    }

    void Update()
    {
        if (on)
        {
            //lift
            if (timer > 0)
            {
                transform.position = transform.position + transform.up * 0.2f * Time.deltaTime;
                timer -= Time.deltaTime;
            }

            //rotation
            if (rotSpeed < rotThreshold)
            {
                rotSpeed += Time.deltaTime;
                innerSpider.Rotate(Vector3.up, rotSpeed);
            }
            else
            {
                sound.Play();
                ran = true;
                foreach (DanceSpider spider in spiders)
                {
                    spider.Dance();
                }
                on = false;
            }
        }

        if (ran)
        {
            if (sound.isPlaying)
            {
                float temp = Mathf.PingPong(Time.time * 3, 1.5f);
                transform.localScale = new Vector3(temp,temp,temp);
            }
            else
            {
                End();
            }
        }

        if (dying)
        {
            transform.localScale = transform.localScale  - new Vector3(0.05f, 0.05f, 0.05f);
            if (transform.localScale.magnitude <= 0.1f)
            {
                Destroy(gameObject);
            }
        }
    }

    void End()
    {
        foreach (DanceSpider spider in spiders)
        {
            spider.Leave();
        }
        dying = true;
        on = false;
    }


    public void Interact(PlayerMaster player)
    {
        on = true;
        foreach (DanceSpider spider in spiders)
        {
            spider.Prepare();
        }        
    }

    public void OnHoverEnter()
    {
        myMat.SetColor("Color_70BF2FCC", lineOnHover);
        myMat.SetFloat("Vector1_F5D76E9B", lineThickOnHover);
    }

    public void OnHover(){}

    public void OnHoverExit()
    {
        myMat.SetColor("Color_70BF2FCC", initalCol);
        myMat.SetFloat("Vector1_F5D76E9B", initalFloat);
    }
}
