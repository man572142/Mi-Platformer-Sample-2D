using Cinemachine;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Control
{
    [RequireComponent(typeof(Rigidbody2D))]
    public abstract class Movement : MonoBehaviour
    {
        protected Rigidbody2D myRigid = null;
        
        Collider2D boxCollider = null;

        
        [SerializeField] protected float maxMovingVelocity = 10f;
        [SerializeField] protected float maxJumpVelocity = 5f;

        protected bool isStart = false;
        protected float velocityX = 0f;

        
        
        [SerializeField] LayerMask groundLayer;
        protected Tween fallTweener;
        protected float velocityY = 0f;
        protected bool isJumping = false;
        protected bool isFalling = false;

        [Header("Camera")]
        [SerializeField] CinemachineVirtualCamera vCam = null;
        protected CinemachineFramingTransposer vCamTransposer;
        [SerializeField] protected float cinemachineLookAhead = 1f;
        public event Action<float> OnRefreshUI;
        [SerializeField] protected AnimateBody animateBody = null;



        protected bool isGrounded = false;
        protected RaycastHit2D touchGround;

        private void Awake()
        {
            myRigid = GetComponent<Rigidbody2D>();
            boxCollider = GetComponent<Collider2D>();
            vCamTransposer = vCam.GetCinemachineComponent<CinemachineFramingTransposer>();
            vCam.Follow = transform;
        }

        public virtual void Start()
        {
            myRigid.gravityScale = 1f;
        }

        public virtual void Update()
        {
            Move();
            Jump();

            // TODO: Edge bug fix
            touchGround = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size,0f,Vector2.down, 0.01f, groundLayer);
            if (touchGround)
            {
                //Debug.DrawRay(boxCollider.bounds.center, Vector2.down * (boxCollider.bounds.extents.y + 0.05f), Color.green);
                animateBody?.SetAirColor(false);
            }
            else
            {
                animateBody?.SetAirColor(true);
            }
        }



        public virtual void FixedUpdate()
        {           
            Fall();
        }

        public abstract void Move();
        public abstract void Jump();
        public abstract void Fall();

        public void InvokeRefreshUI(float value)
        {
            OnRefreshUI?.Invoke(value);
        }

    }
}


