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

    public void Clear()
    {
        ax = 0;
        bx = 0;
        cx = 0;
        dx = 0;
        si = 0;
        di = 0;
        bp = 0;
        sp = 0;
        ip = 0;
        zf = false;
        sf = false;
        of = false;
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

public enum CPUMode
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
        if (cpuError != CPUError.Nothing &&
           memoryError != MemoryError.Nothing)
            return true;

        return false;
    }

    public void Clear()
    {
        cpuError = CPUError.Nothing;
        memoryError = MemoryError.Nothing;
    }
}

public class MachineCode
{
    public Instructions instruction;
    public CPUMode mode;
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
            case CPUMode.MemoryToRegister:
            case CPUMode.RegisterToRegister:
            case CPUMode.RegisterOnly:
            case CPUMode.NumberToRegister:
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
            case CPUMode.MemoryToMemory:
            case CPUMode.RegisterToMemory:
            case CPUMode.MemoryOnly:
            case CPUMode.NumberToMemory:
                if(isDestMemoryIndexIsRegister)
                {
                    switch (destRegister)
                    {
                        case RegisterIndex.ax:
                            destMemoryIndex = register.ax;
                            break;
                        case RegisterIndex.bx:
                            destMemoryIndex = register.bx;
                            break;
                        case RegisterIndex.cx:
                            destMemoryIndex = register.cx;
                            break;
                        case RegisterIndex.dx:
                            destMemoryIndex = register.dx;
                            break;
                        case RegisterIndex.si:
                            destMemoryIndex = register.si;
                            break;
                        case RegisterIndex.di:
                            destMemoryIndex = register.di;
                            break;
                        case RegisterIndex.bp:
                            destMemoryIndex = register.bp;
                            break;
                        case RegisterIndex.sp:
                            destMemoryIndex = register.sp;
                            break;
                        case RegisterIndex.ip:
                            destMemoryIndex = register.ip;
                            break;
                        default:
                            error.cpuError = CPUError.NotDefinedRegisterError;
                            break;
                    }
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
            case CPUMode.MemoryToRegister:
            case CPUMode.RegisterToRegister:
            case CPUMode.RegisterOnly:
            case CPUMode.NumberToRegister:
                switch (destRegister)
                {
                    case RegisterIndex.ax:
                        returnVal = register.ax;
                        break;
                    case RegisterIndex.bx:
                        returnVal = register.bx;
                        break;
                    case RegisterIndex.cx:
                        returnVal = register.cx;
                        break;
                    case RegisterIndex.dx:
                        returnVal = register.dx;
                        break;
                    case RegisterIndex.si:
                        returnVal = register.si;
                        break;
                    case RegisterIndex.di:
                        returnVal = register.di;
                        break;
                    case RegisterIndex.bp:
                        returnVal = register.bp;
                        break;
                    case RegisterIndex.sp:
                        returnVal = register.sp;
                        break;
                    case RegisterIndex.ip:
                        returnVal = register.ip;
                        break;
                    default:
                        error.cpuError = CPUError.NotDefinedRegisterError;
                        break;
                }
                break;
            case CPUMode.MemoryToMemory:
            case CPUMode.RegisterToMemory:
            case CPUMode.MemoryOnly:
            case CPUMode.NumberToMemory:
                if (isSourceMemoryIndexIsRegister || isDestMemoryIndexIsRegister)
                {
                    switch (destRegister)
                    {
                        case RegisterIndex.ax:
                            destMemoryIndex = register.ax;
                            break;
                        case RegisterIndex.bx:
                            destMemoryIndex = register.bx;
                            break;
                        case RegisterIndex.cx:
                            destMemoryIndex = register.cx;
                            break;
                        case RegisterIndex.dx:
                            destMemoryIndex = register.dx;
                            break;
                        case RegisterIndex.si:
                            destMemoryIndex = register.si;
                            break;
                        case RegisterIndex.di:
                            destMemoryIndex = register.di;
                            break;
                        case RegisterIndex.bp:
                            destMemoryIndex = register.bp;
                            break;
                        case RegisterIndex.sp:
                            destMemoryIndex = register.sp;
                            break;
                        case RegisterIndex.ip:
                            destMemoryIndex = register.ip;
                            break;
                        default:
                            error.cpuError = CPUError.NotDefinedRegisterError;
                            break;
                    }
                }

                returnVal = memory.GetData(destMemoryIndex, error);
                break;
            case CPUMode.NumberOnly:
            case CPUMode.RegisterToNumber:
            case CPUMode.MemoryToNumber:
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
            case CPUMode.RegisterOnly:
            case CPUMode.RegisterToMemory:
            case CPUMode.RegisterToRegister:
                switch (destRegister)
                {
                    case RegisterIndex.ax:
                        returnVal = register.ax;
                        break;
                    case RegisterIndex.bx:
                        returnVal = register.bx;
                        break;
                    case RegisterIndex.cx:
                        returnVal = register.cx;
                        break;
                    case RegisterIndex.dx:
                        returnVal = register.dx;
                        break;
                    case RegisterIndex.si:
                        returnVal = register.si;
                        break;
                    case RegisterIndex.di:
                        returnVal = register.di;
                        break;
                    case RegisterIndex.bp:
                        returnVal = register.bp;
                        break;
                    case RegisterIndex.sp:
                        returnVal = register.sp;
                        break;
                    case RegisterIndex.ip:
                        returnVal = register.ip;
                        break;
                    default:
                        error.cpuError = CPUError.NotDefinedRegisterError;
                        break;
                }
                break;
            case CPUMode.MemoryToRegister:
            case CPUMode.MemoryToMemory:
            case CPUMode.MemoryOnly:
                if (isSourceMemoryIndexIsRegister || isDestMemoryIndexIsRegister)
                {
                    switch (sourceRegister)
                    {
                        case RegisterIndex.ax:
                            sourceMemoryIndex = register.ax;
                            break;
                        case RegisterIndex.bx:
                            sourceMemoryIndex = register.bx;
                            break;
                        case RegisterIndex.cx:
                            sourceMemoryIndex = register.cx;
                            break;
                        case RegisterIndex.dx:
                            sourceMemoryIndex = register.dx;
                            break;
                        case RegisterIndex.si:
                            sourceMemoryIndex = register.si;
                            break;
                        case RegisterIndex.di:
                            sourceMemoryIndex = register.di;
                            break;
                        case RegisterIndex.bp:
                            sourceMemoryIndex = register.bp;
                            break;
                        case RegisterIndex.sp:
                            sourceMemoryIndex = register.sp;
                            break;
                        case RegisterIndex.ip:
                            sourceMemoryIndex = register.ip;
                            break;
                        default:
                            error.cpuError = CPUError.NotDefinedRegisterError;
                            break;
                    }
                }

                returnVal = memory.GetData(sourceMemoryIndex, error);
                break;
            case CPUMode.NumberToMemory:
            case CPUMode.NumberToRegister:
            case CPUMode.NumberOnly:
                returnVal = value;
                break;
            default:
                returnVal = -1;
                error.cpuError = CPUError.NotDefinedRegisterError;
                break;
        }
    }
}


