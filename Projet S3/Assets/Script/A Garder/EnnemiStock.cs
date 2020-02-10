﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemiStock : MonoBehaviour
{

    public GameObject ennemiStock;
    [HideInInspector] public Vector3 pos;
    private Klak.Motion.SmoothFollow mySmoothFollow;
    private LineRenderer lineRenderer;
    private bool rotate;
    private bool slam;
    private RotationPlayer rotationPlayer;
    
     public float powerOfProjection;
    [Header("Sound")]
    [FMODUnity.EventRef]
    public string contact;
    private FMOD.Studio.EventInstance contactSound;
    private bool startBool;
    public float volume = 20;
    [Header("Retour Sound")]
    [FMODUnity.EventRef]
    public string OrbitSound;
    private FMOD.Studio.EventInstance OrbitEvent;
    public float OrbitVolume = 10;

    [HideInInspector] public bool onHitEnter;
    public GameObject onHitEnemy;
    private Material enemyStockMat;
    private Texture ennemyStockTextChange;
    private Color baseColor;
    float myFOV;
    bool isOnZoom = false;
 
    private MouseScope mouse;
    [HideInInspector] public bool inputNeed;

    public AnimationCurve curveVolumeOrbitation;
    float tempsEcoule;
    // Start is called before the first frame update
    void Start()
    {

        mouse = GetComponent<MouseScope>();
        mySmoothFollow = GetComponent<Klak.Motion.SmoothFollow>();
        myFOV = Camera.main.fieldOfView;
        rotationPlayer = GetComponent<RotationPlayer>();
       
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, transform.position);
        //Sound
        contactSound = FMODUnity.RuntimeManager.CreateInstance(contact);
        contactSound.setVolume(volume);
        OrbitEvent = FMODUnity.RuntimeManager.CreateInstance(OrbitSound);


    }

    // Update is called once per frame
    void Update()
    {
        tempsEcoule += Time.deltaTime;

        if (tempsEcoule > 1.2)
        {
            tempsEcoule = 0;
        }
        OrbitEvent.setVolume(curveVolumeOrbitation.Evaluate(tempsEcoule));
        Camera.main.fieldOfView = myFOV;
        float input = Input.GetAxis("Attract1");

        contactSound.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
       
        if (ennemiStock != null)
        {

            if (onHitEnter)
            {
               
                if (StateAnim.state == StateAnim.CurrentState.Tir)
                {
                    StateAnim.ChangeState(StateAnim.CurrentState.Rotate);
                }
                isOnZoom = true;
                Instantiate(onHitEnemy, ennemiStock.transform.position, transform.rotation /*, ennemiStock.transform */);
                baseColor = ennemiStock.gameObject.GetComponent<Renderer>().material.color;
                ennemiStock.gameObject.GetComponent<Renderer>().material.color = Color.red;
                onHitEnter = false;
               
                if (mouse.lastInput)
                {
                    if (ennemiStock.tag == "wall")
                    {
                        rotate = rotationPlayer.StartRotationWall(gameObject, pos,ennemiStock, powerOfProjection, false);

                    }
                    else
                    {
                        rotate = rotationPlayer.StartRotation(gameObject, ennemiStock, "Player", powerOfProjection, false);

                    }
                }
                else
                {
                    if (ennemiStock.tag == "wall")
                    {
                        rotate = rotationPlayer.StartRotationWall(gameObject, pos, ennemiStock, powerOfProjection, true);

                    }
                    else
                    {
                        rotate = rotationPlayer.StartRotation(gameObject, ennemiStock, "Player", powerOfProjection, true);

                    }

                }
                if (ennemiStock.gameObject.GetComponent<EnnemiBehavior>())
                {
                    ennemiStock.gameObject.GetComponent<EnnemiBehavior>().imStock = true;

                }
                contactSound.start();

            }
            

            if (isOnZoom)
            {
                zoomOnHit();
            }
            FMOD.Studio.PLAYBACK_STATE orbitState;
            OrbitEvent.getPlaybackState(out orbitState);
            if (orbitState != FMOD.Studio.PLAYBACK_STATE.PLAYING)
            {
                OrbitEvent.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
                OrbitEvent.start();
            }
            mySmoothFollow.target = ennemiStock.gameObject.transform;



            if (!Input.GetKey(KeyCode.Mouse1) && !Input.GetKey(KeyCode.Mouse0) && input == 0 )
            {
                // myRE.Emit();
                DetachPlayer();
            }

        }
        else
        {

            rotate = false;
            slam = false;
            startBool = false;

        }
    }


    public void DetachPlayer()
    {
        myFOV = 70;
        isOnZoom = false;
        if (ennemiStock.gameObject.GetComponent<EnnemiBehavior>())
        {
            ennemiStock.GetComponent<EnnemiBehavior>().imStock = false;
           
        }
        mySmoothFollow.target = null;
        ennemiStock.gameObject.GetComponent<Renderer>().material.color = baseColor;
        if (ennemiStock.tag == "wall")
        {
            rotationPlayer.StopRotation(false);
        }
        else
        {
            rotationPlayer.StopRotation(true);

        }
        ennemiStock = null;
        OrbitEvent.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
    }

    public void zoomOnHit()
    {
        if (myFOV == 70)
        {
            myFOV = 90;
        }
        if (myFOV > 70)
        {
            myFOV -= Time.deltaTime * 20;
        }
        else if (myFOV < 70)
        {
            myFOV = 70;
            isOnZoom = false;
        }
    }
    public void StopRotate()
    {
        rotate = false;
        ennemiStock = null;
    }
    public void StopSlam()
    {
        slam = false;
        ennemiStock = null;
    }
}
