using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class PlanetSelectorPlanet : MonoBehaviour, IInteractable
{
    //TEMP
    public PlayableDirector director;

    public static float switchTime = 1f;

    StateMachine fsm;

    private Vector3 stillOffset;
    private Vector3 spinningOffset;

    Vector3 curOffset;
    Vector3 targetOffset;

    float startTime;

    private bool fromStill;

    public float rotateSpeed = 30f;

    public bool interactable = false;

    public int levelIndex;

    public Color lineOnHover = Color.red;
    public float lineThickOnHover = 2f;

    private Material myMat;

    private Color initalCol;
    private float initalFloat;

    public GameObject planetInfo;

    string destinationState;

    void Start()
    {
        //backup layer set
        if (interactable)
        {
            //backup layer set
            this.gameObject.layer = 8;
            myMat = GetComponent<MeshRenderer>().material;
            initalCol = myMat.GetColor("Color_70BF2FCC");
            initalFloat = myMat.GetFloat("Vector1_F5D76E9B");
            planetInfo.SetActive(false);
        }

        Dictionary<string, State> states = new Dictionary<string, State>();
        states.Add("Still", new State(
            new State.StateAction(StillEnter),
            new State.StateAction(StillUpdate),
            new State.StateAction(StillExit)
            )
        );
        states.Add("Spinning", new State(
            new State.StateAction(SpinningEnter),
            new State.StateAction(SpinningUpdate),
            new State.StateAction(SpinningExit)
            )
        );
        states.Add("Switching", new State(
            new State.StateAction(SwitchingEnter),
            new State.StateAction(SwitchingUpdate),
            new State.StateAction(SwitchingExit)
            )
        );

        stillOffset = Vector3.zero;
        spinningOffset = transform.localPosition;
        transform.localPosition = Vector3.zero;

        fsm = new StateMachine(states, "Still");
    }
    protected virtual void StillEnter() {}
    protected virtual void StillUpdate() {}
    protected virtual void StillExit()
    {
        fromStill = true;
        curOffset = stillOffset;
    }

    protected virtual void SpinningEnter() {}
    protected virtual void SpinningUpdate()
    {
        //spin
        transform.RotateAround(transform.parent.transform.position, Vector3.up, rotateSpeed * Time.deltaTime);
    }
    protected virtual void SpinningExit()
    {
        fromStill = false;
        curOffset = transform.localPosition;
        transform.rotation = Quaternion.identity;
    }

    protected virtual void SwitchingEnter()
    {
        startTime = Time.time;
        if (fromStill)
        {
            targetOffset = spinningOffset;
        }
        else
        {
            targetOffset = stillOffset;
        }
    }
    protected virtual void SwitchingUpdate()
    {
        if (Time.time - startTime < switchTime)
        {
            float fracComplete = (Time.time - startTime) / switchTime;
            transform.localPosition = Vector3.Slerp(curOffset, targetOffset, fracComplete);
        }
        else
        {
            if (fromStill)
            {
                fsm.ChangeState("Spinning");
            }
            else
            {
                fsm.ChangeState("Still");
            }
        }
    }
    protected virtual void SwitchingExit() {}


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            Toggle();
        }
        fsm.Update();


        //if dest state exists
        if (destinationState != null)
        {   
            //and we aren't switching
            if (fsm.currentStateStr != "Switching")
            {
                //and our current state isn't our destination
                if (fsm.currentStateStr != destinationState)
                {
                    fsm.ChangeState("Switching");
                }
                destinationState = null;
            }
        }
    }

    public bool Toggle()
    {
        if (fsm.currentStateStr != "Switching")
        {
            fsm.ChangeState("Switching");
            return true;
        }
        else
        {
            return false;
        }
    }

    public void PlanetsOn()
    {
        if (fsm.currentStateStr == "Still")
        {
            fsm.ChangeState("Switching");
        }
        else
        {
            destinationState = "Spinning";
        }
    }

    public void PlanetsOff()
    {
        if (fsm.currentStateStr == "Spinning")
        {
            fsm.ChangeState("Switching");
        }
        else
        {
            destinationState = "Still";
        }
    }

    public void Interact(PlayerMaster player)
    {        
        if (interactable)
        {
            CutSceneLoadLink.instance.StartCutScene(levelIndex, player);
        }
    }

    public void OnHoverEnter()
    {
        if (interactable)
        {
            myMat.SetColor("Color_70BF2FCC", lineOnHover);
            myMat.SetFloat("Vector1_F5D76E9B", lineThickOnHover);
            planetInfo.SetActive(true);
        }
    }

    public void OnHover()
    {
    }

    public void OnHoverExit() 
    {
        if (interactable)
        {
            myMat.SetColor("Color_70BF2FCC", initalCol);
            myMat.SetFloat("Vector1_F5D76E9B", initalFloat);
            //catch if next level loads too quickly
            if (planetInfo != null)
            {
                planetInfo.SetActive(false);
            }
        }
    }
}
