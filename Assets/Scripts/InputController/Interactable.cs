using UnityEngine;
using System.Collections;

public delegate void VoidReturnNotify();

// Allows the object to be interacted with by the InputController
// Other components on the object will subscribe to the Interactable object
// Subscribed objects will be notified when an action occurs
public class Interactable : MonoBehaviour {

    

    VoidReturnNotify onMouseEnter;
    VoidReturnNotify onMouseExit;
    VoidReturnNotify onMouseStay;
    VoidReturnNotify onMouseDown;
    VoidReturnNotify onMouseUp;
    VoidReturnNotify onMouseClick;

    public  void    SubscribeMouseEnter     (VoidReturnNotify del)  { onMouseEnter += del; }
    public  void    SubscribeMouseExit      (VoidReturnNotify del)  { onMouseExit += del; }
    public  void    SubscribeMouseStay      (VoidReturnNotify del)  { onMouseStay += del; }
    public  void    SubscribeMouseDown      (VoidReturnNotify del)  { onMouseDown += del; }
    public  void    SubscribeMouseUp        (VoidReturnNotify del)  { onMouseUp += del; }
    public  void    SubscribeMouseClick     (VoidReturnNotify del)  { onMouseClick += del; }

    public  void    UnsubscribeMouseEnter   (VoidReturnNotify del)  { onMouseEnter -= del; }
    public  void    UnsubscribeMouseExit    (VoidReturnNotify del)  { onMouseExit -= del; }
    public  void    UnsubscribeMouseStay    (VoidReturnNotify del)  { onMouseStay -= del; }
    public  void    UnsubscribeMouseDown    (VoidReturnNotify del)  { onMouseDown -= del; }
    public  void    UnsubscribeMouseUp      (VoidReturnNotify del)  { onMouseUp -= del; }
    public  void    UnsubscribeMouseClick   (VoidReturnNotify del)  { onMouseClick -= del; }


    public void InteractOnMouseEnter()
    {
        if( onMouseEnter != null )
            onMouseEnter();
    }

    public void InteractOnMouseExit()
    {
        if( onMouseExit != null )
            onMouseExit();
    }

    public void InteractOnMouseStay()
    {
        if( onMouseStay != null )
            onMouseStay();
    }

    public void InteractOnMouseDown()
    {
        if( onMouseDown != null )
            onMouseDown();
    }

    public void InteractOnMouseUp()
    {
        if( onMouseUp != null )
            onMouseUp();
    }

    public void InteractOnMouseClick()
    {
        if( onMouseClick != null )
            onMouseClick();
    }

//	// Use this for initialization
//	void Start () {
//	
//	}
//	
//	// Update is called once per frame
//	void Update () {
//	
//	}
}
