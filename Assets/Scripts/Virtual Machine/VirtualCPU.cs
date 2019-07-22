using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Useage Of Virtual CPU
 * VirtualCPU vcpu = new VirtualCPU(m, e);
 * vcpu.setMachineCode(--);
 * vcpu.StartProcessing();
 * if(error.IsError())
 * vcpu.StopProcessing();
 * Error Report;
 */

public class VirtualCPU
{
    //public 
    public int clockSpeed;

    //private
    VirtualMemory memory;
    Error error;
    Register register;
    List<MachineCode> machineCodes;
    MachineCode currentCode;
    bool isRun;

    public void SetVirtualCPU(VirtualMemory memory, Register register, Error error)
    {
        this.memory = memory;
        this.error = error;
        this.register = register;
    }

    public void SetMachineCodes(List<MachineCode> machineCodes)
    {
        this.machineCodes = machineCodes;
    }

    public void Clear()
    {
        isRun = false;
    }

    public bool IsRun()
    {
        return isRun;
    }

    public void Start()
    {
        isRun = true;
    }

    public void InstructionPerClock()
    {
        int source = 1, dest = 1, result = 1;

        if(machineCodes == null)
        {
            Debug.Log("CPU Error");
            error.cpuError = CPUError.NotDefinedMachineCode;
            return;
        }

        if(machineCodes.Count <= register.ip)
        {
            Debug.Log("CPU Error");
            error.cpuError = CPUError.IndexOutOfRangeFromInstructions;
            isRun = false;
            return;
        }

        //fetch Machine Code
        currentCode = machineCodes[register.ip];
        register.ip += 1;

        Debug.Log("-----------------------");
        Debug.Log("Instruction:" + currentCode.instruction + " Mode:" + currentCode.mode);
        Debug.Log("AX:" + register.ax + " BX:" + register.bx);
        Debug.Log("SP:" + register.sp + " IP:" + register.ip);

        //decoding the instructions and execute
        switch (currentCode.instruction)
        {
            case Instructions.MOV:
                GetDataSource(ref source);
                SetData(source);
                break;
            case Instructions.PUSH:
                memory.SetData(register.sp, currentCode.value, error);
                register.sp += 1;
                break;
            case Instructions.POP:
                result = memory.GetData(register.sp, error);
                SetData(result);
                register.sp--;
                break;
            case Instructions.INC:
                GetDataDest(ref source);
                result = source + 1;
                SetData(result);
                break;
            case Instructions.DEC:
                GetDataDest(ref source);
                result = source - 1;
                SetData(result);
                break;
            case Instructions.ADD:
                GetDataSource(ref source);
                GetDataDest(ref dest);
                result = source + dest;
                SetData(result);
                break;
            case Instructions.SUB:
                GetDataSource(ref source);
                GetDataDest(ref dest);
                source = -source;
                result = source + dest;
                SetData(result);
                break;
            case Instructions.MUL:
                GetDataSource(ref source);
                GetDataDest(ref dest);
                result = source * dest;
                SetData(result);
                break;
            case Instructions.XOR:
                GetDataSource(ref source);
                GetDataDest(ref dest);
                result = source ^ dest;
                SetData(result);
                break;
            case Instructions.OR:
                GetDataSource(ref source);
                GetDataDest(ref dest);
                result = source | dest;
                SetData(result);
                break;
            case Instructions.AND:
                GetDataSource(ref source);
                GetDataDest(ref dest);
                result = source & dest;
                SetData(result);
                break;
            case Instructions.SAL:
                GetDataSource(ref source);
                GetDataDest(ref dest);
                result = source << dest;
                SetData(result);
                break;
            case Instructions.SAR:
                GetDataSource(ref source);
                GetDataDest(ref dest);
                result = source >> dest;
                SetData(result);
                break;
            case Instructions.CMP:
                GetDataSource(ref source);
                GetDataDest(ref dest);
                source = -source;
                result = source + dest;
                break;
            case Instructions.TEST:
                GetDataSource(ref source);
                GetDataDest(ref dest);
                result = source & dest;
                break;
            case Instructions.JMP:
                GetDataSource(ref result);
                register.ip = result;
                break;
            case Instructions.JE:
                if(register.zf)
                {
                    GetDataSource(ref result);
                    register.ip = result;
                }
                break;
            case Instructions.JNE:
                if(!register.zf)
                {
                    GetDataSource(ref result);
                    register.ip = result;
                }
                break;
            case Instructions.JS:
                if (register.sf)
                {
                    GetDataSource(ref result);
                    register.ip = result;
                }
                break;
            case Instructions.JNS:
                if (!register.sf)
                {
                    GetDataSource(ref result);
                    register.ip = result;
                }
                break;
            case Instructions.JG:
                if (!(register.sf^register.of)&!register.zf)
                {
                    GetDataSource(ref result);
                    register.ip = result;
                }
                break;
            case Instructions.JGE:
                if (!(register.sf ^ register.of))
                {
                    GetDataSource(ref result);
                    register.ip = result;
                }
                break;
            case Instructions.JL:
                if (register.sf ^ register.of)
                {
                    GetDataSource(ref result);
                    register.ip = result;
                }
                break;
            case Instructions.JLE:
                if ((register.sf ^ register.of)|register.zf)
                {
                    GetDataSource(ref result);
                    register.ip = result;
                }
                break;
            case Instructions.END:
                isRun = false;
                break;
            default:
                error.cpuError = CPUError.NotDefinedMachineCode;
                isRun = false;
                break;
        }

        register.SetCarryFlag(result, dest, source);
    }

    private void GetDataDest(ref int t)
    {
        currentCode.GetDataDest(register, memory, ref t, error);
    }

    private void GetDataSource(ref int t)
    {
        currentCode.GetDataSource(register, memory, ref t, error);
    }

    private void SetData(int t)
    {
        currentCode.SetData(register, memory, t, error);
    }
}

