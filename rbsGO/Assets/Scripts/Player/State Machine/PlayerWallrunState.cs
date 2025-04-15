 using UnityEngine;

public class PlayerWallrunState : PlayerBaseState
{
    public override void EnterState(PlayerStateManager player)
    {
         //What happens when we enter this state?
        Debug.Log("Wallrun Entered");
        player.speed = player.WALLRUN_SPEED;
        player.gravity = player.WALL_GRAVITY;
        player.velocity.y = 0;
        player.wallrunning = true;

    }

    public override void UpdateState(PlayerStateManager player)
    {   
        //What happens in this state?
        float moveX = player.movement.x;

        float moveZ = player.movement.y;

        Vector3 actual_movement = (player.transform.forward * moveZ) + (player.transform.right * moveX);
        player.storedMovement = actual_movement;
        player.controller.Move(actual_movement * Time.deltaTime * player.speed);        

        //When will we leave this state? 
        if ((!player.wallLeft && !player.wallRight) || !player.AboveGround())
        {
            player.SwitchState(player.runState);
        } 
    }   
}
