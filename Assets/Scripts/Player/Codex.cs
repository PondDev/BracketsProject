using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Codex
{
    Dictionary<int, GameObject> codexData;
    GameObject codexDataPrefab;
    Transform codexParent;
    //Sample entry prefab?
    //parent transform for instatiateds

    public Codex(Transform _codexParent)
    {
        codexParent = _codexParent;
        codexData = new Dictionary<int, GameObject>();

        //other codex set up
        codexDataPrefab = Resources.Load<GameObject>("Prefabs/UI/CodexEntry");
    }
    
    public void AddSample(Sample sample)
    {
        if (!codexData.ContainsKey(sample.id))
        {
            GameObject instanceObj = Object.Instantiate(codexDataPrefab, codexParent);
            instanceObj.GetComponentInChildren<Image>().overrideSprite = sample.icon;
            string traitString = "";
            for (int i = 0; i < sample.traits.Count; i++)
            {
                if (i == sample.traits.Count-1)
                {
                    traitString += sample.traits[i].ToString();
                }
                else
                {
                    traitString += sample.traits[i].ToString() + ", ";
                }
            }
            foreach(Trait trait in sample.traits)
            {
                
            }
            instanceObj.GetComponentInChildren<Text>().text = traitString;
            codexData.Add(sample.id, instanceObj);

            //instantiate new entry prefab
            //set all values
        }
    }
}
