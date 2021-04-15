using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Giant spide class
public class MoveBetweenPoints : MonoBehaviour
{
    public List<Vector3> positions;
    public float speed = 1f;
    public float rotSpeed = 0.2f;

    private int posCounter = 0;

    private StateMachine fsm;
    private Rigidbody rb;
    private Animator anim;

    public float turnSpeed = 0.5f;
    private Quaternion initRotation;
    private Quaternion goalRotation;
    private float fracComplete;

    // Start is called before the first frame update
    void Start()
    {
        Dictionary<string, State> states = new Dictionary<string, State>();
        states.Add("Moving", new State(
            new State.StateAction(MovingEnter),
            new State.StateAction(MovingUpdate),
            new State.StateAction(MovingExit)
            )
        );
        states.Add("Turning", new State(
            new State.StateAction(TurningEnter),
            new State.StateAction(TurningUpdate),
            new State.StateAction(TurningExit)
            )
        );
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        anim.SetFloat("Speed", speed);
        fsm = new StateMachine(states, "Turning");
    }

    // Update is called once per frame
    void Update()
    {
        //fsm.Update();
        transform.localRotation = transform.localRotation * Quaternion.Euler(0,rotSpeed,0);
        rb.MovePosition(rb.position + transform.forward * speed * Time.deltaTime);
    }

    void MovingEnter(){}
    void MovingUpdate()
    {   
        rb.MovePosition(rb.position + transform.forward * speed * Time.deltaTime);

        if (Vector3.Distance(positions[posCounter], rb.position) < 1f)
        {
            posCounter++;
            fsm.ChangeState("Turning");
        }
    }
    void MovingExit(){}

    void TurningEnter()
    {
        goalRotation = Quaternion.LookRotation(positions[posCounter] - rb.position, transform.up);
        initRotation = transform.rotation;
        fracComplete = 0;
    }
    void TurningUpdate()
    {
        Debug.Log(fracComplete);
        fracComplete = Mathf.Clamp(Time.deltaTime * turnSpeed + fracComplete, 0, 1);
        transform.rotation = Quaternion.Lerp(initRotation, goalRotation, fracComplete);
        if (fracComplete >= 1)
        {
            fsm.ChangeState("Moving");
        }
    }
    void TurningExit(){}
}