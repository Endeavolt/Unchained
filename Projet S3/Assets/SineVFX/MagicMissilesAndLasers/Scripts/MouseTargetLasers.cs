﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseTargetLasers : MonoBehaviour {

    public Transform laserShotPosition;
    public float speed = 1f;
    public ParticleSystem startWavePS;
    public ParticleSystem startParticles;
    public int startParticlesCount = 100;
    public GameObject laserShotPrefab;

    public Vector3 mouseWorldPosition;
    public Animator anim;

    public bool isTesting;

    void Start () 
    {
        anim = GetComponent<Animator>();
        if(!isTesting)
        {
            gameObject.SetActive(false);
        }
        else
        {
            startWavePS.Emit(1);
            startParticles.Emit(startParticlesCount);

        }


    }

    private void Update()
    {
        if(isTesting)
        {
            anim.SetBool("Fire", true);
        }
            //if (Input.GetMouseButtonDown(0))
            //{
            //    startWavePS.Emit(1);
            //    startParticles.Emit(startParticlesCount);
            //}
            //
            //if (Input.GetMouseButton(0))
            //{
            //    anim.SetBool("Fire", true);
            //}
            //else
            //{
            //    anim.SetBool("Fire", false);
            //}
            //
            //if (Input.GetMouseButtonDown(1))
            //{
            //    anim.SetBool("Fire", true);
            //    startWavePS.Emit(1);
            //    startParticles.Emit(startParticlesCount);
            //    Instantiate(laserShotPrefab, laserShotPosition.position, transform.rotation);
            //}
    }

    // Raycasting and positioning Cursor GameObject at Collision point
    void FixedUpdate () {

        //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //RaycastHit hit;
        //
        //if (Physics.Raycast(ray, out hit))
        //{
        //    mouseWorldPosition = hit.point;
        //}
        if (isTesting)
        {
            mouseWorldPosition = PlayerMoveAlone.playerPos;
        }
        Quaternion toRotation = Quaternion.LookRotation(mouseWorldPosition - transform.position);
        transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, speed * Time.deltaTime);

    }
}
