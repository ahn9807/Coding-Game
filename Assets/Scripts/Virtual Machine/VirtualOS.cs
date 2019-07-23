using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(VirtualHardware))]
public class VirtualOS : MonoBehaviour
{
    //public
    public int errorLine;
    public int clockSpeed;
    public int memoryCapacity;

    //private
    VirtualHardware hardware;
    List<MachineCode> machineCodes;
    Compiler compiler;
    Error error;
    bool isCompiled;

    public void Awake()
    {
        error = new Error();
        hardware = GetComponent<VirtualHardware>();
        compiler = new Compiler();
    }

    public void Start()
    {
        hardware.memoryCapacity = memoryCapacity;
        hardware.clockSpeed = clockSpeed;
        hardware.SetVirtualHarware(error);
    }

    public VirtualHardware GetHardware()
    {
        return hardware;
    }

    public void SetPrograme(List<MachineCode> machineCodes)
    {
        hardware.SetMachineCode(machineCodes);
    }

    public void SetPrograme()
    {
        hardware.SetMachineCode(machineCodes);
    }

    public bool CompilePrograme(string rawData)
    {
        compiler.Clear();
        isCompiled = compiler.Compile(rawData, ref machineCodes, ref errorLine);
        return isCompiled;
    }

    public void ExecutePrograme()
    {
        hardware.clockSpeed = clockSpeed;
        if(!isCompiled)
        {
            Debug.Log("Not Compiled yet");
            error.cpuError = CPUError.NotDefinedMachineCode;
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

    public void StopExecute()
    {
        hardware.Clear();
    }

    public int AccessMemory(int index)
    {
        return hardware.AccessMemory(index);
    }

    public void WriteMemroy(int index, int value)
    {
        hardware.WirteMemory(index, value);
    }

    public void EndOS()
    {
        hardware.StopProcessing();
    }
}
