using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

namespace Game.Control
{
    public class Movement_Dotween : Movement
    {
        [Header("Dotween Ease")]
        [SerializeField] Ease acclerationEase = Ease.OutQuint;
        [SerializeField] Ease deacclerationEase = Ease.OutCubic;
        Tween movingTween;
        Sequence jumpSequence;
        [SerializeField] Ease jumpAccEase = Ease.OutCubic;
        [SerializeField] Ease jumpDeaccEase = Ease.OutQuint;
        [SerializeField] Ease fallEase = Ease.InSine;


        public override void Move()
        {
            if (Input.GetAxisRaw("Horizontal") != 0)
            {
                if (!isStart)
                {
                    movingTween?.Kill();
                    movingTween = DOTween.To(() => velocityX, x => velocityX = x, Input.GetAxisRaw("Horizontal") * maxMovingVelocity, acclerationTime).SetEase(acclerationEase);
                    vCamTransposer.m_ScreenX = 0.5f - Input.GetAxisRaw("Horizontal") * 0.1f * cinemachineLookAhead;
                    animateBody?.SqueezeHorizontal();
                    isStart = true;
                }
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
                velocityY = 0;
                isFalling = false;
            }

            if (!isJumping && !touchGround && !isFalling)
            {
                fallTweener = DOTween.To(() => velocityY, y => velocityY = y, -fallAccleration, Time.fixedDeltaTime)
                    .SetEase(fallEase).SetUpdate(UpdateType.Fixed)
                    .SetLoops(-1,LoopType.Incremental);
                isFalling = true;
            }

        }


        //public static void DoValue(float value , float end , float duration)
        //{
        //    DOTween.To(() => value,x => value = x, end, duration);
        //}




    }

}


