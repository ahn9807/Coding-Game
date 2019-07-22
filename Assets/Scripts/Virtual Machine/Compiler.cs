using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Compiler 
{
    public CompileError error;

    bool endContained;

    public bool Compile(string rawData, ref List<MachineCode> mc, ref int errorLine)
    {
        error.Clear();

        List<MachineCode> machineCodes = new List<MachineCode>();
        string[] inputStringsByLine = rawData.Split(Environment.NewLine.ToCharArray());

        int index = 0;
        machineCodes.Capacity = inputStringsByLine.Length;

        Debug.Log(rawData);
        Debug.Log(inputStringsByLine.Length);

        while (index < inputStringsByLine.Length)
        { 
            int commentIndex = inputStringsByLine[index].IndexOf("//");
            if (commentIndex > 0)
                inputStringsByLine[index] = inputStringsByLine[index].Substring(0, commentIndex);
             

            string[] subStrings = inputStringsByLine[index].Split(' ');
            string s1 = "", s2 = "", s3 = "";
            if (subStrings.Length == 1)
            {
                s1 = subStrings[0];
                if (s1 == "END")
                    endContained = true;
                if (s1 == "")
                    continue;
            }
            else if(subStrings.Length == 2)
            {
                s1 = subStrings[0];
                s2 = subStrings[1];
            }
            else 
            {
                s1 = subStrings[0];
                s2 = subStrings[1];
                s3 = subStrings[2];
            }

            machineCodes.Add(new MachineCode());
            SetMachineCode(machineCodes[index], s1, s2, s3);

            
            if(error.IsError())
            {
                Debug.Log("Compile Error Line:" + index);
                errorLine = index;
                mc = null;
                return false;
            }
            
            index++;
        }

        /*
        if(endContained == false)
        {
            Debug.Log("Compile Error");
            error.NotContainEND = true;
            errorLine = -1;
            mc = null;
            return false;
        }
        */

        errorLine = -1;
        mc = machineCodes;
        return true;
    }

    private void SetMachineCode(MachineCode mc, string instruction, string input1, string input2)
    {
        //setting instruction
        error.NotDefinedInstruction |= !Enum.TryParse(instruction.ToUpper(), out mc.instruction);

        //setting mode and following settings
        CompileType type1 = GetType(input1);
        CompileType type2 = GetType(input2);

        if (type1 == CompileType.Register && type2 == CompileType.Register)
        {
            mc.mode = CPUMode.RegisterToRegister;
            mc.sourceRegister = GetRegisterIndex(input1);
            mc.destRegister = GetRegisterIndex(input2);
        }
        else if (type1 == CompileType.Register && type2 == CompileType.Memory)
        {
            mc.mode = CPUMode.RegisterToMemory;
            mc.sourceRegister = GetRegisterIndex(input1);
            SetMemoryIndex(mc, input2, false);

        }
        else if(type1 == CompileType.Register && type2 == CompileType.Number)
        {
            mc.mode = CPUMode.RegisterToNumber;
            mc.sourceRegister = GetRegisterIndex(input1);
            mc.value = GetValue(input2);
        }
        else if(type1 == CompileType.Memory && type2 == CompileType.Number)
        {
            mc.mode = CPUMode.MemoryToNumber;
            SetMemoryIndex(mc, input1, true);
        }
        else if (type1 == CompileType.Memory && type2 == CompileType.Register)
        {
            mc.mode = CPUMode.MemoryToRegister;
            mc.destRegister = GetRegisterIndex(input2);
            SetMemoryIndex(mc, input1, true);
        }
        else if (type1 == CompileType.Memory && type2 == CompileType.Memory)
        {
            mc.mode = CPUMode.MemoryToMemory;
            SetMemoryIndex(mc, input1, true);
            SetMemoryIndex(mc, input2, false);
        }
        else if (type1 == CompileType.Memory && type2 == CompileType.Noting)
        {
            mc.mode = CPUMode.MemoryOnly;
            SetMemoryIndex(mc, input1, true);
        }
        else if (type1 == CompileType.Register && type2 == CompileType.Noting)
        {
            mc.mode = CPUMode.RegisterOnly;
            mc.sourceRegister = GetRegisterIndex(input1);
        }
        else if (type1 == CompileType.Number && type2 == CompileType.Register)
        {
            mc.mode = CPUMode.NumberToRegister;
            mc.value = GetValue(input1);
            mc.destRegister = GetRegisterIndex(input2);
        }
        else if (type1 == CompileType.Number && type2 == CompileType.Memory)
        {
            mc.mode = CPUMode.NumberToMemory;
            mc.value = GetValue(input1);
            SetMemoryIndex(mc, input2, false);
        }
        else if (type1 == CompileType.Number && type2 == CompileType.Noting)
        {
            mc.mode = CPUMode.NumberOnly;
            mc.value = GetValue(input1);
        }
        else if (type1 == CompileType.Noting && type2 == CompileType.Noting)
        {
            mc.mode = CPUMode.Noting;
        }
        else
        {
            error.SetModeError = true;
            mc.mode = CPUMode.Noting;
        }
    }

    private CompileType GetType(string input)
    {
        if(input == "")
        {
            return CompileType.Noting;
        }

        if(input.Contains("M") || input.Contains("m"))
        {
            return CompileType.Memory;
        }

        if(int.TryParse(input, out int n))
        {
            return CompileType.Number;
        }

        return CompileType.Register;
    }

    private RegisterIndex GetRegisterIndex(string input)
    {
        input = input.ToLower();


        if (!Enum.TryParse<RegisterIndex>(input, out RegisterIndex ri))
        {
            error.NotDefinedRegister = true;
            return RegisterIndex.ax;
        }

        return ri;
    }

    private void SetMemoryIndex(MachineCode mc, string input, bool isSource)
    {
        int val = 0;
        string sval = "";
        bool isNumber = false;

        Char[] subChar = input.ToCharArray();
        string subString = "";
        for(int i=0;i<subChar.Length;i++)
        {
            if(subChar[i] != 'm' && subChar[i] != 'M' && subChar[i] != ']' && subChar[i] != '[')
            {
                subString += subChar[i];
            }
        }

        CompileType type = GetType(subString);

        if(type == CompileType.Number)
        {
            int.TryParse(subString, out val);
            isNumber = true;
        }

        if(type == CompileType.Register)
        {
            sval = subString;
            isNumber = false;
        }

        if(isNumber)
        {
            if(isSource)
            {
                mc.sourceMemoryIndex = val;
            }
            else
            {
                mc.destMemoryIndex = val;
            }
        }
        else
        {
            if(isSource)
            {
                mc.isSourceMemoryIndexIsRegister = true;
                mc.sourceRegister = GetRegisterIndex(sval);
            }
            else
            {
                mc.isDestMemoryIndexIsRegister = true;
                mc.destRegister = GetRegisterIndex(sval);
            }
        }
    }

    private int GetValue(string input)
    {
        error.InvaildNumberFormat |= !int.TryParse(input, out int returnVal);

        return returnVal;
    }

    public void Clear()
    {
        error.Clear();
    }
}

public struct CompileError
{
    public bool NotDefinedInstruction;
    public bool NotDefinedRegister;
    public bool InvalidMemoryFormat;
    public bool InvaildNumberFormat;
    public bool SetModeError;
    public bool NotContainEND;

    public bool IsError()
    {
        return NotDefinedInstruction |
               NotDefinedRegister |
               InvalidMemoryFormat |
               InvaildNumberFormat |
               NotContainEND |
               SetModeError;
    }

    public void Clear()
    {
        NotDefinedInstruction = false;
        NotDefinedRegister = false;
        InvaildNumberFormat = false;
        InvaildNumberFormat = false;
        SetModeError = false;
        NotContainEND = false;
    }
}

public enum CompileType
{
    Noting,
    Instruction,
    Register,
    Memory,
    Number,
}
