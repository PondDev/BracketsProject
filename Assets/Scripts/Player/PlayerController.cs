using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController
{
    private StateMachine fsm;
    private InputController input;
    private Rigidbody rb;
    private Transform transform;

    private Animator animator;

    private float speed = 2f;
    private float jump = 250f;
    private float movementSmoothing = 0.05f;
    private float mouseSensitivity = 170f;
    private LayerMask groundedMask;

    private float boundsAdjust = 0.1f;

    Vector3 movement;
    Vector3 smoothMoveVelocity;
    public Vector3 targetMoveAmount;
    private float inX;
    private float inZ;
    private bool inJump;
    public float airResistance;
    
    private GravityObject gravObj;

    public PlayerController(InputController _input, Rigidbody _rb, GravityObject _gravObj, Animator _animator, Collider coll)
    {
        input = _input;
        rb = _rb;
        animator = _animator;
        transform = rb.transform;

        //set gravobj corners
        gravObj = _gravObj;
        gravObj.SetCorners(
            new Vector3[4]{
                new Vector3(coll.bounds.extents.x - boundsAdjust, 0, 0),
                new Vector3(0, 0, coll.bounds.extents.z - boundsAdjust),
                new Vector3(-coll.bounds.extents.x + boundsAdjust, 0, 0),
                new Vector3(0, 0, -coll.bounds.extents.z + boundsAdjust)
            }
        );
        
        //init state machine
        Dictionary<string, State> states = new Dictionary<string, State>();
        states.Add("Grounded", new State(
            new State.StateAction(GroundedEnter),
            new State.StateAction(GroundedUpdate),
            new State.StateAction(GroundedExit)
            )
        );
        states.Add("Jumping", new State(
            new State.StateAction(JumpingEnter),
            new State.StateAction(JumpingUpdate),
            new State.StateAction(JumpingExit)
            )
        );
        fsm = new StateMachine(states, "Grounded");
    }

    public void OnNewScene(SceneData data)
    {
        rb.velocity = Vector3.zero;
        //Set location + rotation
        transform.position = data.playerInitial;
        transform.rotation = Quaternion.identity;

        //Set controller mode (grav obj or rb)
        if (data.planet)
        {
            gravObj.planet = data.planet;
            rb.useGravity = false;
            
            gravObj.enabled = true;
        }
        else
        {
            rb.useGravity = true;
            gravObj.enabled = false;
        }

        //Set controller variables
        airResistance = data.airResistance;
    }

    public void Tick(float delta)
    {
        GetInput();
        fsm.Update();
        
        if (input.inputOn)
        {
            float mouseX = input.Mouse.deltaXRaw * input.sensitivity * delta;
            transform.Rotate(Vector3.up * mouseX, Space.Self);
        }
    }

    public void FixedTick()
    {
        Vector3 worldMove = transform.TransformDirection(movement) * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + worldMove);
    }

    public void GetInput()
    {
        if (input.inputOn)
        {
            inX = Input.GetAxisRaw("Horizontal");
            inZ =  Input.GetAxisRaw("Vertical");
            inJump = input.Jump.Down;
        }
        else
        {
            inX = 0;
            inZ = 0;
            inJump = false;
        }
    }

    //State Functions

    void GroundedEnter(){}
    void GroundedUpdate()
    {
        //Calculation
        Vector3 moveDir = new Vector3(inX,0,inZ).normalized;
        targetMoveAmount = moveDir * speed;
        movement =  Vector3.SmoothDamp(movement, targetMoveAmount, ref smoothMoveVelocity, movementSmoothing);

        //Animation
        if (inX > 0 || inZ > 0)
        {
            animator.SetBool("isWalking", true);
        }
        else
        {
            animator.SetBool("isWalking", false);
        }


        //State Change
        if (gravObj.enabled)
        {
            if (Input.GetKeyDown(input.Jump.key))
            {
                rb.AddForce(transform.up * jump);
                fsm.ChangeState("Jumping");
            }
            if (!gravObj.grounded)
            {
                fsm.ChangeState("Jumping");
            }
        }

    }
    void GroundedExit()
    {
        smoothMoveVelocity = Vector3.zero;
        animator.SetBool("isWalking", false);
    }

    void JumpingEnter(){}
    void JumpingUpdate()
    {
        //Calculation
        if (airResistance != 0)
        {
            Vector3 moveDir = new Vector3(inX,0,inZ).normalized * speed;
            targetMoveAmount = moveDir;
            movement =  Vector3.SmoothDamp(movement, targetMoveAmount, ref smoothMoveVelocity, airResistance);
        }
        else
        {
            movement = transform.InverseTransformDirection(targetMoveAmount);
        }

        //State Change
        if (gravObj.enabled)
        {
            if (gravObj.grounded)
            {
                fsm.ChangeState("Grounded");
            }
        }
    }
    void JumpingExit()
    {
        smoothMoveVelocity = Vector3.zero;
    }

    public float ReplaceJump(float newJump)
    {
        float curJump = jump;
        jump = newJump;
        return curJump;
    }
}
