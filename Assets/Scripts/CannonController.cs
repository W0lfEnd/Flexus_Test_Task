using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class CannonController : MonoBehaviour
{
    #region Serialized Fields
    [SerializeField] private LineTrajectory     projectileLineTrajectory                = null;
    [SerializeField] private CannonTubeAnimator cannonTubeAnimator                      = null;
    [SerializeField] private Transform          tr_CannonTubePivot                      = null;
    [SerializeField] private GameObject         go_prefabHitEffect                      = null;
    [SerializeField] private GameObject         go_prefabProjectileExplosionEffect = null;
    [SerializeField] private Material           mat_Projectile                          = null;

    [Header("Shot Settings")]
    [SerializeField] private float shotPower          = 10.0f;
    [SerializeField] private float trajectoryLength   = 10.0f;
    [SerializeField] private int   maxBouncesCount    = 5;
    [SerializeField] private float bounceVelocityCoef = 0.7f;

    [Header("Projectile Settings")]
    [SerializeField] private float projectileSpeed  = 10.0f;
    [SerializeField] private float projectileRadius = 3.0f;

    [Header( "Speed of change input values")]
    [SerializeField] private float   shotPowerSpeedChange = 10.0f;
    [SerializeField] private Vector2 rotSpeedInDeg        = new Vector2( 90.0f, 90.0f );
    
    [Header( "Limits")]
    [SerializeField] private Vector2 shotPowerLimits = new Vector2( 10f, 40.0f );
    [SerializeField] private Vector2 xRotLimitsInDeg = new Vector2( 10.0f, 90.0f );
    #endregion

    #region Private Fields
    private List<(List<Vector3> path, RaycastHit hit)> curTrajectory = new List<(List<Vector3>, RaycastHit)>();
    #endregion


    #region Unity Events
    private void Update()
    {
        updateTrajectoryLine();

        handleShotPowerInput( Time.deltaTime );
        tryToRotateCannon( handleCannonRotationInput() * Time.deltaTime );
        handleShotInput();
    }

    private void OnDrawGizmos()
    {
        // if ( Application.isPlaying )
        //     return;

        updateTrajectoryLine();
    }
    #endregion

    #region Private Methods
     private void handleShotPowerInput( float deltaTime )
    {
        if ( Input.GetKey( KeyCode.W ) )
            shotPower += ( shotPowerSpeedChange * deltaTime );
        else if ( Input.GetKey( KeyCode.S ) )
            shotPower -= ( shotPowerSpeedChange * deltaTime );

        shotPower = shotPower.toRange( shotPowerLimits.x, shotPowerLimits.y );
    }

    private Vector2 handleCannonRotationInput()
    {
        Vector2 rotationVector = Vector2.zero;
        if ( Input.GetKey( KeyCode.DownArrow ) )
            rotationVector.x += rotSpeedInDeg.x;
        else if ( Input.GetKey( KeyCode.UpArrow ) )
            rotationVector.x -= rotSpeedInDeg.x;

        if ( Input.GetKey( KeyCode.LeftArrow ) )
            rotationVector.y -= rotSpeedInDeg.y;
        else if ( Input.GetKey( KeyCode.RightArrow ) )
            rotationVector.y += rotSpeedInDeg.y;

        return rotationVector;
    }

    private void handleShotInput()
    {
        if ( !cannonTubeAnimator.isRecoilAnimationPlaying && Input.GetKeyDown( KeyCode.Space ) )
            doShot();
    }

    private void tryToRotateCannon( Vector2 rotation )
    {
        transform.Rotate( 0.0f, rotation.y, 0.0f );

        float futureXRot = (tr_CannonTubePivot.localEulerAngles.x + rotation.x).toRange( xRotLimitsInDeg.x, xRotLimitsInDeg.y );
        tr_CannonTubePivot.localEulerAngles = tr_CannonTubePivot.localEulerAngles.setX( futureXRot );
    }

    private void doShot()
    {
        cannonTubeAnimator.play();
        CameraShaker.instance.shake();

        instantiateProjectile()
            .init( curTrajectory, projectileSpeed, go_prefabHitEffect, go_prefabProjectileExplosionEffect );
    }

    private ProjectileController instantiateProjectile()
    {
        GameObject newProjectile = new GameObject();

        MeshFilter meshFilter = newProjectile.AddComponent<MeshFilter>();

        Mesh projectileMesh = MeshHelper.generateSphereMesh( projectileRadius );
        meshFilter.mesh = projectileMesh;

        newProjectile.AddComponent<MeshRenderer>()
                     .material = mat_Projectile;

        return newProjectile.AddComponent<ProjectileController>();
    }

    private void updateTrajectoryLine()
    {
        curTrajectory = PhysicsHelper.traceParabolicPathWithBounces(
            tr_CannonTubePivot.position
          , tr_CannonTubePivot.forward * shotPower
          , trajectoryLength
          , Physics.gravity.y
          , projectileLineTrajectory.SegmentsCount
          , bounceVelocityCoef
          , maxBouncesCount
        );

        List<Vector3> aggregatedTrajectory = curTrajectory.SelectMany( it => it.path ).ToList();
        projectileLineTrajectory.init( aggregatedTrajectory );
    }
    #endregion
}
