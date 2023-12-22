using System.Collections;
using UnityEngine;


public class CannonTubeAnimator : MonoBehaviour
{
    #region Serialized Fields
    [SerializeField] private Transform tr_Tube     = null;
    [SerializeField] private float     recoilPower = 1.0f;
    [SerializeField] private float     recoilTime  = 0.3f;
    #endregion

    #region Private Fields
    private Vector3   tubePositionOnAnimStart  = Vector3.zero;
    private Coroutine recoilAnimationCoroutine = null;
    #endregion

    #region Public Properties
    public bool isRecoilAnimationPlaying => recoilAnimationCoroutine != null;
    #endregion


    #region Public Methods
    public void play()
    {
        if ( recoilAnimationCoroutine != null )
        {
            StopCoroutine( recoilAnimationCoroutine );
            tr_Tube.localPosition = tubePositionOnAnimStart;
        }

        tubePositionOnAnimStart = tr_Tube.localPosition;
        recoilAnimationCoroutine = StartCoroutine( recoilAnimation() );
    }
    #endregion

    #region Private Methods
    private IEnumerator recoilAnimation()
    {
        float halfRecoilTime = recoilTime / 2f;

        yield return moveFromTo( tr_Tube.localPosition, tubePositionOnAnimStart.plusZ( -recoilPower ), halfRecoilTime );
        yield return moveFromTo( tr_Tube.localPosition, tubePositionOnAnimStart,                       halfRecoilTime );

        recoilAnimationCoroutine = null;
    }

    private IEnumerator moveFromTo( Vector3 from, Vector3 to, float time )
    {
        float timer = 0.0f;
        while ( timer <= time )
        {
            tr_Tube.localPosition = Vector3.Lerp( from, to, Mathf.Clamp01( timer / time ) );
            timer += Time.deltaTime;
            yield return null;
        }
        
        tr_Tube.localPosition = to;
    }
    #endregion
}
