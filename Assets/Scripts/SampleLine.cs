using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vectrosity;

public class SampleLine : MonoBehaviour
{
    private VectorLine line;
    Transform lineTrans;

    // Start is called before the first frame update
    void Start()
    {
        lineTrans = transform;

        var lines = new List<Vector3>(){new Vector3(0.1f, -0.1f, 0.1f), new Vector3(0.17f, 0.589f, 0.17f), new Vector3(0.17f, 0.589f, 0.17f), new Vector3(-0.17f, 0.589f, 0.17f), new Vector3(-0.17f, 0.589f, 0.17f), new Vector3(-0.1f, -0.1f, 0.1f), new Vector3(-0.1f, -0.1f, 0.1f), new Vector3(0.1f, -0.1f, 0.1f), new Vector3(0.002f, 0.786f, 0.002f), new Vector3(0.002f, 0.786f, -0.002f), new Vector3(0.002f, 0.786f, -0.002f), new Vector3(-0.002f, 0.786f, -0.002f), new Vector3(-0.002f, 0.786f, -0.002f), new Vector3(0.002f, 0.786f, 0.002f), new Vector3(-0.002f, 0.786f, -0.002f), new Vector3(-0.002f, 0.786f, 0.002f), new Vector3(-0.002f, 0.786f, 0.002f), new Vector3(0.002f, 0.786f, 0.002f), new Vector3(0.17f, 0.589f, -0.17f), new Vector3(0.1f, -0.1f, -0.1f), new Vector3(0.1f, -0.1f, -0.1f), new Vector3(-0.1f, -0.1f, -0.1f), new Vector3(-0.1f, -0.1f, -0.1f), new Vector3(0.17f, 0.589f, -0.17f), new Vector3(-0.1f, -0.1f, -0.1f), new Vector3(-0.17f, 0.589f, -0.17f), new Vector3(-0.17f, 0.589f, -0.17f), new Vector3(0.17f, 0.589f, -0.17f), new Vector3(0.1f, -0.1f, -0.1f), new Vector3(0.1f, -0.1f, 0.1f), new Vector3(-0.1f, -0.1f, 0.1f), new Vector3(0.1f, -0.1f, -0.1f), new Vector3(-0.1f, -0.1f, 0.1f), new Vector3(-0.1f, -0.1f, -0.1f), new Vector3(-0.17f, 0.589f, 0.17f), new Vector3(-0.17f, 0.589f, -0.17f), new Vector3(-0.17f, 0.589f, -0.17f), new Vector3(-0.1f, -0.1f, 0.1f), new Vector3(0.17f, 0.589f, -0.17f), new Vector3(0.17f, 0.589f, 0.17f), new Vector3(0.17f, 0.589f, 0.17f), new Vector3(0.049f, 0.744f, 0.049f), new Vector3(0.049f, 0.744f, 0.049f), new Vector3(-0.049f, 0.744f, 0.049f), new Vector3(-0.049f, 0.744f, 0.049f), new Vector3(0.17f, 0.589f, 0.17f), new Vector3(-0.049f, 0.744f, 0.049f), new Vector3(-0.17f, 0.589f, 0.17f), new Vector3(-0.049f, 0.744f, 0.049f), new Vector3(-0.049f, 0.744f, -0.049f), new Vector3(-0.049f, 0.744f, -0.049f), new Vector3(-0.17f, 0.589f, 0.17f), new Vector3(-0.049f, 0.744f, -0.049f), new Vector3(-0.17f, 0.589f, -0.17f), new Vector3(-0.049f, 0.744f, -0.049f), new Vector3(0.049f, 0.744f, -0.049f), new Vector3(0.049f, 0.744f, -0.049f), new Vector3(-0.17f, 0.589f, -0.17f), new Vector3(0.049f, 0.744f, -0.049f), new Vector3(0.17f, 0.589f, -0.17f), new Vector3(0.049f, 0.744f, -0.049f), new Vector3(0.049f, 0.744f, 0.049f), new Vector3(0.049f, 0.744f, 0.049f), new Vector3(0.17f, 0.589f, -0.17f), new Vector3(0.049f, 0.744f, 0.049f), new Vector3(0.002f, 0.786f, 0.002f), new Vector3(-0.002f, 0.786f, 0.002f), new Vector3(0.049f, 0.744f, 0.049f), new Vector3(-0.002f, 0.786f, 0.002f), new Vector3(-0.049f, 0.744f, 0.049f), new Vector3(-0.002f, 0.786f, -0.002f), new Vector3(-0.049f, 0.744f, 0.049f), new Vector3(-0.002f, 0.786f, -0.002f), new Vector3(-0.049f, 0.744f, -0.049f), new Vector3(0.002f, 0.786f, -0.002f), new Vector3(-0.049f, 0.744f, -0.049f), new Vector3(0.002f, 0.786f, -0.002f), new Vector3(0.049f, 0.744f, -0.049f), new Vector3(0.002f, 0.786f, 0.002f), new Vector3(0.049f, 0.744f, -0.049f)};
        
        line =  new VectorLine("Crystal", lines, 3.0f, LineType.Discrete, Joins.Weld);
        line.drawTransform = transform;
        lineTrans.localScale = new Vector3(1.1f, 1.1f, 1.1f);
        

        line.Draw3DAuto();
    }
}
