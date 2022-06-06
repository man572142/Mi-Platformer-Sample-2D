using Cinemachine;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Movement : MonoBehaviour
{
    [SerializeField] protected Rigidbody2D myRigid = null;
    protected float velocityX = 0f;
    protected float velocityY = 0f;
    [SerializeField] protected float maxMovingVelocity = 10f;
    [SerializeField] protected float acclerationTime = 1f;
    [SerializeField] protected float deacclerationTime = 0.4f;
    protected bool isStart = false;
    [SerializeField] CinemachineVirtualCamera vCam = null;
    
    
    protected CinemachineFramingTransposer vCamTransposer;
    [SerializeField] protected float cinemachineLookAhead = 1f;
    protected AnimateBody animateBody = null;

    public event Action<float> OnRefreshUI;

    protected bool isJumping = false;
    [SerializeField] protected float maxJumpVelocity = 5f;
    [SerializeField] protected float jumpAccTime = 0.1f;
    [SerializeField] protected float jumpDeaccTime = 0.2f;
    [SerializeField] protected float fallAccTime = 1f;


    protected virtual void Awake()
    {
        animateBody = GetComponent<AnimateBody>();
        vCamTransposer = vCam.GetCinemachineComponent<CinemachineFramingTransposer>();
    }
    
    public void InvokeRefreshUI(float value)
    {
        OnRefreshUI?.Invoke(value);
    }

    public virtual void Start()
    {

    }

    public virtual void Update()
    {
        //¥ÎLerp
    }

    public virtual void FixedUpdate()
    {

    }

}
