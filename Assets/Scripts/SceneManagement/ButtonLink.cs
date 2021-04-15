using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonLink : MonoBehaviour
{
    [System.NonSerialized]
    public int sceneIndex = 1;
    private Button button;
    // Start is called before the first frame update

    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(delegate{SceneLoader.instance.LoadLevel(sceneIndex);});
    }
}
