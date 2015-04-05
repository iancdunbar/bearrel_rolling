using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[ CustomEditor( typeof( TerrainSegment ) ) ]
public class TerrainSegmentEditor : Editor 
{
    private readonly Color START  = new Color( 1, 0, 0, 1 );
    private readonly Color MIDDLE = new Color( 0, 0, 1, 1 );
    private readonly Color END    = new Color( 0, 1, 0, 1 );

    string[] choices = new string[ ] { };
    int choice_index = 0;

    public override void OnInspectorGUI( )
    {
        DrawDefaultInspector( );

        choice_index = EditorGUILayout.Popup( "Options!", choice_index, choices );

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

        if( GUILayout.Button( "Generate Collider From Mesh Colors" ) )
        {
            TerrainSegment tgt = (TerrainSegment) target;

            MeshFilter mesh_filter = tgt.CollisionMesh;

            if( mesh_filter == null )
            {
                Debug.LogWarning( "No mesh filter explicity assigned to be the collider, searching for a mesh filter instead." );
                mesh_filter = tgt.GetComponent<MeshFilter>( );
            }

            if( mesh_filter == null )
            {
                Debug.LogError( "Unable to generate a collider from the mesh. There is no mesh filter on " + tgt.name + "!" );
                return;
            }

            // NOTE: mesh_filter.mesh copies the mesh!!!
            Mesh mesh = mesh_filter.sharedMesh;

            if( mesh == null )
            {
                Debug.LogError( "Unable to generate a collider from the mesh. There is no mesh specified in " + tgt.name + "'s mesh filter!" );
                return;
            }

            EdgeCollider2D[] old_edges = tgt.gameObject.GetComponents<EdgeCollider2D>( );

            for( int i = 0; i < old_edges.Length; i++ )
            {
                old_edges[ i ].enabled = false;
            }

            Vector3[] points = mesh.vertices;
            Color[] colors   = mesh.colors;
            List<Vector2> edge_points = null;

            int old_edge_index = 0;
            bool generating = false;

            for( int i = 0; i < colors.Length; i++ )
            {
                // TEMP FIX
                if( colors[ i ].g != 1 ) colors[ i ].g = 0;

                if( generating )
                {
                    //Debug.Log( "Comparing for middle ( " + MIDDLE + " ) and Current ( " + colors[ i ] + " )" );
                    // We are currently adding points to an edge collider
                    // Check to see if we are at a mid point
                    if( colors[ i ] == MIDDLE || colors[ i ] == START )
                    {
                        if( colors[ i ] == START )
                            Debug.LogWarning( "Encountered a Start at index " + i + " without reaching an end. Treating it as if it were a midpoint." );

                        edge_points.Add( new Vector2( points[ i ].x, points[ i ].y ) );

                    }
                    else if( colors[ i ] == END )
                    {
                        // We have reached the end of the sequence and now its time to make the points
                        edge_points.Add( new Vector2( points[ i ].x, points[ i ].y ) );

                        // Get or create the edge collider
                        EdgeCollider2D edge = null;
                        if( old_edge_index < old_edges.Length )
                        {
                            edge = old_edges[ old_edge_index ];
                            edge.enabled = true;
                            old_edge_index++;
                        }
                        else
                        {
                            edge = tgt.gameObject.AddComponent<EdgeCollider2D>( );
                        }

                        // Set the points of the edge collider
                        edge.points = edge_points.ToArray( );

                        // Turn off generation so we start looking for the next start point
                        generating = false;
                    }
                }
                else
                {
                    // We are looking for the next point to begin a collider
                    if( colors[ i ] == START )
                    {
                        // We have found a start point
                        edge_points = new List<Vector2>( );
                        edge_points.Add( new Vector2( points[ i ].x, points[ i ].y ) );
                        generating = true;
                    }
                }
            }
        }
    }

}
