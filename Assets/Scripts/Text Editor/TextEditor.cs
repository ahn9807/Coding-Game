using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextEditor : MonoBehaviour
{
    public TMP_InputField inputField;
    public TMP_Text outputField;
    public GameObject console;
    [HideInInspector]
    public VirtualOS os;
    [HideInInspector]
    public VirtualFunction vfunction;

    private void Start()
    {
        HideEditor();
    }

    public void Update()
    {
        ObjectClick();

        if(os != null)
        {
            if(inputField != null)
            {
                
            }
            if(outputField != null)
            {
                PrintMemory();
            }
        }
    }

    /*
    public string EditInput(string input)
    {
        
    }
    */

    public bool ObjectClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform != null)
                {
                    if(hit.transform.GetComponent<VirtualFunction>() != null)
                    {
                        if (vfunction != null)
                        {
                            vfunction.DettachedToEditor();
                        }

                        vfunction = hit.transform.GetComponent<VirtualFunction>();
                        vfunction.AttachedToEditor();
                        os = vfunction.vos;
                    }

                    OpenEditor();
                    return true;
                }
            }
        }

        return false;
    }

    public void HideEditor()
    {
        console.SetActive(false);
    }

    public void OpenEditor()
    {
        console.SetActive(true);
    }

    public void PrintMemory()
    {
        string outputData = "";

        for (int i = 0; i < os.GetHardware().memoryCapacity; i++)
        {
            outputData += os.AccessMemory(i).ToString() + " ";
        }

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
