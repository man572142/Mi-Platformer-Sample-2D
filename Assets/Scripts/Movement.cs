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

        [Header("Horizontal")]
        [SerializeField] protected float maxMovingVelocity = 10f;
        [SerializeField] protected float acclerationTime = 1f;
        [SerializeField] protected float deacclerationTime = 0.4f;
        protected bool isStart = false;
        protected float velocityX = 0f;

        [Header("Vertical")]
        [SerializeField] protected float maxJumpVelocity = 5f;
        [SerializeField] protected float jumpAccTime = 0.1f;
        [SerializeField] protected float jumpDeaccTime = 0.2f;
        [SerializeField] protected float fallAccleration = 1f;
        [SerializeField] protected float fallAccTime = 1f;
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

        protected virtual void Awake()
        {
            myRigid = GetComponent<Rigidbody2D>();
            boxCollider = GetComponent<Collider2D>();
            vCamTransposer = vCam.GetCinemachineComponent<CinemachineFramingTransposer>();
        }

        public virtual void Start()
        {

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
            if (velocityX > 0.01f || velocityX < -0.01f)
            {
                myRigid.velocity = new Vector2(velocityX, myRigid.velocity.y);
                InvokeRefreshUI(Mathf.Abs(velocityX) / maxMovingVelocity);
            }

            if (velocityY > 0.01f || velocityY < -0.01f)
            {
                myRigid.velocity = new Vector2(myRigid.velocity.x, velocityY);
            }
            Fall();
        }

        public virtual void Move()
        {

        }
        public virtual void Jump()
        {

        }

        public virtual void Fall()
        {

        }

        public void InvokeRefreshUI(float value)
        {
            OnRefreshUI?.Invoke(value);
        }

    }
}


