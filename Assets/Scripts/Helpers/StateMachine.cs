using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine
{
    public Dictionary<string, State> states;

    public string currentStateStr;
    State currentState;
    

    public StateMachine(Dictionary<string, State> _states, string initialState)
    {
        states = _states;
        currentStateStr = initialState;
        currentState = states[initialState];
    }

    public void Update()
    {
        currentState.Update();
    }

    public void ChangeState(string intoState)
    {
        currentState.Exit();
        currentState = states[intoState];
        currentStateStr = intoState;
        currentState.Enter();
    }
}

public class State
{
    public delegate void StateAction();

    StateAction enterHandler;
    StateAction updateHandler;
    StateAction exitHandler;

    public State(StateAction _enterHandler, StateAction _updateHandler, StateAction _exitHandler)
    {
        enterHandler = _enterHandler;
        updateHandler = _updateHandler;
        exitHandler = _exitHandler;
    }

    public void Enter()
    {
        enterHandler();
    }
    public void Update()
    {
        updateHandler();
    }
    public void Exit()
    {
        exitHandler();
    }
}
