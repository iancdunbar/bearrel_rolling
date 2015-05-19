using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    private bool action_flag;

    private bool up_flag;
    private bool down_flag;
    private bool left_flag;
    private bool right_flag;

    public float ymax;

    public Vector3 gravity;
    public Vector3 velocity;

    public float radius = 0.5f;

    

	// Use this for initialization
	void Start () 
    {
        //velocity = Vector3.zero;
        ymax = transform.position.y;
	}

    void Update( )
    {
        if( Input.GetKeyDown( KeyCode.W ) || Input.GetKeyDown( KeyCode.UpArrow ) )
        {
            up_flag = true;
        }

        if( Input.GetKeyDown( KeyCode.S ) || Input.GetKeyDown( KeyCode.DownArrow ) )
        {
            down_flag = true;
        }

        if( Input.GetKeyDown( KeyCode.A ) || Input.GetKeyDown( KeyCode.LeftArrow ) )
        {
            left_flag = true;
        }

        if( Input.GetKeyDown( KeyCode.D ) || Input.GetKeyDown( KeyCode.RightArrow ) )
        {
            right_flag = true;
        }
    }

	// Update is called once per frame
	void FixedUpdate () 
    {
        velocity.x += 0.5f * Time.fixedDeltaTime;

        velocity = velocity + gravity * Time.fixedDeltaTime;

        if( velocity.y < 0 && transform.position.y - radius < 0 )
        {
            transform.position = new Vector3( transform.position.x, radius, transform.position.z );

            velocity.y = 10;

            action_flag = true;
        }
        else
        {
            if( up_flag )
            {
                up_flag = false;

                if( action_flag )
                {
                    velocity.y = 10;
                    action_flag = false;
                }
                
            }
            
            if( down_flag )
            {
                down_flag = false;

                if( action_flag )
                {
                    velocity.y = -10;
                    action_flag = false;
                }
            }
            
            if( left_flag )
            {
                left_flag = false;

                if( action_flag )
                {
                    velocity.x -= 6;
                    Invoke( "KillExtraX", 0.3f );
                    action_flag = false;
                }
            }

            if( right_flag )
            {
                right_flag = false;

                if( action_flag )
                {
                    velocity.x += 6;
                    Invoke( "KillExtraX", 0.3f );
                    action_flag = false;
                }
            }
        }

        transform.position = transform.position + velocity * Time.fixedDeltaTime;

        if( transform.position.y > ymax ) ymax = transform.position.y;
	}

    public void KillExtraX( )
    {
        velocity.x = 1;
    }
}
