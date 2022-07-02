using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Control
{
    public class Movement_UnityDefault : Movement
    {

        public override void Move()
        {
            if(Input.GetAxis("Horizontal") != 0)
                myRigid.velocity = new Vector2(maxMovingVelocity * Input.GetAxis("Horizontal"), myRigid.velocity.y);
        }

        public override void Jump()
        {
            if (Input.GetKeyDown(KeyCode.Space) && touchGround)
            {
                myRigid.velocity = new Vector2(myRigid.velocity.x, maxJumpVelocity);
            }
        }

        public override void Fall()
        {
            // Use unity's default physic system
        }
    }

}