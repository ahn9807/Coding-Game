using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirtualOS : MonoBehaviour
{
    //public
    public string testString;
    public int errorLine;
    public VirtualHardware hardware;

    //private
    List<MachineCode> machineCodes;
    Compiler compiler;
    Error error;
    bool isCompiled;

    public void Start()
    {
        StartOS();
        CompilePrograme(testString);
        SetPrograme(machineCodes);
        ExecutePrograme();
    }

    public void StartOS()
    {
        error = new Error();
        hardware.SetVirtualHarware(error);
        compiler = new Compiler();
    }

    public void SetPrograme(List<MachineCode> machineCodes)
    {
        hardware.SetMachineCode(machineCodes);
    }

    public bool CompilePrograme(string rawData)
    {
        isCompiled = true;
        return compiler.Compile(rawData, ref machineCodes, ref errorLine);
    }

    public void ExecutePrograme()
    {
        if(!isCompiled)
        {
            return;
        }
        hardware.StartProcessing();
        if(error.IsError())
        {
            hardware.StopProcessing();
            Debug.Log(error.cpuError);
            Debug.Log(error.memoryError);
        }
    }

    public void EndOS()
    {
        hardware.StopProcessing();
    }
}
