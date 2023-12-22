using UnityEngine;


public static class MeshHelper
{
  public static Mesh generateSphereMesh( float radius, float noiceAmount = 0.1f, int numSegments = 20, int numRings = 10 )
  {
    Mesh mesh = new Mesh();

    int numVertices  = ( numSegments + 1 ) * ( numRings + 1 );
    int numTriangles = numSegments * numRings * 6;

    Vector3[] vertices  = new Vector3[numVertices];
    Vector2[] uv        = new Vector2[numVertices];
    int[]     triangles = new int[numTriangles];

    float phiStep   = Mathf.PI / numRings;
    float thetaStep = 2.0f * Mathf.PI / numSegments;

    int vertexIndex   = 0;
    int triangleIndex = 0;

    for ( int ring = 0; ring <= numRings; ring++ )
    {
      float phi = ring * phiStep;

      for ( int segment = 0; segment <= numSegments; segment++ )
      {
        float theta = segment * thetaStep;

        float x = radius * Mathf.Sin( phi ) * Mathf.Cos( theta );
        float y = radius * Mathf.Cos( phi );
        float z = radius * Mathf.Sin( phi ) * Mathf.Sin( theta );

        Vector3 noiceOffset = Vector3.zero;
        if ( ring > 0 && ring < numRings && segment > 0 && segment < numSegments )
          noiceOffset = Random.onUnitSphere * noiceAmount;

        vertices[vertexIndex] = new Vector3( x, y, z ) + noiceOffset;
        uv[vertexIndex] = new Vector2( (float)segment / numSegments, (float)ring / numRings );

        if ( ring < numRings && segment < numSegments )
        {
          triangles[triangleIndex++] = vertexIndex + 1;
          triangles[triangleIndex++] = vertexIndex + numSegments + 1;
          triangles[triangleIndex++] = vertexIndex;

          triangles[triangleIndex++] = vertexIndex + numSegments + 2;
          triangles[triangleIndex++] = vertexIndex + numSegments + 1;
          triangles[triangleIndex++] = vertexIndex + 1;

        }

        vertexIndex++;
      }
    }

    mesh.vertices = vertices;
    mesh.uv = uv;
    mesh.triangles = triangles;

    mesh.RecalculateNormals();
    mesh.RecalculateBounds();

    return mesh;
  }
}