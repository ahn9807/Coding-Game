using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirtualBUS
{
    public VirtualOS os;
    public List<int> pins;

    public VirtualBUS(VirtualOS os)
    {
        this.os = os;
        this.pins = new List<int>();
    }

    public bool AddPin(int index)
    {
        if(os.GetHardware().memoryCapacity < index)
        {
            return false;
        }
        if(pins.Contains(index))
        {
            return false;
        }

        pins.Add(index);
        return true;
    }

    public void DeletePin(int index)
    {
        pins.Remove(index);
    }

    public int[] AccessBus(int size)
    {
        if(size < pins.Count)
        {
            size = pins.Count;
        }

        int[] returnVal = new int[size];
        for (int i = 0; i < pins.Count; i++)
        {
            returnVal[i] = os.AccessMemory(pins[i]);
        }

        return returnVal;
    }

    public void WriteBus(int size, int[] value)
    {
        if(size < pins.Count)
        {
            size = pins.Count;
        }
    }
}
