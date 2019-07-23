using UnityEngine;
using System.Collections;

public class Sensor_Touch : VirtualFunction
{
    //pin0 collision!

    private void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    private void OnCollisionEnter(Collision collision)
    {
        int[] temp = { 1 };
        
        bus.WriteBus(1, temp);
    }

    private void OnCollisionExit(Collision collision)
    {
        int[] temp = { 0 };

        bus.WriteBus(1, temp);
    }
}
