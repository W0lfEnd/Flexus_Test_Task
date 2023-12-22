using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;


public class CameraShaker : MonoBehaviour
{
  #region Singleton Logic
  public static CameraShaker instance
  {
    get
    {
      if ( _instance == null )
      {
        Debug.LogError( $"Please add any of {typeof(CameraShaker)} to Scene" );
      }

      return _instance;
    }
  }

  private static CameraShaker _instance = null;
  #endregion

  #region Serialize Fields
  [SerializeField] private Transform cameraContainer = null;
  #endregion

  #region Private Fields
  Vector3    onStartPos = default;
  Quaternion onStartRot = default;

  private const float DEFAULT_SHAKE_POWER    = 0.03f;

  private const float DEFAULT_SHAKE_TIME_IN  = 0.1f;
  private const float DEFAULT_SHAKE_TIME_OUT = 0.25f;

  private const float SHAKE_ROTATION_INTENSITY_DECREASE_MULTIPLIER = 0.2f;
  private Coroutine shakeCoroutine = null;
  #endregion

  #region Public Properties
  public bool isShakePosition { get; set; } = true;
  public bool isShakeRotation { get; set; } = true;
  #endregion


  #region Unity Events
  private void Awake()
  {
    if ( _instance != null )
    {
      Debug.LogError( $"Trying to register Singleton object {name} but its already exists ({instance.name}). There should be only one instance of the object on the Scene!" );
      return;
    }

    _instance = this;
  }
  #endregion

  #region Public Methods
  public void shake( float shake_power = DEFAULT_SHAKE_POWER, float timeIn = DEFAULT_SHAKE_TIME_IN, float timeOut = DEFAULT_SHAKE_TIME_OUT )
  {
    forceStop();

    shakeCoroutine = StartCoroutine( shakeAnimation( shake_power, timeIn, timeOut ) );
  }

  public void forceStop()
  {
    if ( shakeCoroutine != null )
    {
      StopCoroutine( shakeCoroutine );
      shakeCoroutine = null;
    }

    resetContainer();
  }
  #endregion

  #region Private Methods
  private IEnumerator shakeAnimation( float shakePower, float timeIn, float timeOut )
  {
    float curShakeIntensity = 0.0f;
    float timer = 0.0f;
    onStartPos = cameraContainer.localPosition;
    onStartRot = cameraContainer.localRotation;

    while ( timer < timeIn + timeOut )
    {
      if ( timer <  timeIn )
        curShakeIntensity = Mathf.Lerp( 0.0f, shakePower, timer / timeIn );
      else
        curShakeIntensity = Mathf.Lerp( shakePower, 0.0f , (timer - timeIn) / timeOut );

      if ( isShakePosition )
        cameraContainer.localPosition = onStartPos + Random.insideUnitSphere * curShakeIntensity;

      if ( isShakeRotation )
      {
        float rand_offset = Random.Range( -curShakeIntensity, curShakeIntensity ) * SHAKE_ROTATION_INTENSITY_DECREASE_MULTIPLIER;
        cameraContainer.localRotation = new Quaternion(
          onStartRot.x + rand_offset,
          onStartRot.y + rand_offset,
          onStartRot.z + rand_offset,
          onStartRot.w + rand_offset
        );
      }

      yield return null;

      timer += Time.unscaledDeltaTime;
    }

    resetContainer();
  }

  private void resetContainer()
  {
    cameraContainer.localPosition = onStartPos;
    cameraContainer.localRotation = onStartRot;
  }
  #endregion
}