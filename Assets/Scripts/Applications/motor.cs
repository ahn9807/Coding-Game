using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Motor : VirtualFunction
{
    //pin0 up pin1 down pin2 left pin3 right
    int[] result;

    private void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        result = bus.AccessBus(4);

        if(result[0] != 0)
        {
            transform.position += Vector3.up * Time.fixedDeltaTime * result[0];
        }
        if(result[1] !=0 )
        {
            transform.position += Vector3.down * Time.fixedDeltaTime * result[1];
        }
        if (result[2] != 0)
        {
            transform.position += Vector3.right * Time.fixedDeltaTime * result[2];
        }
        if (result[3] != 0)
        {
            transform.position += Vector3.left * Time.fixedDeltaTime * result[3];
        }
    }
}
