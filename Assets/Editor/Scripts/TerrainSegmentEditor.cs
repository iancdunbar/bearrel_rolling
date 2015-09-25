using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[ CustomEditor( typeof( TerrainSegment ) ) ]
public class TerrainSegmentEditor : Editor 
{
    private readonly Color START  = new Color( 0, 1, 0, 1 );
    private readonly Color MIDDLE = new Color( 0, 0, 1, 1 );
    private readonly Color END    = new Color( 1, 0, 0, 1 );

    string[] choices = new string[ ] { };
    int choice_index = 0;


    public override void OnInspectorGUI( )
    {
        DrawDefaultInspector( );

        TerrainSegment tgt = (TerrainSegment) target;

        if( tgt.PointsFromColor )
        {
            choice_index = EditorGUILayout.Popup( "Start Index", choice_index, choices );
            choice_index = EditorGUILayout.Popup( "End Index", choice_index, choices );
        }

        if( GUILayout.Button( "Print Start End" ) )
        {
            Debug.Log( "Start: " + tgt.StartPoint + ", End: " + tgt.EndPoint );
        }

        if( GUILayout.Button( "Generate Basic Collider" ) )
        {

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

            Vector3 start_point = Vector3.zero;
            Vector3 end_point = Vector3.zero;

            Vector3[] points = mesh.vertices;
            Color[] colors   = mesh.colors;
            List<Vector2> edge_points = null;
            Vector2 mesh_filter_offset = mesh_filter.transform.position - tgt.transform.position;

            int old_edge_index = 0;
           // bool generating = false;

            List<int> start_idxs = new List<int>( );
            List<int> end_idxs = new List<int>( );
            List<int> mid_idxs = new List<int>( );

            // Establish the indices of the relevant verticies
            for( int i = 0; i < points.Length; i++ )
            {
                if( colors[ i ] == START )
                    start_idxs.Add( i );
                else if( colors[ i ] == MIDDLE )
                    mid_idxs.Add( i );
                else if( colors[ i ] == END )
                    end_idxs.Add( i );
            }

            // Test for data validity
            if( start_idxs.Count != end_idxs.Count )
            {
                Debug.LogError( "Unable to generate a collider from the mesh. There is an issue with " + tgt.name + "'s mesh filter!" );
                return;
            }

            // Sort the vertecies by x
            start_idxs.Sort( ( int a, int b ) => 
            {
                float ax = points[ a ].x;
                float bx = points[ b ].x;

                return ax.CompareTo( bx );

            } );

            mid_idxs.Sort( ( int a, int b ) =>
            {
                float ax = points[ a ].x;
                float bx = points[ b ].x;

                return ax.CompareTo( bx );

            } ); 

            end_idxs.Sort( ( int a, int b ) =>
            {
                float ax = points[ a ].x;
                float bx = points[ b ].x;

                return ax.CompareTo( bx );

            } );

            int curr_mid_point = 0;
            edge_points = new List<Vector2>( );
            start_point = points[ start_idxs[ 0 ] ];
            for( int i = 0; i < start_idxs.Count; i++ )
            {
                // Add the starting point to the edge list
                edge_points.Add( new Vector2( points[ start_idxs[ i ] ].x, points[ start_idxs[ i ] ].y ) + mesh_filter_offset );

                // Assign the end point
                end_point = points[ end_idxs[ i ] ];

                // Add the mid points, for each mid point in the mid point list that is to the left of the current end point
                for( ; curr_mid_point < mid_idxs.Count && points[ mid_idxs[ curr_mid_point ] ].x < end_point.x; curr_mid_point++ )
                {
                    edge_points.Add( new Vector2( points[ mid_idxs[ curr_mid_point ] ].x, points[ mid_idxs[ curr_mid_point ] ].y ) + mesh_filter_offset );
                }

                // Add the end point
                edge_points.Add( end_point );

                // Create or assign to an edge collider
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
                edge_points.Clear( );   

            }

            tgt.InitStartEndPoints( start_point, end_point );

        }
    }

}
