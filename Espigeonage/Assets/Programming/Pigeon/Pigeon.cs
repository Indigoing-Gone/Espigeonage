using System;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Splines;

public class Pigeon : MonoBehaviour
{
    public static Action PigeonReady;

    private SplineContainer toPerch;
    private SplineContainer toAgent;
    private bool perched = false;

    private SplineAnimate splineAnimator;
    private MissionGrabber missionGrabber;

    private bool hasMission = true;

    #region Events

    private void OnEnable()
    {
        GameManager.SendNote += GiveNote;
        PigeonSender.SendPigeonEvent += OnSendPigeon;
    }

    private void OnDisable()
    {
        GameManager.SendNote -= GiveNote;
        PigeonSender.SendPigeonEvent -= OnSendPigeon;
    }

    #endregion

    #region Init

    public void Init(IGrabbable _note, SplineContainer _toPerch, SplineContainer _toAgent)
    {
        toPerch = _toPerch;
        toAgent = _toAgent;
        splineAnimator = GetComponent<SplineAnimate>();
        splineAnimator.Loop = SplineAnimate.LoopMode.Once;
        missionGrabber = GetComponentInChildren<MissionGrabber>();
        GiveNote(_note, true);
    }

    public void GiveNote(IGrabbable _note, bool isMission)
    {
        if (missionGrabber == null) return;

        hasMission = isMission;

        missionGrabber.Release();
        missionGrabber.SetGrabbable(_note);
        missionGrabber.Grab();

        StartCoroutine(GiveNoteRoutine());
    }

    private void DestroyHeldNote()
    {
        MonoBehaviour _grabbable = missionGrabber.Release() as MonoBehaviour;
        if (_grabbable != null) Destroy(_grabbable.gameObject);
    }

    #endregion

    #region Pigeon Behavior
    
    private void Coo()
    {
        //SoundManager.Instance.PlaySFX(SoundManager.SFXType.PIGEON, transform.position);
    }

    private IEnumerator FlyRoutine(SplineContainer _route)
    {
        splineAnimator.Container = _route;
        Coo();
        splineAnimator.Restart(false);
        splineAnimator.Play();
        yield return new WaitWhile(() => splineAnimator.IsPlaying);
        Coo();
        yield break;
    }

    public void OnSendPigeon()
    {
        if (!perched || 
           (hasMission && !missionGrabber.HasGrabbable)) return;
        FlyAway();
    }

    public void FlyAway()
    {
        StartCoroutine(FlyAwayRoutine());
        perched = false;
    }

    private IEnumerator GiveNoteRoutine()
    {
        yield return StartCoroutine(FlyRoutine(toPerch));
        perched = true;
    }

    private IEnumerator FlyAwayRoutine()
    {
        yield return StartCoroutine(FlyRoutine(toAgent));

        if (hasMission)
        {
            missionGrabber.CompleteMission();
            //hasMission = false;
            DestroyHeldNote();
            PigeonReady.Invoke();
        }
        else
        {
            DestroyHeldNote();
            PigeonReady.Invoke();
        }
    }
    #endregion
}
