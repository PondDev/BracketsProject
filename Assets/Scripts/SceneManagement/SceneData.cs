using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneData : MonoBehaviour
{
    public Vector3 playerInitial = Vector3.zero;
    public GravitySource planet;
    public Light mainLight;
    public int buttonSceneIndex = 1;
    public float airResistance = 1;
    public SkyboxPlayer skybox;
}
