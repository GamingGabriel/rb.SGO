using Unity.VisualScripting;
using UnityEngine;

public abstract class PlayerBaseState
{
    public abstract void EnterState(PlayerStateManager player); //Start for this state

    public abstract void UpdateState(PlayerStateManager player); //Update for this state
}
