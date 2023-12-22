using System.Collections.Generic;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;


public static class PhysicsHelper
{
  public static List<(List<Vector3>, RaycastHit)> traceParabolicPathWithBounces( Vector3 start, Vector3 velocity, float simulationTime, float gravity, int precisionPerBounce, float bounceVelocityCoef = 0.5f, int maxBouncesCount = 0 )
  {
    List<(List<Vector3>, RaycastHit)> listOfPathes = new List<(List<Vector3>, RaycastHit)>();

    Vector3 curStartPoint      = start;
    Vector3 curVelocity        = velocity;

    for ( int i = 0; i <= maxBouncesCount; i++ )
    {
      List<Vector3> pathPoints = traceParabolicPath( curStartPoint, curVelocity, simulationTime, gravity, precisionPerBounce, out RaycastHit hit, out Vector3 reflectDirection );
      if ( pathPoints == null || pathPoints.Count < 2 )
        break;

      listOfPathes.Add( ( pathPoints, hit ) );
      if ( hit.collider == null )
        break;

      curStartPoint = hit.point;
      curVelocity = reflectDirection.normalized * curVelocity.magnitude * bounceVelocityCoef;
    }

    return listOfPathes;
  }

  public static List<Vector3> traceParabolicPath( Vector3 start, Vector3 velocity, float simulationTime, float gravity, int precision, out RaycastHit hit, out Vector3 reflectDirection )
  {
    hit = default;
    reflectDirection = default;

    int           pointsCount = precision;
    List<Vector3> pathPoints  = new List<Vector3>( pointsCount );
    pathPoints.Add( start );
    Vector3 prevPoint = start;
    for ( int i = 0; i <= precision; i++ )
    {
      float t = ( i + 1 ) * simulationTime / precision;

      Vector3 iPoint           = MathHelper.getPositionForParabolicPath( start, velocity, t, gravity );
      Vector3 raycastDirection = iPoint - prevPoint;

      if ( Physics.Raycast( prevPoint, raycastDirection, out hit, raycastDirection.magnitude ) )
      {
        reflectDirection = Vector3.Reflect( raycastDirection, hit.normal );
        hit.point += hit.normal * 0.001f;
        return pathPoints;
      }

      pathPoints.Add( iPoint );
      prevPoint = iPoint;
    }


    return pathPoints;
  }
}