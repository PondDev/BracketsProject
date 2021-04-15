using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController
{ 
    public float sensitivity;
    public bool inputOn;

    public InputButton Interact = new InputButton(KeyCode.E);
    public InputButton Pocket = new InputButton(KeyCode.Q);
    
    public InputButton Mechanic = new InputButton(KeyCode.Mouse0);
    public InputButton Jump = new InputButton(KeyCode.Space);

    public NumberButton Numbers = new NumberButton();

    public InputAxis Horizontal = new InputAxis(KeyCode.D, KeyCode.A);
    public InputMouse Mouse = new InputMouse();

    private bool m_FixedUpdatePassed;

    public InputController(float _sensitivity)
    {
        sensitivity = _sensitivity;
        SetInputState(true);
    }

    public void Tick()
    {
        RefreshInputs();
        m_FixedUpdatePassed = false;
    }

    public void FixedTick()
    {
        m_FixedUpdatePassed = true;
    }

    private void RefreshInputs()
    {
        Interact.Refresh(m_FixedUpdatePassed);
        Pocket.Refresh(m_FixedUpdatePassed);
        Mechanic.Refresh(m_FixedUpdatePassed);
        Jump.Refresh(m_FixedUpdatePassed);

        Numbers.Refresh();

        Horizontal.Refresh();
        Mouse.Refresh();
    }

    public void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    public void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void SetInputState(bool state)
    {
        inputOn = state;
    }
}

public class InputButton
{
    public KeyCode key;
    public bool Down;
    public bool Held;
    public bool Up;

    //track inputs between fixed updates
    bool m_AfterFixedUpdateDown;
    bool m_AfterFixedUpdateHeld;
    bool m_AfterFixedUpdateUp;

    //Constructor
    public InputButton(KeyCode key)
    {
        this.key = key;
    }

    //Update the Button
    public void Refresh(bool fixedUpdatePassed)
    {
        if (fixedUpdatePassed)
        {
            Down = Input.GetKeyDown(key);
            Held = Input.GetKey(key);
            Up = Input.GetKeyUp(key);

            m_AfterFixedUpdateDown = Down;
            m_AfterFixedUpdateHeld = Held;
            m_AfterFixedUpdateUp = Up;
        }

        Down = Input.GetKeyDown(key) || m_AfterFixedUpdateDown;
        Held = Input.GetKey(key) || m_AfterFixedUpdateHeld;
        Up = Input.GetKeyUp(key) || m_AfterFixedUpdateUp;

        m_AfterFixedUpdateDown |= Down;
        m_AfterFixedUpdateHeld |= Held;
        m_AfterFixedUpdateUp |= Up;
    }
}

 public class InputAxis
{
    public KeyCode positive;
    public KeyCode negative;

    public float Value;

    //Constructor
    public InputAxis(KeyCode positive, KeyCode negative)
    {
        this.positive = positive;
        this.negative = negative;
    }

    //Update the Axis
    public void Refresh()
    {
        bool positiveHeld = false;
        bool negativeHeld = false;

        positiveHeld = Input.GetKey(positive);
        negativeHeld = Input.GetKey(negative);

        Value = 0f;
        if (positiveHeld & negativeHeld)
        {
            Value = 0f;
        }
        else if (positiveHeld)
        {
            Value = 1f;
        }
        else if (negativeHeld)
        {
            Value = -1f;
        }
    }
}

public class InputMouse
{
    public float mouseX;
    public float mouseY;

    public float deltaXRaw;
    public float deltaYRaw;

    public Vector2 mousePos;

    public void Refresh()
    {
        //Kinda temp
        deltaXRaw = Input.GetAxis("Mouse X");
        deltaYRaw = Input.GetAxis("Mouse Y");
    
        mouseX = Input.mousePosition.x;
        mouseY = Input.mousePosition.y;

        mousePos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
    }
}

public class NumberButton
{
    public int recentNum;
    static Dictionary<KeyCode, int> keyDict = new Dictionary<KeyCode, int>
    {
        {KeyCode.Alpha1, 1},
        {KeyCode.Alpha2, 2},
        {KeyCode.Alpha3, 3},
        {KeyCode.Alpha4, 4},
        {KeyCode.Alpha5, 5}
    };

    public NumberButton()
    {
        recentNum = 1;
    }

    public void Refresh()
    {
        foreach(KeyValuePair<KeyCode, int> entry in keyDict)
        {
            if (Input.GetKeyDown(entry.Key))
            {
                recentNum = entry.Value;
            }
        }
    }
}