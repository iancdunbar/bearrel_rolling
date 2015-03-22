using UnityEngine;
using UnityEditor;
using System.Collections;

[ CustomEditor( typeof( TerrainSegment ) ) ]
public class TerrainSegmentEditor : Editor 
{

    public override void OnInspectorGUI( )
    {
        DrawDefaultInspector( );

        if( GUILayout.Button( "Generate Basic Collider" ) )
        {

            TerrainSegment tgt = (TerrainSegment)target;

            EdgeCollider2D col = tgt.GetComponent<EdgeCollider2D>( );

            if( col == null )
            {
                col = tgt.gameObject.AddComponent<EdgeCollider2D>( );
            }

            Vector2[] points = new Vector2[ 2 ];

            Transform start = tgt.transform.FindChild( "Start" );
            Transform end   = tgt.transform.FindChild( "End" );

            points[ 0 ] = start.localPosition;
            points[ 1 ] = end.localPosition;

            col.points = points;

        }
    }

}
