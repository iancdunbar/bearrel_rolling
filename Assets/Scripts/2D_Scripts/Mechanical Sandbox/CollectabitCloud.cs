using UnityEngine;
using System.Collections;

public class CollectabitData
{
	public Vector3 pos;
	public int index;
}

public class CollectabitCloud : MonoBehaviour {

	/////////////////////////////////////////////
	// Private Variables
	/////////////////////////////////////////////

	

	/////////////////////////////////////////////

	
	/////////////////////////////////////////////
	// Inspector Variables
	/////////////////////////////////////////////

	[SerializeField]
	Transform[ ] bits;

	/////////////////////////////////////////////


	/////////////////////////////////////////////
	// Private Functions
	/////////////////////////////////////////////



	/////////////////////////////////////////////

	
	/////////////////////////////////////////////
	// Public Functions
	/////////////////////////////////////////////

	public CollectabitData TestBits( Vector2 dir, Vector3 pos )
	{

		for( int i = 0; i < bits.Length; i++ )
		{
			if( bits[ i ].gameObject.activeSelf )
			{
				Vector2 delta = bits[ i ].position - pos;
			
				float dot = Vector2.Dot( dir, delta.normalized );

				if( dot > 0.9f )
				{
					CollectabitData result = new CollectabitData( );
					result.pos = bits[ i ].position;
					result.index = i;

					return result;
				}
			}

		}

		return null;
	}

	public bool CollectBit( int index )
	{
		if( index >= 0 && index < bits.Length )
		{
			bits[ index ].gameObject.SetActive( false );
		}

		for( int i = 0; i < bits.Length; i++ )
		{
			if( bits[ i ].gameObject.activeSelf )
				return false;
		}

		return true;
	}

	/////////////////////////////////////////////

	/////////////////////////////////////////////
	// Unity Messages
	/////////////////////////////////////////////

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	/////////////////////////////////////////////
}
