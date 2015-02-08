using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MessageDispatch : MonoBehaviour {

    // Message Function Prototype
    public delegate void MessageFunction( object arg = null );

    ///////////////////////////////////
    // Private Variables
    //////////////////////////////////

    private static MessageDispatch instance;

    private Dictionary< string, MessageFunction > message_dictionary;
    private bool initialized = false;

    //////////////////////////////////


    ///////////////////////////////////
    // Private Functions
    //////////////////////////////////

    public static MessageDispatch get_instance( )
    {
        if( instance == null )
        {
            instance = FindObjectOfType<MessageDispatch>( );

            if( FindObjectsOfType<MessageDispatch>( ).Length > 1 )
                Debug.LogError( "There are multiple copies of a message dispatch object" );

            // If the instance is still null then there was no MessageDispatch Object
            if( instance == null )
            {
                GameObject message_object = new GameObject( );
                message_object.name = "MessageDispatch";

                instance = message_object.AddComponent<MessageDispatch>( );
                instance.initialize( );
            }
        }

        return instance;
    }

    private void initialize( )
    {
        message_dictionary = new Dictionary<string, MessageFunction>( );
    }

    //////////////////////////////////


    ///////////////////////////////////
    // Public Static Interface
    //////////////////////////////////

    public static void RegisterListener( string message_name, MessageFunction func )
    {
        MessageDispatch md = get_instance( );

        if( md.message_dictionary.ContainsKey( message_name ) )
        {
            md.message_dictionary[ message_name ] += func;
        }
        else
        {
            md.message_dictionary.Add( message_name, func );
        }
    }

    public static new void BroadcastMessage( string message, object arg = null )
    {
        MessageDispatch md = get_instance( );

        if( md.message_dictionary.ContainsKey( message ) )
        {
            md.message_dictionary[ message ]( arg );
        }
        else
        {
            Debug.LogWarning( "No listeners found for " + message );
        }
    }

    //////////////////////////////////


    ///////////////////////////////////
    // Unity Message Handlers
    //////////////////////////////////

    private void Awake()
    {
        // Maintain singleton consistency
        if( instance != this )
        {
            if( instance == null )
            {
                instance = this;
                instance.initialize( ); 
            }
            else
                Destroy( this.gameObject );
        }

        
    }
}
