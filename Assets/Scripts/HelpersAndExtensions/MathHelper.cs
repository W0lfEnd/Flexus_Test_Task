using UnityEngine;


public static class MathHelper
{
  public static Vector3[] traceParabolicPath( Vector3 start, Vector3 velocity, float simulationTime, float gravity, int precision )
  {
    int       pointsCount = precision + 1;
    Vector3[] pathPoints  = new Vector3[pointsCount];
    for ( int i = 0; i <= precision; i++ )
    {
      float t = i * simulationTime / precision;
      pathPoints[i] = getPositionForParabolicPath( start, velocity, t, gravity );
    }

    return pathPoints;
  }

  public static Vector3 getPositionForParabolicPath( Vector3 start, Vector3 velocity, float t, float gravity )
  {
    float x = start.x + velocity.x * t;
    float y = start.y + velocity.y * t + gravity * t * t / 2.0f;
    float z = start.z + velocity.z * t;

    return new Vector3( x, y, z );
  }
}