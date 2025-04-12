 using UnityEngine;

public class PlayerWallrunState : PlayerBaseState
{
    public override void EnterState(PlayerStateManager player)
    {
         //What happens when we enter this state?
        Debug.Log("Wallrun Entered");

    }

    public override void UpdateState(PlayerStateManager player)
    {
        //What happens in this state?

        //When will we leave this state?  
    }   
}
