using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
/*
TODO:
 - Make test machines
 - Draw out samples tree
 - Samples notebook
 - ?
*/

[System.Serializable]
public class Sample
{
    public int id;
    public string slug;
    
    public List<Trait> traits;
    public Dictionary<Test, int> products;

    [System.NonSerialized]
    public GameObject worldPrefab;
    [System.NonSerialized]
    public Sprite icon;

    public void Init()
    {
        this.worldPrefab = Resources.Load<GameObject>("Prefabs/Samples/" + slug);
        this.icon = Resources.Load<Sprite>("Sprites/Items/" + slug);
    }

    public Sample(int _id, string _slug, List<Trait> _traits, Dictionary<Test, int> _products)
    {
        id = _id;
        slug = _slug;
        traits = _traits;
        products = _products;
    }
}

public enum Test
{
    Cooker,
    Crusher
}

public enum Trait
{
    Conductive,
    Brittle,
    Fibrous,
    Metallic,
    Elastic,
    Absorbent,
    Acidic,
    Alkaline,
    Toxic,
    Dense,
    Sticky,
    Light,
    Vitality,
    Living,
    Flora,
    Hot,
    Liquid,
    Tough,
    Powder,
    Emitter,
    Growing,
    Frozen,
    Crystalline,
    NoGravity,
    Pliable
}