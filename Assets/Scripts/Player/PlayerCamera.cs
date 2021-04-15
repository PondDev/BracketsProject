using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera
{
    InputController input;
    
    static float viewClampMax = 85f;
    static float viewClampMin = -75f;

    float viewAngle = 0f;

    public GameObject camInstance;

    public PlayerCamera(InputController _input, Transform transform, Vector3 offset)
    {
        input = _input;
        GameObject camPrefab = Resources.Load<GameObject>("PlayerCamera");
        camInstance = Object.Instantiate(camPrefab, transform);
        camInstance.transform.localPosition = offset;
    }

    public void OnNewScene(SceneData data)
    {
        camInstance.GetComponentInChildren<Light>().color = data.mainLight.color;
    }

    public void Tick(float delta)
    {
        if (input.inputOn)
        { 
            viewAngle -= input.Mouse.deltaYRaw * input.sensitivity * delta;
            viewAngle = Mathf.Clamp(viewAngle, viewClampMin, viewClampMax);

            camInstance.transform.localRotation = Quaternion.Euler(viewAngle, 0, 0);
        }
    }
}
