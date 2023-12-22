using System;
using System.Collections.Generic;
using UnityEngine;


public class LineTrajectory : MonoBehaviour
{
  #region Serialized Fields
  [SerializeField] private LineRenderer lineRenderer  = null;
  [SerializeField] private int          segmentsCount = 20;
  #endregion

  #region Public Properties
  public int SegmentsCount => segmentsCount;
  #endregion


  #region Public Methods
  public Vector3[] init( Vector3 start, Vector3 velocity, float simulationTime, float gravity )
  {
    if ( lineRenderer == null || segmentsCount <= 0 )
      return Array.Empty<Vector3>();

    Vector3[] pathPoints = MathHelper.traceParabolicPath( start, velocity, simulationTime, gravity, segmentsCount );
    lineRenderer.positionCount = pathPoints.Length;
    lineRenderer.SetPositions( pathPoints );

    return pathPoints;
  }

  public void init( List<Vector3> points )
  {
    if ( points == null || points.Count == 0 )
      return;

    lineRenderer.positionCount = points.Count;
    lineRenderer.SetPositions( points.ToArray() );
  }
  #endregion
}