using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

namespace Game.Control
{
    public class Movement_Dotween : Movement
    {
        [Header("Horizontal")]
        [SerializeField] float acclerationTime = 1f;
        [SerializeField] float deacclerationTime = 0.4f;
        [Header("Vertical")]
        [SerializeField] float jumpAccTime = 0.1f;
        [SerializeField] float jumpDeaccTime = 0.2f;
        [SerializeField] float fallAccleration = 1f;
        //[SerializeField] float fallAccTime = 1f;

        [Header("Dotween Ease")]
        [SerializeField] Ease acclerationEase = Ease.OutQuint;
        [SerializeField] Ease deacclerationEase = Ease.OutCubic;
        Tween movingTween;
        Sequence jumpSequence;
        Tween fallCamTween;
        [SerializeField] Ease jumpAccEase = Ease.OutCubic;
        [SerializeField] Ease jumpDeaccEase = Ease.OutQuint;
        [SerializeField] Ease fallEase = Ease.InSine;

        public override void Start()
        {
            myRigid.gravityScale = 0f;           
        }



        public override void Move()
        {
            if (Input.GetAxisRaw("Horizontal") != 0)
            {
                if (!isStart)
                {
                    movingTween?.Kill();
                    movingTween = DOTween.To(() => velocityX, x => velocityX = x, Input.GetAxisRaw("Horizontal") * maxMovingVelocity, acclerationTime).SetEase(acclerationEase);
                    
                    animateBody?.SqueezeHorizontal();
                    isStart = true;
                }
                vCamTransposer.m_ScreenX -= Input.GetAxisRaw("Horizontal") * Time.deltaTime * 0.1f * cinemachineLookAhead;
                vCamTransposer.m_ScreenX = Mathf.Clamp(vCamTransposer.m_ScreenX, 0.35f, 0.65f);
            }
            else if (isStart)
            {
                animateBody?.ResetBody();
                movingTween?.Kill();
                movingTween = DOTween.To(() => velocityX, x => velocityX = x, 0f, deacclerationTime).SetEase(deacclerationEase).OnComplete(() =>
                {
                    velocityX = 0f;
                    myRigid.velocity = new Vector2(0f, myRigid.velocity.y);
                    InvokeRefreshUI(0f);
                });
                isStart = false;
            }
        }

        public override void Jump()
        {
            if (Input.GetKeyDown(KeyCode.Space) && touchGround)
            {
                isJumping = true;
                //myRigid.gravityScale = 0f;
                animateBody?.SqueezeVertical(jumpAccTime);
                jumpSequence = DOTween.Sequence();
                jumpSequence.Append(DOTween.To(() => velocityY, y => velocityY = y, maxJumpVelocity, jumpAccTime).SetEase(jumpAccEase).OnComplete(() => animateBody.ResetBody(0.2f)));
                jumpSequence.Append(DOTween.To(() => velocityY, y => velocityY = y, 0f, jumpDeaccTime).SetEase(jumpDeaccEase));
                jumpSequence.OnComplete(() =>
                {
                    isJumping = false;
                });

            }
        }

        public override void Fall()
        {
            if (touchGround && fallTweener.IsActive() && isFalling)
            {               
                fallTweener.Kill();
                fallCamTween.Kill();
                velocityY = 0;
                vCamTransposer.m_ScreenY = 0.6f;
                isFalling = false;
            }

            if (!isJumping && !touchGround && !isFalling)
            {
                fallTweener = DOTween.To(() => velocityY, y => velocityY = y, -fallAccleration, Time.fixedDeltaTime)
                    .SetEase(fallEase).SetUpdate(UpdateType.Fixed)
                    .SetLoops(-1,LoopType.Incremental);

                fallCamTween = DOTween.To(() => vCamTransposer.m_ScreenY, y => vCamTransposer.m_ScreenY = y, 0.4f, 0.5f)
                    .SetEase(fallEase);
                isFalling = true;
                
            }

        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (velocityX > 0.01f || velocityX < -0.01f)
            {
                myRigid.velocity = new Vector2(velocityX, myRigid.velocity.y);
                InvokeRefreshUI(Mathf.Abs(velocityX) / maxMovingVelocity);
            }

            if (velocityY > 0.01f || velocityY < -0.01f)
            {
                myRigid.velocity = new Vector2(myRigid.velocity.x, velocityY);
            }
        }



        //public static void DoValue(float value , float end , float duration)
        //{
        //    DOTween.To(() => value,x => value = x, end, duration);
        //}




    }

}


