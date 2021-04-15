using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
* Still needs work
*/

public class Carryable : MonoBehaviour, IInteractable
{
    bool carried;

    Transform targetTrans;
    float offset;

    PlayerInteraction target;

    Rigidbody rb;
    GravityObject gb;

    bool spherePhys;

    public float damp = 5f;

    //cleanup obj to destroy carryables across the scene
    private static Transform m_DestroyOnLoad;


    //maybe temp
    SceneObject sceneObj;

    //outlines
    public bool activeOutline = true;
    public Color lineOnHover = Color.red;
    public float lineThickOnHover = 2f;

    private Material myMat;

    private Color initalCol;
    private float initalFloat;

    void Awake()
    {
        gb = GetComponent<GravityObject>();
        rb = GetComponent<Rigidbody>();

        sceneObj = GetComponent<SceneObject>();
   
        SceneData data = GameObject.Find("SceneData").GetComponent<SceneData>();
        if (data.planet)
        {
            gb.planet = data.planet;
            gb.enabled = true;
            rb.useGravity = false;
            //little bit temp
            rb.freezeRotation = true;

            spherePhys = true;
        }
        else
        {
            gb.enabled = false;
            rb.useGravity = true;

            spherePhys = false;
        }

        //backup layer set
        this.gameObject.layer = 8;
        myMat = GetComponentInChildren<MeshRenderer>().material;
        if (activeOutline)
        {
            initalCol = myMat.GetColor("Color_70BF2FCC");
            initalFloat = myMat.GetFloat("Vector1_F5D76E9B");
        }
    }

    public void Interact(PlayerMaster player)
    {
        if (carried)
        {
            Drop();
        }
        else
        {
            target = player.GetPlayerInteraction();
            
            carried = true;
            target.carrying = true;

            if (spherePhys)
            {            
               //temp, this is also not quite right it would be nice for object to keep rotating
                gb.enabled = false;
            }
            else
            {
                rb.useGravity = false;
            }
        
            targetTrans = player.GetPlayerCameraTransform();
            offset = Vector3.Distance(transform.position, targetTrans.position);

            rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
            rb.velocity = Vector3.zero;
            rb.freezeRotation = true;
            
            sceneObj.SetTransform();
        }
    }

    public void OnHoverEnter()
    {
        if (activeOutline)
        {
            myMat.SetColor("Color_70BF2FCC", lineOnHover);
            myMat.SetFloat("Vector1_F5D76E9B", lineThickOnHover);
        }
    }

    public void OnHover(){}

    public void OnHoverExit()
    {
        if (activeOutline)
        {
            myMat.SetColor("Color_70BF2FCC", initalCol);
            myMat.SetFloat("Vector1_F5D76E9B", initalFloat);
        }
    }

    void Update()
    {
        if (carried)
        {
            if (Vector3.Distance(targetTrans.position + targetTrans.forward * offset, transform.position) > 0.7f)
            {
                Drop();
            }
        }
    }

    void FixedUpdate()
    {
        //Smooth centering
        if (carried)
        {
            rb.MovePosition(Vector3.Lerp(transform.position, targetTrans.position + targetTrans.forward * offset, Time.fixedDeltaTime * damp));
        }
    }

    void Drop()
    {
        rb.collisionDetectionMode = CollisionDetectionMode.Discrete;

        carried = false;
        target.carrying = false;

        if (spherePhys)
        {
            gb.enabled = true;
        }
        else
        {
            rb.useGravity = true;
            rb.freezeRotation = false;
        }

        //DestroyOnLoad(this.gameObject);
    }

    public static void DestroyOnLoad(GameObject obj)
    {
        if (m_DestroyOnLoad == null)
            m_DestroyOnLoad = (new GameObject("DestroyOnLoad")).transform;
        obj.transform.parent = m_DestroyOnLoad;
    }
}

