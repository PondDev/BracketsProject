using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SporeSpawner : MonoBehaviour
{
    public GameObject spore;
    Transform sporeSpawnPoint;
    // Start is called before the first frame update

    Animator anim;
    AudioSource sound;

    void Start()
    {
        sporeSpawnPoint = transform.Find("SporePos");
        anim = GetComponent<Animator>();
        anim.SetFloat("SpeedMultiplier", Random.Range(0.05f,0.4f));
        sound = GetComponent<AudioSource>();
        sound.pitch = Mathf.Abs(transform.localScale.x - 2);
    }

    public void SpawnSpore()
    {
        //Maybe particles?
        GameObject instanceSpore = Instantiate(spore, sporeSpawnPoint.position, sporeSpawnPoint.rotation);
        instanceSpore.transform.localScale = transform.localScale;
        sound.Play();
    }
}
