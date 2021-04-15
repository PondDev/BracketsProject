using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
needs to become persistent
*/
public class PlayerMaster : PersistentMonoBehaviour
{
    //Fields
    public float sensitivity = 100f;
    public Vector3 cameraOffset;

    //sort of temp
    public Toolbar toolbar;

    //Functionality
    InputController input;

    PlayerController controller;
    PlayerCamera cam;

    PlayerInventory inventory;
    //maybe temp?
    Inventory inv;

    PlayerMechanics mechanics;
    PlayerInteraction interaction;

    Codex codex;

    //maybe temp too
    public Transform codexParent;

    //pause
    public GameObject pause;

    protected override void Awake()
    {
        base.Awake();
        
        inv = GetComponent<Inventory>();

        input = new InputController(sensitivity);
        controller = new PlayerController(input, GetComponent<Rigidbody>(), GetComponent<GravityObject>(), GetComponent<Animator>(), GetComponent<Collider>());
        cam = new PlayerCamera(input, transform, cameraOffset);
        inventory = new PlayerInventory(input, cam, GetComponentInChildren<InvArm>());
        interaction = new PlayerInteraction(this, input, cam, inv);
        mechanics = new PlayerMechanics(this, input, interaction, toolbar);
        codex = new Codex(codexParent);
    }

    void Start()
    {
        mechanics.Start();
        input.LockCursor();
    }

    public override void OnNewScene()
    {
        //cleanup after scene loads
        this.gameObject.SetActive(true);

        SceneData data = GameObject.Find("SceneData").GetComponent<SceneData>();

        controller.OnNewScene(data);
        cam.OnNewScene(data);

        //skybox? temporarily here, would be better somewhere else but would need access to the player's gameobject
        if (data.skybox)
        {
            data.skybox.player = this.gameObject;
        }
    }

    void Update()
    {
        input.Tick();

        controller.Tick(Time.deltaTime);
        cam.Tick(Time.deltaTime);
        mechanics.Tick(Time.deltaTime);
        inventory.Tick(Time.deltaTime);
        interaction.Tick(Time.deltaTime);

        //pause
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (pause.activeSelf)
            {
                input.LockCursor();
                pause.SetActive(false);
                input.SetInputState(true);
            }
            else
            {
                input.UnlockCursor();
                pause.SetActive(true);
                input.SetInputState(false);
            }
        }
    }

    void FixedUpdate()
    {
        input.FixedTick();

        controller.FixedTick();
    }

    //Used on scene loads
    public void DeActivatePlayer()
    {
        this.gameObject.SetActive(false);
        interaction.Cleanup();
    }

    //Exposing for interaction
    public Transform GetPlayerCameraTransform()
    {
        return cam.camInstance.transform;
    }

    public PlayerController GetPlayerController()
    {
        return controller;
    }

    public Inventory GetPlayerInventory()
    {
        return inv;
    }

    public PlayerInteraction GetPlayerInteraction()
    {
        return interaction;
    }

    public PlayerMechanics GetPlayerMechanics()
    {
        return mechanics;
    }

    public Codex GetCodex()
    {
        return codex;
    }
}
