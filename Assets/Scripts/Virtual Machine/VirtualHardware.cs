using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirtualHardware : MonoBehaviour
{
    public int clockSpeed;
    public int memoryCapacity = 100;

    VirtualCPU cpu;
    VirtualMemory memory;
    Error error;
    Register register;
    List<MachineCode> machineCodes;

    public void SetVirtualHarware(Error error)
    {
        memory = new VirtualMemory(memoryCapacity);
        register = new Register();
        cpu = new VirtualCPU();
        cpu.SetVirtualCPU(memory, register, error);
        this.error = error;
    }

    public void SetMachineCode(List<MachineCode> machineCodes)
    {
        this.machineCodes = machineCodes;
        cpu.SetMachineCodes(machineCodes);
    }

    public void StartProcessing()
    {
        if (machineCodes == null)
        {
            error.cpuError = CPUError.NotDefinedMachineCode;
            return;
        }
        StartCoroutine("IEClock");
    }

    public void StopProcessing()
    {
        StopAllCoroutines();
    }

    IEnumerator IEClock()
    {
        cpu.Start();
        while (cpu.IsRun())
        {
            cpu.InstructionPerClock();
            yield return new WaitForSeconds(1.0f / clockSpeed);
        }
    }
}
