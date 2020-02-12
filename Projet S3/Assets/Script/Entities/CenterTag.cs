﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterTag : MonoBehaviour
{
    public enum Types { Blue, Orange, Violet }
    public Types centerTypes;
    public GameObject[] centerVFX;
    public bool finVfxSeries = false;
    // Start is called before the first frame update
    void Start()
    {
        if (!finVfxSeries)
        {
            if (centerTypes == Types.Blue)
            {

                Instantiate(centerVFX[0], transform.position, transform.rotation);
            }
            else if (centerTypes == Types.Orange)
            {
                Instantiate(centerVFX[1], transform.position, transform.rotation);
            }
            else if (centerTypes == Types.Violet)
            {
                Instantiate(centerVFX[2], transform.position, transform.rotation);
            }
        }
        else if (finVfxSeries)
        {
            if (centerTypes == Types.Blue)
            {

                Instantiate(centerVFX[3], transform.position, transform.rotation);
            }
            else if (centerTypes == Types.Orange)
            {
                Instantiate(centerVFX[4], transform.position, transform.rotation);
            }
            else if (centerTypes == Types.Violet)
            {
                Instantiate(centerVFX[5], transform.position, transform.rotation);
            }
        }



    }

    // Update is called once per frame
    void Update()
    {
        
    }
}