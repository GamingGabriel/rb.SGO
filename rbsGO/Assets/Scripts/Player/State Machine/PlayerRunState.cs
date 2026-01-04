using UnityEngine;

public class PlayerRunState : PlayerBaseState
{
    public override void EnterState(PlayerStateManager player)
    {
        //What happens when we enter this scene?
        //Debug.Log("Entered Run");
        player.speed = player.MOVE_SPEED;
        player.gravity = player.BASE_GRAVITY;
        player.wallrunning = false;
    }

    public override void UpdateState(PlayerStateManager player)
    {
        //Debug.Log("Running");
         //What are we doing in this state?
            float moveX = player.movement.x;

            float moveZ = player.movement.y;
            
            Vector3 actual_movement = (player.transform.forward * moveZ) + (player.transform.right * moveX);
            player.storedMovement = actual_movement; //Attempted to make inertia...I think. 

            player.controller.Move(actual_movement * Time.deltaTime * player.speed);
        //When will we leave this scene?  
        if ((player.wallLeft || player.wallRight) && player.AboveGround())
        {
            player.SwitchState(player.wallrunState);
        }
        else if (player.movement.magnitude < 0.1)
        {
            player.SwitchState(player.idleState);
        }
    }
}
