using UnityEngine;
using System.Collections;

[RequireComponent( typeof( Interactable ), typeof( SpriteRenderer ) ) ]
public class Button : MonoBehaviour {

    private VoidReturnNotify notify;

    private Interactable interact;
    private SpriteRenderer sr;

    public Color normal_color;
    public Color hover_color;
    public Color click_color;

    public void SubscribeNotify(VoidReturnNotify del)
    {
        notify += del;
    }

    private void MouseEnter()
    {
        sr.color = hover_color;
    }

    private void MouseExit()
    {
        sr.color = normal_color;
    }

    private void MouseDown()
    {
        sr.color = click_color;
    }

    private void MouseUp()
    {
        sr.color = hover_color;

        if( notify != null ) notify();
    }

	// Use this for initialization
	void Start () 
    {
        interact = transform.GetComponent<Interactable>();
        sr = transform.GetComponent<SpriteRenderer>();

        sr.color = normal_color;

        interact.SubscribeMouseEnter(MouseEnter);
        interact.SubscribeMouseExit(MouseExit);
        interact.SubscribeMouseDown(MouseDown);
        interact.SubscribeMouseUp(MouseUp);
        
	}
	
//	// Update is called once per frame
//	void Update () {
//	
//	}
}
