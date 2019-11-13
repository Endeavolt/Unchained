﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerMassMob : MonoBehaviour
{
    public float timeOfSpawn;
    public float compteur;
    public GameObject objectToInstantiate;
    public GameObject parentToSpawn;
    public GameObject target;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        SpawnObject();
    }
    void SpawnObject()
    {
        if (compteur > timeOfSpawn)
        {
            GameObject add = Instantiate(objectToInstantiate, transform.position, transform.rotation, parentToSpawn.transform);
            add.GetComponent<EnnemiBehaviorMassMob>().target = target;
            compteur = 0;
        }
        else
        {
            compteur += Time.deltaTime;
        }

    }
}
