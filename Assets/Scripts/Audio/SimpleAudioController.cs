using UnityEngine;
using System.Collections;

public class SimpleAudioController : MonoBehaviour {


    private static SimpleAudioController instance;

    public static SimpleAudioController Instance { get { return instance; } }

    [SerializeField]
    private AudioSource music;
    [SerializeField]
    private AudioSource sfx;

    [SerializeField]
    private AudioClip[] crash_emotes;

    public static void PlayCrashEmote( )
    {
        PlaySound( instance.crash_emotes[ Random.Range( 0, instance.crash_emotes.Length ) ] );
    }

    public static void PlaySound( AudioClip clip )
    {
        instance.sfx.PlayOneShot( clip );
    }


    ///////////////////////////////////////////////
    // Unity Messages
    ///////////////////////////////////////////////

    void Awake( )
    {
        if( instance == null )
            instance = this;
        else
            DestroyImmediate( this.gameObject );
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    ///////////////////////////////////////////////
}
