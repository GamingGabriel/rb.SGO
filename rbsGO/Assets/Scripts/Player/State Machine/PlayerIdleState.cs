using UnityEngine;

public class PlayerIdleState : PlayerBaseState
{
    public override void EnterState(PlayerStateManager player)
    {
        //What happens when we enter this scene?
        //Debug.Log("Entered Idle");
    }

    public override void UpdateState(PlayerStateManager player)
    {

        //What are we doing in this state?

        //When will we leave this scene?
        if (player.movement.magnitude > 0.1)
        {
            player.SwitchState(player.runState);
        }
        
        //Debug.Log("Idling");
    }
}
