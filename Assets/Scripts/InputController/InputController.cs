using UnityEngine;
using System.Collections;

public class InputController : MonoBehaviour
{

    // Private Singleton Instance
    private static InputController ic_instance;
    
    public static InputController Instance
    {
        get
        {
            if( ic_instance == null )
                ic_instance = ConstructController();

            return ic_instance;
        }
    }

    private Camera main_camera;

    // TARGET VARIABLES
    private     Vector2         mouse_world_position;

    private     Collider2D      collider_last;
    private     Rigidbody2D     rigidbody_last;
    private     Interactable    collider_interactable_last;
    private     Interactable    rigidbody_interactable_last;

    private     Collider2D      collider_target;
    private     Rigidbody2D     rigidbody_target;
    private     Interactable    collider_interactable_target;
    private     Interactable    rigidbody_interactable_target;
    private     bool            collider_target_dirty;
    private     bool            rigidbody_target_dirty;

    private     bool            mouse_targets_dirty;

    // Public Read Variables
    public      Vector2         Mouse_World_Position { get { return mouse_world_position; } }

    public      Collider2D      Active_Collider_Target { get { return collider_target; } }
    public      Rigidbody2D     Active_Rigidbody_Target { get { return rigidbody_target; } }
    public      bool            Primary_Button { get { return Input.GetMouseButton( 0 ); } }

    // METHODS
    // This function constructs a new InputController if one is absent
    private static InputController ConstructController()
    {
        GameObject input_controller = new GameObject();

        return input_controller.AddComponent<InputController>();
    }

    // This function locates the mouse on screen and updates the targets if nessicary
    private void Mouse_Pick()
    {
        mouse_world_position = new Vector2(main_camera.ScreenToWorldPoint(Input.mousePosition).x, main_camera.ScreenToWorldPoint(Input.mousePosition).y);

        RaycastHit2D hit = Physics2D.Raycast(mouse_world_position, Vector2.zero, 0);

        if( hit.collider != collider_target )
        {
            collider_last = collider_target;
            collider_interactable_last = collider_interactable_target;
            collider_target = hit.collider;
            if( collider_target )
                collider_interactable_target = collider_target.GetComponent<Interactable>();
            else
                collider_interactable_target = null;

            collider_target_dirty = true;

            if( hit.rigidbody != rigidbody_target )
            {
                rigidbody_last = rigidbody_target;
                rigidbody_interactable_last = rigidbody_interactable_target;
                rigidbody_target = hit.rigidbody;
                if( rigidbody_target )
                    rigidbody_interactable_target = rigidbody_target.GetComponent<Interactable>();
                else
                    rigidbody_interactable_target = null;

                rigidbody_target_dirty = true;
            }

            mouse_targets_dirty = true;
        }     
    }

    // This function processes all dirty targets and takes the appropriate action
    private void Clean_Targets()
    {

        if( collider_target_dirty )
        {
            collider_target_dirty = false;

            if( collider_interactable_last != null) collider_interactable_last.InteractOnMouseExit();

            if( collider_interactable_target != null ) collider_interactable_target.InteractOnMouseEnter();
        }

        if( rigidbody_target_dirty )
        {
            rigidbody_target_dirty = false;

            if( rigidbody_interactable_last ) rigidbody_interactable_last.InteractOnMouseExit();

            if( rigidbody_interactable_target ) rigidbody_interactable_target.InteractOnMouseEnter();
        }

        mouse_targets_dirty = false;
    }


    // UNITY FUNCTIONS
    // Called upon the first frame of the objects life
    void Awake()
    {
        if( ic_instance == null )
        {
            ic_instance = this;
        }
        else if( ic_instance != this )
        {
            Destroy(this.gameObject);
        }
    }

	// Use this for initialization
	void Start () 
    {

        main_camera = GameObject.FindGameObjectWithTag("MainCamera").camera;

        collider_last = null;
        rigidbody_last = null;
        collider_interactable_last = null;
        rigidbody_interactable_last = null;

        collider_target = null;
        rigidbody_target = null;
        collider_interactable_target = null;
        rigidbody_interactable_target = null;
        collider_target_dirty = false;
        rigidbody_target_dirty = false;
        mouse_targets_dirty = false;

	}
	
	// Update is called once per frame
	void Update () 
    {

        if( Input.GetMouseButtonDown(0) )
        {
            if( collider_interactable_target ) collider_interactable_target.InteractOnMouseDown();
            if( rigidbody_interactable_target ) rigidbody_interactable_target.InteractOnMouseDown();
        }
        else if( Input.GetMouseButtonUp(0) )
        {
            if( collider_interactable_target ) collider_interactable_target.InteractOnMouseUp();
            if( rigidbody_interactable_target ) rigidbody_interactable_target.InteractOnMouseUp();
        }




	}
    


    // Fixed updated is called at a fixed interval
    void FixedUpdate()
    {

        Mouse_Pick();

        if( mouse_targets_dirty )
            Clean_Targets();

    }
}
