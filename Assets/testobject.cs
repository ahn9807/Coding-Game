﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testobject : MonoBehaviour
{
    public VirtualOS vos;
    Rigidbody rigidbody;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }
    // Update is called once per frame
    void Update()
    {
        if(vos.AccessMemory(0) == 1)
        {
            transform.position = Vector3.up;
        }
        if(vos.AccessMemory(1) == 1)
        {
            transform.position = Vector3.down;
        }
    }
}
