using UnityEngine;
using System.Collections;
using System.IO;

[RequireComponent(typeof(Rigidbody))]
public class TempController : MonoBehaviour {

	private Rigidbody _rb;
    private BoxCollider _bc;
    private CapsuleCollider _cc;

	private Transform right_shoulder;
	private Transform left_shoulder;
	private Transform original_right;
	private Transform original_left;
	private Transform L_Force;
	private Transform R_Force;
    private Transform original_L;
    private Transform original_R;
	private Transform main_camera;
	
	private Vector3 right;
	private Vector3 left;
	private Vector3 inital_position;
	private Vector3 inital_camera_position;
	private Quaternion initial_angles;
	private Quaternion initial_camera_angles;

    private string pause = "Pause";

    private bool draw_gui = true;
    private bool paused = false;
    private bool left_ground = false;
    public bool grounded = false;

	private float lerp_in_time = 1;
	private float lerp_out_time = 1;
	private float lerp_in_rate;
	private float lerp_out_rate;

    public TextAsset default_controls;

	public float velocity_follow_value = 20;
	public float braking_force_value = 100;
	public float shear_force_value = 70;
	public float forward_force_value = 200;
    public float bearrel_roll_value = 100;
    public float air_steer_value = 20;
    public float air_yaw_force = 40;
    public float air_velocity_follow = 1;

    public LayerMask mask = -1;

	private float default_velocity_follow_value;
	private float default_braking_force_value;
	private float default_shear_force_value;
	private float default_forward_force_value;
    private float default_bearrel_roll_value;
    private float default_air_steer_value;
    private float default_air_yaw_force;
    private float default_air_velocity_follow;

	// Use this for initialization
	void Start () {

		main_camera = GameObject.FindWithTag ("MainCamera").transform;

		original_right = transform.FindChild ("R_Shoulder");
		original_left = transform.FindChild ("L_Shoulder");

		original_L = transform.FindChild ("L_Force");
		original_R = transform.FindChild ("R_Force");

		_rb = GetComponent<Rigidbody> ();
        _bc = GetComponent<BoxCollider>();
        _cc = GetComponent<CapsuleCollider>();

		lerp_in_rate = 90 / lerp_in_time;
		lerp_out_rate = 90 / lerp_out_time;

		inital_position = transform.position;
		initial_angles = transform.rotation;

		inital_camera_position = main_camera.position;
		initial_camera_angles = main_camera.rotation;

        if( default_controls )
            parse_controls( default_controls );

		default_velocity_follow_value   = velocity_follow_value;
		default_braking_force_value	    = braking_force_value;
		default_shear_force_value       = shear_force_value;
		default_forward_force_value     = forward_force_value;
        default_bearrel_roll_value      = bearrel_roll_value;
        default_air_steer_value         = air_steer_value;
        default_air_yaw_force           = air_yaw_force;
        default_air_velocity_follow     = air_velocity_follow;
	}

	void restart_rigidbody()
	{
		transform.position = inital_position;
		transform.rotation = initial_angles;
		_rb.velocity = Vector3.zero;

		main_camera.position = inital_camera_position;
		main_camera.rotation = initial_camera_angles;
	}

	void reset_force_defaults()
	{
		velocity_follow_value = default_velocity_follow_value;
		shear_force_value = default_shear_force_value;
		braking_force_value = default_braking_force_value;
		forward_force_value = default_forward_force_value;

        bearrel_roll_value = default_bearrel_roll_value;
        air_steer_value = default_air_steer_value;
        air_yaw_force = default_air_yaw_force;
        air_velocity_follow = default_air_velocity_follow;
	}

    void parse_controls( TextAsset tgt )
    {
        string data = tgt.text;

        string[] values = data.Split( ',' );

        // Ugly temp hard and unsafe code
        velocity_follow_value = float.Parse( values[ 0 ] );
        braking_force_value = float.Parse( values[ 1 ] );
        shear_force_value = float.Parse( values[ 2 ] );
        forward_force_value = float.Parse( values[ 3 ] );
        bearrel_roll_value = float.Parse( values[ 4 ] );
        air_steer_value = float.Parse( values[ 5 ] );
        air_yaw_force = float.Parse( values[ 6 ] );
        air_velocity_follow = float.Parse( values[ 7 ] );
        // End ugly temp hard and unsafe code

    }

    void flush_current_controls( )
    {
#if !UNITY_WEBPLAYER
        string filename = Application.dataPath + "/Data/SavedControls_" + System.DateTime.Now.ToString("MM-dd-yyyy-hh-mm-ss") + ".txt";

        string data = velocity_follow_value + "," + braking_force_value + "," + shear_force_value + "," + forward_force_value  + "," +
                        bearrel_roll_value + "," + air_steer_value + "," + air_yaw_force + "," + air_velocity_follow;

//        File.WriteAllText( filename, data );
#endif
    }


    void Update()
    {
        // Press R to restart the rigidbody
        if (Input.GetKeyDown(KeyCode.R))
        {
            restart_rigidbody();
        }
    }

	// Update is called once per frame
	void FixedUpdate () 
	{

        float orientation = 1;
        right_shoulder = original_right;
        left_shoulder = original_left;
        L_Force = original_L;
        R_Force = original_R;

        if (transform.up.y < 0)
        {
            orientation = -1;

            right_shoulder = original_left;
            left_shoulder = original_right;

            L_Force = original_R;
            R_Force = original_L;
        }

        if (!grounded)
        {
            left_ground = true;
        }


        if (grounded && orientation < 0)
        {
            if (left_ground)
            {
                //_rb.velocity = new Vector3(_rb.velocity.x, -_rb.velocity.y * 0.5f, _rb.velocity.z);
                left_ground = false;
            }
        }
		
        
		right = R_Force.position;
		left = L_Force.position; 
		
		if(grounded)
            _rb.AddForce ( transform.forward * forward_force_value);
        
        
		if(!Input.GetKey(KeyCode.A))
		{
			// If the arm is not full in, rotate it in!
			if(left_shoulder.localEulerAngles.y < 90)
			{
				left_shoulder.localEulerAngles = left_shoulder.localEulerAngles + new Vector3(0, lerp_out_rate * Time.deltaTime, 0);
			}
		}
		else
		{
			// If the arm is not fully out, then rotate it out!
			if(left_shoulder.localEulerAngles.y > 0)
			{
				left_shoulder.localEulerAngles = left_shoulder.localEulerAngles - new Vector3(0, lerp_out_rate * Time.deltaTime, 0);
			}
			else
				left_shoulder.localEulerAngles = new Vector3(0, 0, 0);

            if (grounded)
            {
                _rb.AddForceAtPosition(_rb.velocity.normalized * -braking_force_value, left);
                _rb.AddForce(-transform.right * shear_force_value * orientation);
            }
            else
            {
                // Right angled bear velocity
                Vector3 tgt_velocity = Quaternion.Euler( 0, -1, 0 ) * _rb.velocity;


                // adjust the forward velocity
                _rb.velocity = Vector3.MoveTowards( _rb.velocity, tgt_velocity, air_steer_value  * Time.deltaTime );

                _rb.AddTorque(transform.forward * bearrel_roll_value);
                _rb.AddTorque( -main_camera.up * air_yaw_force );
               // _rb.AddForce( -main_camera.right * shear_force_value );
            }

            //_rb.AddForce(-transform.right * shear_force_value * orientation);
        
		}
		
		if(!Input.GetKey(KeyCode.D))
		{
			// If the arm is not full in, rotate it in!
			if(right_shoulder.localEulerAngles.y > 90)
			{
				right_shoulder.RotateAround(right_shoulder.position, -right_shoulder.up, -lerp_in_rate * Time.deltaTime);
        
			}
        
		}
		else
		{
			// If the arm is not fully out, then rotate it out!
			if(right_shoulder.localEulerAngles.y < 180)
			{
				right_shoulder.RotateAround(right_shoulder.position, -right_shoulder.up, lerp_out_rate * Time.deltaTime);
        
			}

            if (grounded)
            {
                _rb.AddForceAtPosition(_rb.velocity.normalized * -braking_force_value, right);
                _rb.AddForce(transform.right * shear_force_value * orientation);
            }
            else
            {
                // Right angled bear velocity
                Vector3 tgt_velocity = Quaternion.Euler( 0, 1, 0 ) * _rb.velocity;


                // adjust the forward velocity
                _rb.velocity = Vector3.MoveTowards( _rb.velocity, tgt_velocity, air_steer_value * Time.deltaTime );

                // Spin the bear
                _rb.AddTorque(-transform.forward * bearrel_roll_value);
                _rb.AddTorque( main_camera.up * air_yaw_force );
                //_rb.AddForce( main_camera.right * shear_force_value );
            }
            //_rb.AddForce(transform.right * shear_force_value * orientation);
		}

        if(grounded)
		    _rb.velocity = Vector3.MoveTowards (_rb.velocity, transform.forward * _rb.velocity.magnitude, velocity_follow_value * Time.deltaTime);
        else
            _rb.velocity = Vector3.MoveTowards( _rb.velocity, transform.forward * _rb.velocity.magnitude, velocity_follow_value * air_velocity_follow * Time.deltaTime );
	}

	void OnGUI()
	{

#if UNITY_EDITOR
        if( GUI.Button( new Rect( 10, 10, 115, 30 ), "Save Controls" ) )
        {
            flush_current_controls( );
        }
#endif

        if( GUI.Button( new Rect( 125, 10, 115, 30 ), "Toggle GUI" ) )
            draw_gui = !draw_gui;

        if( draw_gui )
        {

            if( GUI.Button( new Rect( 10, 40, 230, 30 ), pause ) )
            {
                if( paused )
                {
                    Time.timeScale = 1;
                    pause = "Pause";
                    paused = false;
                }
                else
                {
                    Time.timeScale = 0;
                    pause = "Resume";
                    paused = true;
                }
            }

            if( GUI.Button( new Rect( 10, 70, 115, 30 ), "Restart" ) )
            {
                restart_rigidbody( );
            }

            if( GUI.Button( new Rect( 125, 70, 115, 30 ), "Reset Forces" ) )
            {
                reset_force_defaults( );
            }

            GUI.TextArea( new Rect( 10, 110, 230, 40 ), "Velocity to Forward modifier. 0 - 100.\nCurrent: " + velocity_follow_value );

            velocity_follow_value = GUI.HorizontalSlider( new Rect( 10, 150, 230, 30 ), velocity_follow_value, 0, 100 );

            GUI.TextArea( new Rect( 10, 170, 230, 40 ), "Braking Force. 10 - 100.\nCurrent: " + braking_force_value );

            braking_force_value = GUI.HorizontalSlider( new Rect( 10, 210, 230, 30 ), braking_force_value, 10, 100 );

            GUI.TextArea( new Rect( 10, 230, 230, 40 ), "Shear Force on Turn. 0 - 300.\nCurrent: " + shear_force_value );
            shear_force_value = GUI.HorizontalSlider( new Rect( 10, 270, 230, 30 ), shear_force_value, 0, 300 );

            GUI.TextArea( new Rect( 10, 290, 230, 40 ), "Forward Pull Force. 0 - 200.\nCurrent: " + forward_force_value );
            forward_force_value = GUI.HorizontalSlider( new Rect( 10, 330, 230, 40 ), forward_force_value, 0, 200 );

            GUI.TextArea( new Rect( 10, 350, 230, 40 ), "Air Steering Value. 0 - 100.\nCurrent: " + air_steer_value );
            air_steer_value = GUI.HorizontalSlider( new Rect( 10, 390, 230, 40 ), air_steer_value, 0, 100 );

            GUI.TextArea( new Rect( 10, 410, 230, 40 ), "Air Yaw force. 0 - 50.\nCurrent: " + air_yaw_force );
            air_yaw_force = GUI.HorizontalSlider( new Rect( 10, 450, 230, 40 ), air_yaw_force, 0, 50 );

            GUI.TextArea( new Rect( 10, 470, 230, 40 ), "Bearrel Roll Foce. 0 - 100.\nCurrent: " + bearrel_roll_value );
            bearrel_roll_value = GUI.HorizontalSlider( new Rect( 10, 510, 230, 40 ), bearrel_roll_value, 0, 100 );

            GUI.TextArea( new Rect( 10, 530, 230, 40 ), "Air Velocity Follow. 0 - 1.\nCurrent: " + air_velocity_follow );
            air_velocity_follow = GUI.HorizontalSlider( new Rect( 10, 570, 230, 40 ), air_velocity_follow, 0, 1 );

        }

	}

	void OnTriggerEnter(Collider other)
	{
		// If the other is the respawn rectangle
		if (other.tag == "Respawn")
		{
			restart_rigidbody();
		}

        if( other.tag == "Ground" )
        {
            grounded = true;
        }
	}

    void OnTriggerExit( Collider other )
    {
        if( other.tag == "Ground" )
            grounded = false;
    }
}
