using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextEditor : MonoBehaviour
{
    public TMP_InputField inputField;
    public TMP_Text outputField;
    public VirtualOS os;

    private void Start()
    {
        
    }

    public void Update()
    {
        string outputData = os.AccessMemory(0).ToString();
        outputField.text = outputData;
    }

    public void OnCompileButton()
    {
        os.CompilePrograme(inputField.text);
        os.SetPrograme();
        os.ExecutePrograme();
    }

    public void OnRunButton()
    {

    }

    
}

public enum EditorMode
{
    
}
