using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class InputManager : SingletonMonoBehaviour<InputManager>
{
    public float ZombieSpeed { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnChangeSpeed(string _text)
    {
        ZombieSpeed = float.Parse(_text);
        Debug.Log(ZombieSpeed);
    }
}
