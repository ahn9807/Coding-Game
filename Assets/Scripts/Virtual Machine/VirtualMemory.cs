using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirtualMemory 
{
    public int size;
    public int[] memory;

    public VirtualMemory(int size)
    {
        this.size = size;
        memory = new int[size];
    }


    public void SetData(int index, int data, Error error)
    {
        if(index > size)
        {
            error.memoryError = MemoryError.IndexOutOfRange;
            return;
        }

        memory[index] = data;
    }   

    public int GetData(int index, Error error)
    {
        if(index > size)
        {
            error.memoryError = MemoryError.IndexOutOfRange;
            return -1;
        }

        return memory[index];
    }
}
