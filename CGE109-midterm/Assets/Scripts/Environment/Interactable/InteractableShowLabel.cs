using Gamekit3D.GameCommands;
using TMPro;
using UnityEngine;
//using UnityEngine.Windows;

public class InteractableShowLabel : MonoBehaviour
{
    [SerializeField] private bool _onFogus = false;

    [SerializeField] private GameObject text;
    public Vector3 _TextStartingpoint;
    public float UpperY = 0.5f;

    public KeyCode _keyCode;

    public bool IsItem = false;
    [SerializeField] private SimpleTranslator SimpleTranslator;

    void Update()
    {
        if (_onFogus)
        {
            text.SetActive(_onFogus);
            Event();
        }
        else if (!_onFogus)
        {
            text.SetActive(_onFogus);
        }
        _onFogus = false;
    }

    public void OnFogusByPlayer()
    {
        _onFogus = true;
    }

    private void Event()
    {
        if (Input.GetKeyDown(_keyCode))
        {
            if (_keyCode == KeyCode.E)
            {
                if (IsItem) 
                {
                    Debug.Log("Pick up item!!"); 
                    return;
                } 
                if (SimpleTranslator != null) 
                {
                    SimpleTranslator.activate = true;
                    return;
                }
                Debug.Log("Nothing");
                return;
            }
        }
        return;
    }
}
