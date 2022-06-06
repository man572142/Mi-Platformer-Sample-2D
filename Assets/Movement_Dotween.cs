using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class Movement_Dotween : Movement
{
    [SerializeField] Ease acclerationEase = Ease.OutQuint;
    [SerializeField] Ease deacclerationEase = Ease.OutCubic;
    Tween movingTween;
    Sequence jumpSequence;
    [SerializeField] Ease jumpAccEase = Ease.OutCubic;
    [SerializeField] Ease jumpDeaccEase = Ease.OutQuint;
    [SerializeField] Ease fallEase = Ease.InSine;

    // Update is called once per frame
    public override void Update()
    {
        MoveHorizontal();
        Jump();
    }

        private void MoveHorizontal()
    {
        if (Input.GetAxisRaw("Horizontal") != 0)
        {
            if (!isStart)
            {
                movingTween?.Kill();
                movingTween = DOTween.To(() => velocityX, x => velocityX = x, Input.GetAxisRaw("Horizontal") * maxMovingVelocity, acclerationTime).SetEase(acclerationEase);
                vCamTransposer.m_ScreenX = 0.5f - Input.GetAxisRaw("Horizontal") * 0.1f * cinemachineLookAhead;
                animateBody.SqueezeHorizontal();
                isStart = true;
            }
        }
        else if (isStart)
        {
            animateBody.ResetBody();
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

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isJumping)
        {
            isJumping = true;
            animateBody.SqueezeVertical(jumpAccTime);
            jumpSequence = DOTween.Sequence();
            jumpSequence.Append(DOTween.To(() => velocityY, y => velocityY = y, maxJumpVelocity, jumpAccTime).SetEase(jumpAccEase).OnComplete(() => animateBody.ResetBody(0.2f)));
            jumpSequence.Append(DOTween.To(() => velocityY, y => velocityY = y, 0f, jumpDeaccTime).SetEase(jumpDeaccEase));
            jumpSequence.Append(DOTween.To(() => velocityY, y => velocityY = y, -maxJumpVelocity, fallAccTime).SetEase(fallEase));
            jumpSequence.OnComplete(() =>
            {
                velocityY = 0;
                isJumping = false;
            });

        }
    }



    public override void FixedUpdate()
    {
        if (velocityX > 0.1f || velocityX < -0.1f)
        {
            myRigid.velocity = new Vector2(velocityX, myRigid.velocity.y);
            InvokeRefreshUI(Mathf.Abs(velocityX) / maxMovingVelocity);
        }
        
        if (velocityY > 0.01f || velocityY  < -0.01f)
        {
            myRigid.velocity = new Vector2(myRigid.velocity.x, velocityY);
        }
    }


    //public static void DoValue(float value , float end , float duration)
    //{
    //    DOTween.To(() => value,x => value = x, end, duration);
    //}




}
