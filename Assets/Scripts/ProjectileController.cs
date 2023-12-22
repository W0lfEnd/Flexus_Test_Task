using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class ProjectileController : MonoBehaviour
{
  #region Private Fields
  private List<(List<Vector3> path, RaycastHit hit)> pathWithHits = null;

  private float      speed                      = 0.0f;
  private GameObject prefabProjectileHit        = null;
  private GameObject prefabFinalExplosionEffect = null;

  private const float DESTROY_PROJECTILE_HIT_TIMER = 10.0f;
  #endregion


  #region Public Methods
  public void init( List<(List<Vector3> path, RaycastHit hit)> pathWithHits, float speed, GameObject prefabProjectileHit, GameObject prefabFinalExplosionEffect )
  {
    this.pathWithHits        = pathWithHits;
    this.speed               = speed;
    this.prefabProjectileHit = prefabProjectileHit;
    this.prefabFinalExplosionEffect = prefabFinalExplosionEffect;

    if ( pathWithHits == null || pathWithHits.Count == 0 )
    {
      Debug.LogError( "That was strange. You tried to shot projectile without a path" );
      Destroy( gameObject );
      return;
    }

    StartCoroutine( moveAcrossThePath() );
  }
  #endregion

  #region Private Methods
  private IEnumerator moveAcrossThePath()
  {
    transform.position = pathWithHits[0].path[0];
    for ( int i = 0; i < pathWithHits.Count; i++ )
    {
      for ( int j = 0; j < pathWithHits[i].path.Count; j++ )
        yield return moveToPoint( pathWithHits[i].path[j] );

      if ( pathWithHits[i].hit.collider != null )
        spawnProjectileHit( pathWithHits[i].hit.point, pathWithHits[i].hit.normal );
    }

    yield return playExplosion( pathWithHits.LastOrDefault().hit.normal );

    Destroy( gameObject );
  }

  private void spawnProjectileHit( Vector3 position, Vector3 normal )
  {
    GameObject hitEffect = Instantiate( prefabProjectileHit );
    hitEffect.transform.position = position;
    hitEffect.transform.up = normal;
    
    Destroy( hitEffect, DESTROY_PROJECTILE_HIT_TIMER );
  }

  private IEnumerator moveToPoint( Vector3 point )
  {
    while ( Vector3.Distance( transform.position, point ) > 0.001f )
    {
      Vector3 distanceVector = point - transform.position;
      Vector3 direction      = distanceVector.normalized;
      Vector3 curVelocity    = direction * speed;
      Vector3 moveVector     = curVelocity * Time.deltaTime;
      if ( moveVector.magnitude > distanceVector.magnitude )
        moveVector = moveVector.normalized * distanceVector.magnitude;

      transform.Translate( moveVector );

      yield return null;
    }

    transform.position = point;
  }

  private IEnumerator playExplosion( Vector3 finalPointNormal )
  {
    GameObject effectGameObject = Instantiate( prefabFinalExplosionEffect, transform.position, Quaternion.identity );
    if ( finalPointNormal != default )
      effectGameObject.transform.up = finalPointNormal;

    yield return null;
  }
  #endregion
}