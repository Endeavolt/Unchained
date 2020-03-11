﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAction : MonoBehaviour
{

    [HideInInspector] public GameObject player;

    public GameObject posCamera;

    [Header("Options")] public bool StatiqueCam;

    [Header("Bullet")]
    public float distanceOfStartDezoomBullet = 10;
    public float speedDezoomBullet;

    [Header("Agent")]
    public float distanceOfStartDezoomAgent = 20;
    public float speedDezoomAgent;

    [Header("Zoom")]
    public float speedZoomSpeed;


    [Header("Proposition")]
    public bool decalageScope;
    public float decalageCamera = 0;
    public float speedDecalage = 10;
    public float multiplicateurSpeedMax = 5;
    [Range(0, 1)]
    public float resetLerp = 0.3f;
    private Vector3 previousMousePos;

    public float distanceMax = 100;
    private bool supZero;
    [HideInInspector] public Vector3 ecartJoueur;
    [HideInInspector] public Vector3 basePosition;
    private MouseScope playerMouseScope;
    private EnnemiStock playerEnnemiStock;
    private float distanceBullet;
    private float distanceAgent;
    public float compteurDezoomBullet;
    private float compteurZoomBullet;
    private float competeur;
    public Camera orthoCam;
    public PlayerMoveAlone moveAlone;
    public float pointFocusCame = 1;

    private bool activeBehavior =true;
    private Vector3 currentDir = Vector3.zero;
    private Vector3 rotate;
    public bool newBehavior = true;
    // Start is called before the first frame update
    void Start()
    {
        player = PlayerMoveAlone.Player1;
        moveAlone = player.GetComponent<PlayerMoveAlone>();

        ecartJoueur = transform.position - player.transform.position;
       
        playerMouseScope = player.GetComponent<MouseScope>();
        playerEnnemiStock = player.GetComponent<EnnemiStock>();
        previousMousePos = transform.position;
    }


   public void ResetPos()
    {
        ecartJoueur = transform.position - player.transform.position;
        activeBehavior = false;
    }

    public void Deactive()
    {
        activeBehavior = true;
    }


    void Update()
    {
        orthoCam.orthographicSize = 15 * Vector3.Distance(transform.position, player.transform.position) / 25;

        if (!activeBehavior)
        {
            basePosition = player.transform.position + ecartJoueur;
            transform.position = basePosition;
         
        }

        if (!StatiqueCam)
        {

            if (playerMouseScope.instanceBullet != null || playerEnnemiStock.ennemiStock != null)
            {
                if (playerMouseScope.instanceBullet != null)
                {
                    distanceBullet = Vector3.Distance(player.transform.position, playerMouseScope.instanceBullet.transform.position);
                    if (distanceBullet > distanceOfStartDezoomBullet)
                    {
                        compteurDezoomBullet += Time.deltaTime / speedDezoomBullet;

                        float currentDezoom = Mathf.Clamp(((distanceBullet - 10) / 1.5f), 0, distanceMax);
                        Vector3 camPos = basePosition + -transform.forward * currentDezoom;
                        transform.position = Vector3.Lerp(transform.position, camPos, compteurDezoomBullet);
                    }
                    else
                    {
                        transform.position = Vector3.Lerp(transform.position, basePosition, compteurZoomBullet);
                        compteurZoomBullet += Time.deltaTime;
                    }
                    competeur = 0;
                }
                if (playerEnnemiStock.ennemiStock != null)
                {
                    distanceAgent = Vector3.Distance(player.transform.position, playerEnnemiStock.ennemiStock.transform.position);

                    Vector3 newPos = Vector3.zero;
                    if (!newBehavior)
                    {
                        Vector3 dir = playerEnnemiStock.ennemiStock.transform.position - player.transform.position;
                        if (distanceAgent > distanceOfStartDezoomAgent)
                        {

                            compteurDezoomBullet += Time.deltaTime / speedDezoomAgent;

                            Vector3 camPos = basePosition + -transform.forward * (distanceAgent / 1f);
                            transform.position = Vector3.Lerp(transform.position, camPos, compteurDezoomBullet);
                        }
                        else
                        {
                            compteurZoomBullet += Time.deltaTime;
                            transform.position = Vector3.Lerp(transform.position, basePosition, compteurZoomBullet);

                        }

                        dir = new Vector3(dir.x, 0, dir.z);
                        float dot = Vector3.Dot(dir.normalized, player.transform.forward);
                        pointFocusCame = distanceAgent / 50;
                        pointFocusCame = Mathf.Clamp(pointFocusCame, 0f, 1);

                        newPos = transform.position + dir.normalized * (distanceAgent * (pointFocusCame + dot));
                    }
                    else
                    {
                        if (distanceAgent > distanceOfStartDezoomAgent)
                        {

                            newPos = playerEnnemiStock.ennemiStock.transform.position + -transform.forward * (distanceAgent * 1.5f);
                        }
                        else
                        {
                            newPos = playerEnnemiStock.ennemiStock.transform.position + -transform.forward * (distanceOfStartDezoomAgent * 1.5f);
                        }
                    }
                    transform.position = Vector3.Lerp(transform.position, newPos, competeur);
                    competeur += Time.deltaTime;
                    compteurZoomBullet = 0;

                }

            }
            else
            {
                if (StateAnim.state != StateAnim.CurrentState.Projection)
                {
                    Vector3 checkDir = (playerMouseScope.direction + playerMouseScope.directionManette).normalized;
                    basePosition = player.transform.position + ecartJoueur;
                    Vector3 newPos = basePosition;
                    if (compteurZoomBullet >= 1)
                    {
                        compteurZoomBullet = 0;
                    }

                    if (checkDir != Vector3.zero)
                    {
                        float dirDot = Vector3.Dot(checkDir, currentDir);
                        Debug.DrawRay(player.transform.position, checkDir * 100, Color.blue);
                        if (dirDot < 0.85f)
                        {
                            currentDir = checkDir;
                            compteurZoomBullet = 0;
                        }
                        float orientationOnScreen = Mathf.Abs(Mathf.Clamp(0f, -1f, checkDir.z - 1));
                        newPos = basePosition + (checkDir * decalageCamera * orientationOnScreen);
                        transform.position = Vector3.Lerp(transform.position, newPos, compteurZoomBullet);
                        compteurZoomBullet += Time.deltaTime * speedDecalage;
                    }
                    else
                    {
                        transform.position = newPos;
                    }
                    compteurDezoomBullet = 0;
                    competeur = 0;
                    Debug.DrawRay(player.transform.position, currentDir * 100, Color.green);
                }
                else
                {
                    transform.position = Vector3.Lerp(transform.position, basePosition, compteurZoomBullet);
                    compteurZoomBullet += Time.deltaTime * speedDecalage;
                }



            }
            //float clampHeightZ = Mathf.Clamp(transform.position.z,basePosition.z+ -30 , basePosition.z);
            //float clampHeightY = Mathf.Clamp(transform.position.y, basePosition.y, basePosition.y +20);
            //transform.position = new Vector3(transform.position.x, clampHeightY, clampHeightZ);
        }
    }

}
