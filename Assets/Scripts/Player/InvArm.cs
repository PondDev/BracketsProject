using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvArm : MonoBehaviour
{
    public PlayerController controller;
    public PlayerCamera cam;

    private StateMachine fsm;

    public float switchTime = 0.5f;
    private float startTime;

    private bool invActive = false;

    public float posDifference = 0.3f;
    private Vector3 initialPosition;
    private Quaternion initialRotation;

    private Vector3 currentPosition;
    private Quaternion currentRotation;

    private Vector3 targetPosition;
    private Quaternion targetRotation;

    void Start()
    {
        //not great, will be nicer when inv arm is properly moved
        GetComponentInChildren<Canvas>().worldCamera = cam.camInstance.GetComponent<Camera>();
        Dictionary<string, State> states = new Dictionary<string, State>();
        states.Add("Still", new State(
            new State.StateAction(StillEnter),
            new State.StateAction(StillUpdate),
            new State.StateAction(StillExit)
            )
        );
        states.Add("Switching", new State(
            new State.StateAction(SwitchingEnter),
            new State.StateAction(SwitchingUpdate),
            new State.StateAction(SwitchingExit)
            )
        );
        fsm = new StateMachine(states, "Still");

        initialPosition = transform.localPosition;
        initialRotation = transform.localRotation;
        currentPosition = initialPosition;
        currentRotation = initialRotation;
    }

    void Update()
    {
        fsm.Update();
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

    private void StillEnter() {}
    private void StillUpdate() {}
    private void StillExit() {}

    private void SwitchingEnter()
    {
        startTime = Time.time;
        if (invActive)
        {
            targetPosition = initialPosition;
            targetRotation = initialRotation;
            invActive = false;
        }
        else
        {
            //camera position + offset view dir
            targetPosition = cam.camInstance.transform.localPosition + cam.camInstance.transform.localRotation * Vector3.forward * posDifference;
            targetRotation = cam.camInstance.transform.localRotation;
            invActive = true;
        }
    }

    private void SwitchingUpdate()
    {
        if (Time.time - startTime < switchTime)
        {
            float fracComplete = (Time.time - startTime) / switchTime;
            transform.localPosition = Vector3.Slerp(currentPosition, targetPosition, fracComplete);
            transform.localRotation = Quaternion.Slerp(currentRotation, targetRotation, fracComplete);
        }
        else
        {
            transform.localPosition = Vector3.Slerp(currentPosition, targetPosition, 1f);
            transform.localRotation = Quaternion.Slerp(currentRotation, targetRotation, 1f);
            fsm.ChangeState("Still");
        }
    }

    private void SwitchingExit()
    {
        currentPosition = targetPosition;
        currentRotation = targetRotation;
    }
}
