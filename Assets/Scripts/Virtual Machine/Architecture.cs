using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Blue print
//Harvard Architecture

public enum Instructions
{
    MOV = 0x0,
    PUSH = 0x1,
    POP = 0x2,
    INC = 0x3,
    DEC = 0x4,
    ADD = 0x5,
    SUB = 0x6,
    MUL = 0x7,
    XOR = 0x8,
    OR = 0x9,
    AND = 0xA,
    SAL = 0xB,
    SAR = 0xC,
    CMP = 0xD,
    TEST = 0xE,
    JMP = 0xF,
    JE = 0xF1,
    JNE = 0xF2,
    JS = 0xF3,
    JNS = 0xF4,
    JG = 0xF5,
    JGE = 0xF6,
    JL = 0xF7,
    JLE = 0xF8,
    END = 0xF9,
}

public class Register
{
    public int ax, bx, cx, dx, si, di, bp, sp;
    public int ip;
    public bool zf, sf, of;

    public void SetCarryFlag(int t, int a, int b)
    {
        zf = false;
        sf = false;
        of = false;

        zf |= t == 0;
        sf |= t < 0;
        of |= ((a < 0) == (b < 0)) && ((t < 0) != (a < 0));
    }
}

public enum RegisterIndex
{
    ax = 0x0,
    bx = 0x1,
    cx = 0x2,
    dx = 0x3,
    si = 0x4,
    di = 0x5,
    bp = 0x6,
    sp = 0x7,
    ip = 0x8,
    zf = 0x9,
    sf = 0xA,
    of = 0xB,
}

public enum Mode
{
    RegisterToRegister = 0x1,
    RegisterToMemory = 0x2,
    MemoryToRegister = 0x3,
    MemoryToMemory = 0x4,
    RegisterOnly = 0x5,
    MemoryOnly = 0x6,
    NumberToRegister = 0x7,
    NumberToMemory = 0x8,
    NumberOnly = 0x9,
    RegisterToNumber = 0xA,
    MemoryToNumber = 0xB,
    Noting = 0xF,
}

public enum CPUError
{
    Nothing,
    NotDefinedRegisterError,
    NotDefinedMachineCode,
    IndexOutOfRangeFromInstructions,
}

public enum MemoryError
{
    Nothing,
    IndexOutOfRange,
}

public class Error
{
    public CPUError cpuError;
    public MemoryError memoryError;

    public bool IsError()
    {
        if (cpuError != CPUError.NotDefinedMachineCode &&
           memoryError != MemoryError.Nothing)
            return true;

        return false;
    }
}

public class MachineCode
{
    public Instructions instruction;
    public Mode mode;
    public RegisterIndex sourceRegister;
    public RegisterIndex destRegister;
    public int sourceMemoryIndex;
    public bool isSourceMemoryIndexIsRegister;
    public bool isDestMemoryIndexIsRegister;
    public int destMemoryIndex;
    public int value;

    public MachineCode()
    {
        isSourceMemoryIndexIsRegister = false;
        isDestMemoryIndexIsRegister = false;
    }

    public void SetData(Register register, VirtualMemory memory, int value, Error error)
    {
        switch (mode)
        {
            case Mode.MemoryToRegister:
            case Mode.RegisterToRegister:
            case Mode.RegisterOnly:
            case Mode.NumberToRegister:
                switch (destRegister)
                {
                    case RegisterIndex.ax:
                        register.ax = value;
                        break;
                    case RegisterIndex.bx:
                        register.bx = value;
                        break;
                    case RegisterIndex.cx:
                        register.cx = value;
                        break;
                    case RegisterIndex.dx:
                        register.dx = value;
                        break;
                    case RegisterIndex.si:
                        register.si = value;
                        break;
                    case RegisterIndex.di:
                        register.di = value;
                        break;
                    case RegisterIndex.bp:
                        register.bp = value;
                        break;
                    case RegisterIndex.sp:
                        register.sp = value;
                        break;
                    case RegisterIndex.ip:
                        register.ip = value;
                        break;
                    default:
                        error.cpuError = CPUError.NotDefinedRegisterError;
                        break;
                }
                break;
            case Mode.MemoryToMemory:
            case Mode.RegisterToMemory:
            case Mode.MemoryOnly:
            case Mode.NumberToMemory:
                if(isSourceMemoryIndexIsRegister)
                {
                    GetDataSource(register, memory, ref sourceMemoryIndex, error);
                }
                if(isDestMemoryIndexIsRegister)
                {
                    GetDataDest(register, memory, ref destMemoryIndex, error);
                }
                memory.SetData(destMemoryIndex, value, error);
                break;
            default:
                error.cpuError = CPUError.NotDefinedRegisterError;
                break;
        }
    }

    public void GetDataDest(Register register, VirtualMemory memory, ref int returnVal, Error error)
    {
        switch (mode)
        {
            case Mode.MemoryToRegister:
            case Mode.RegisterToRegister:
            case Mode.RegisterOnly:
            case Mode.NumberToRegister:
                switch (destRegister)
                {
                    case RegisterIndex.ax:
                        returnVal = register.ax;
                        break;
                    case RegisterIndex.bx:
                        returnVal = register.ax;
                        break;
                    case RegisterIndex.cx:
                        returnVal = register.ax;
                        break;
                    case RegisterIndex.dx:
                        returnVal = register.ax;
                        break;
                    case RegisterIndex.si:
                        returnVal = register.ax;
                        break;
                    case RegisterIndex.di:
                        returnVal = register.ax;
                        break;
                    case RegisterIndex.bp:
                        returnVal = register.ax;
                        break;
                    case RegisterIndex.sp:
                        returnVal = register.ax;
                        break;
                    case RegisterIndex.ip:
                        returnVal = register.ax;
                        break;
                    default:
                        error.cpuError = CPUError.NotDefinedRegisterError;
                        break;
                }
                break;
            case Mode.MemoryToMemory:
            case Mode.RegisterToMemory:
            case Mode.MemoryOnly:
            case Mode.NumberToMemory:
                if (isSourceMemoryIndexIsRegister)
                {
                    GetDataSource(register, memory, ref sourceMemoryIndex, error);
                }
                if (isDestMemoryIndexIsRegister)
                {
                    GetDataDest(register, memory, ref destMemoryIndex, error);
                }
                returnVal = memory.GetData(sourceMemoryIndex, error);
                break;
            case Mode.NumberOnly:
            case Mode.RegisterToNumber:
            case Mode.MemoryToNumber:
                returnVal = value;
                break;
            default:
                returnVal = -1;
                error.cpuError = CPUError.NotDefinedRegisterError;
                break;
        }
    }

    public void GetDataSource(Register register, VirtualMemory memory, ref int returnVal, Error error)
    {
        switch (mode)
        {
            case Mode.RegisterOnly:
            case Mode.RegisterToMemory:
            case Mode.RegisterToRegister:
                switch (destRegister)
                {
                    case RegisterIndex.ax:
                        returnVal = register.ax;
                        break;
                    case RegisterIndex.bx:
                        returnVal = register.ax;
                        break;
                    case RegisterIndex.cx:
                        returnVal = register.ax;
                        break;
                    case RegisterIndex.dx:
                        returnVal = register.ax;
                        break;
                    case RegisterIndex.si:
                        returnVal = register.ax;
                        break;
                    case RegisterIndex.di:
                        returnVal = register.ax;
                        break;
                    case RegisterIndex.bp:
                        returnVal = register.ax;
                        break;
                    case RegisterIndex.sp:
                        returnVal = register.ax;
                        break;
                    case RegisterIndex.ip:
                        returnVal = register.ax;
                        break;
                    default:
                        error.cpuError = CPUError.NotDefinedRegisterError;
                        break;
                }
                break;
            case Mode.MemoryToRegister:
            case Mode.MemoryToMemory:
            case Mode.MemoryOnly:
                returnVal = memory.GetData(sourceMemoryIndex, error);
                break;
            case Mode.NumberToMemory:
            case Mode.NumberToRegister:
            case Mode.NumberOnly:
                returnVal = value;
                break;
            default:
                returnVal = -1;
                error.cpuError = CPUError.NotDefinedRegisterError;
                break;
        }
    }
}


