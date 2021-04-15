using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/*
 * Need to expand this to allow combination of samples also
 *
 * Need to make sure that multiple of the same trait is tracked also
 */

public class Combiner : MonoBehaviour
{
    //Complete list of recipes
    List<RecipeData> recipes = new List<RecipeData>();

    Animator animator;

    //temp
    public Transform outSpawn;

    public Transform pillarParent;
    public Transform particleParent;
    Dictionary<string, CombinerPillar> pillars = new Dictionary<string, CombinerPillar>();
    Dictionary<string, ParticleSystem> particles = new Dictionary<string, ParticleSystem>();

    List<string> keys = new List<string>();
    
    Vector3 targetScale;
    GameObject curProduct;
    
    bool ticking = false;
    float particlesTimer = 3f;
    float time = 0;

    bool productExists = false;

    void Start()
    {
        animator = GetComponent<Animator>();

        foreach(Transform trans in particleParent)
        {
            particles.Add(trans.name, trans.GetComponent<ParticleSystem>());
        }

        foreach(Transform trans in pillarParent)
        {
            pillars.Add(trans.name, trans.GetComponent<CombinerPillar>());
        }

        recipes.Add(new RecipeData() {productPrefab = new Flashlight().mechanicObject, traits = new List<Trait>(){Trait.Metallic, Trait.Light}});
        recipes.Add(new RecipeData() {productPrefab = new HeatRay().mechanicObject, traits = new List<Trait>() {Trait.Vitality, Trait.Metallic, Trait.Hot}});
        recipes.Add(new RecipeData() {productPrefab = new WateringCan().mechanicObject, traits = new List<Trait>() {Trait.Tough, Trait.Liquid, Trait.Growing}});
        recipes.Add(new RecipeData() {productPrefab = new SpringShoes().mechanicObject, traits = new List<Trait>() {Trait.NoGravity, Trait.Metallic, Trait.Pliable}});
        recipes.Add(new RecipeData() {productPrefab = Resources.Load<GameObject>("Prefabs/Samples/Compost"), traits = new List<Trait>(){Trait.Flora, Trait.Flora, Trait.Flora}});
        recipes.Add(new RecipeData() {productPrefab = Resources.Load<GameObject>("Prefabs/Samples/Rope"), traits = new List<Trait>(){Trait.Fibrous, Trait.Fibrous}});
    }

    void Update()
    {
        if (ticking)
        {
            float fraction = time / particlesTimer;

            if (curProduct != null)
            {
                curProduct.transform.localScale = Vector3.Lerp(Vector3.zero, targetScale, fraction);
            }

            foreach (string key in keys)
            {
                pillars[key].curSample.transform.localScale = Vector3.Lerp(pillars[key].sampleScale, Vector3.zero, fraction);
            }

            time += Time.deltaTime;
            if (time >= particlesTimer)
            {
                time = 0;
                ticking = false;
                ParticlesOff();

                foreach (string key in keys)
                {
                    pillars[key].DestroySample();
                }
                keys.Clear();

                animator.Play("CombinerDoorOpen");
            }
        }

        if (productExists)
        {
            if (curProduct == null)
            {
                animator.Play("CombinerDoorClose");
                productExists = false;
            }
        }
    }

    public void ParticlesOff()
    {
        foreach (KeyValuePair<string, ParticleSystem> entry in particles)
        {
            entry.Value.Stop();
        }
    }

    public void CombineSamples()
    {
        List<Trait> totalTraits = new List<Trait>();

        bool noSamples = true;

        //Construct single list of unique input traits
        foreach (KeyValuePair<string, CombinerPillar> entry in pillars)
        {
            CombinerPillar pillar = entry.Value;
            if (pillar.curSample != null)
            {
                //Adds all sample traits to the set
                totalTraits.AddRange(pillar.curSample.sample.traits);
                noSamples = false;

                keys.Add(entry.Key);
            }
        }

        //this is dumb but w/e
        if (noSamples)
        {
            keys.Clear();
            return;
        }

        List<RecipeData> validRecipes = new List<RecipeData>();
        bool isProduct = false;

        //Sorting algorithm to get valid recipes
        //My complete list of total traits has to cover the recipe
        foreach (RecipeData recipe in recipes)
        {
            // This check is probably more expensive than it could be
            List<Trait> hold = recipe.traits;
            foreach (Trait trait in totalTraits)
            {
                if (hold.Contains(trait))
                {
                    hold.Remove(trait);
                }
            }
            if (hold.Count <= 0)
            {
                //recipe is valid
                validRecipes.Add(recipe);
                //check if any products are found
                isProduct = true;
            }
        }

        if (isProduct)
        {
            ticking = true;
            
            foreach (string key in keys)
            {
                //not great
                particles[key].GetComponent<ParticleSystemRenderer>().material = pillars[key].curSample.currentMat;
                particles[key].Play();
            }

            //Get a random recipe from valids?
            RecipeData product = validRecipes[Random.Range(0, validRecipes.Count - 1)];

            //fulfil item
            curProduct = Instantiate(product.productPrefab, outSpawn.position, Quaternion.identity);
            curProduct.GetComponent<SceneObject>().InitInstance(product.productPrefab);

            targetScale = curProduct.transform.localScale;
            curProduct.transform.localScale = Vector3.zero;
            productExists = true;

            //TEMP
            if (curProduct.name == "Flashlight(Clone)")
            {
                TempTasksManager.instance.UpdateCurrentTask(4);
            }
            if (curProduct.name == "HeatRay(Clone)")
            {
                TempTasksManager.instance.UpdateCurrentTask(5);
            }
        }
        else
        {
            keys.Clear();
        }
    }

    //This needs to be expanded
    //Maybe just prefab?
    public struct RecipeData
    {
        public GameObject productPrefab;
        public List<Trait> traits;
    }
}