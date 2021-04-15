using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Analyser : MonoBehaviour
{
    public Transform textParent;
    List<Text> textOut;
    Sample curSample;
    public ParticleSystem particles;

    bool state = false;

    void Start()
    {
        textOut = new List<Text>(textParent.GetComponentsInChildren<Text>());
    }

    public void Analyse(Codex codex)
    {
        if (curSample != null)
        {
            //temp
            TempTasksManager.instance.UpdateCurrentTask(1);


            particles.Play();
            codex.AddSample(curSample);
        }
        


        /*
        if (!state)
        {
            if (curSample != null)
            {
                AnalyserOn();
            }
        }
        else
        {
            AnalyserOff();
        }
        */
    }

    void OnTriggerEnter(Collider other)
    {
        SampleBehaviour sample = other.GetComponent<SampleBehaviour>();
        if (sample != null)
        {
            curSample = sample.sample;
        }
    }

    void AnalyserOn()
    {
        particles.Play();
        for (int i = 0; i < curSample.traits.Count; i++)
        {
            textOut[i].text =  curSample.traits[i].ToString();
            //write to codex
                //Instance? Or somehow pass it in?
            //probably just pass  sample in and have codex deciphher?
        }
        state = true;
    }

    void AnalyserOff()
    {
        particles.Stop();
        foreach(Text text in textOut)
        {
            text.text = "";
        }
        state = false;
    }

    void OnTriggerExit(Collider other)
    {
        AnalyserOff();
        curSample = null;
    }
}
