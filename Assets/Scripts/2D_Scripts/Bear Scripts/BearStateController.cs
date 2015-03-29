using UnityEngine;
using System.Collections;

public enum BearState { IDLE, JUMPING, SLAMMING, PAUSED }

public class BearStateController 
{

	/////////////////////////////////////////////
    // Private Variable
    /////////////////////////////////////////////

    public BearState previous_state;
    public BearState current_state;

    /////////////////////////////////////////////


    /////////////////////////////////////////////
    // Inspector Variables
    /////////////////////////////////////////////



    /////////////////////////////////////////////


    /////////////////////////////////////////////
    // Public Functions
    /////////////////////////////////////////////

    public BearStateController( )
    {
    	previous_state = BearState.PAUSED;
    	current_state = BearState.PAUSED;
    }

    public void ChangeState( BearState tgt )
    {

    	MessageDispatch.BroadcastMessage( "OnExitState", current_state );

    	previous_state = current_state;

    	current_state = tgt;

    	MessageDispatch.BroadcastMessage( "OnEnterState", current_state );

    }

    /////////////////////////////////////////////

}
