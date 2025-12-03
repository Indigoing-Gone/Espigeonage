using System;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Pigeon : MonoBehaviour
{
    public static Action PigeonReady;

    [SerializeField] private float flyTime;
    private Transform startTransform;
    private Transform perchTransform;

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

    public void Init(Grabbable _note, Transform _start, Transform _end)
    {
        startTransform = _start;
        perchTransform = _end;
        missionGrabber = GetComponentInChildren<MissionGrabber>();
        GiveNote(_note, true);
    }

    public void GiveNote(Grabbable _note, bool isMission)
    {
        if (missionGrabber == null) return;

        hasMission = isMission;

        missionGrabber.Release();
        missionGrabber.SetGrabbable(_note);
        missionGrabber.Grab();

        StartCoroutine(FlyRoutine(startTransform, perchTransform));
    }

    private void DestroyHeldNote()
    {
        Destroy((missionGrabber.Release() as MonoBehaviour).gameObject);
    }

    #endregion

    #region Pigeon Behavior
    
    private void Coo()
    {
        //SoundManager.Instance.PlaySFX(SoundManager.SFXType.PIGEON, transform.position);
    }

    // Easing function for the pigeon's movement from start to end transform
    private float PigeonEase(float _per)
    {
        // Cubic easing (can be changed)
        return 1.0f - Mathf.Pow(1.0f - _per, 3);
    }

    private IEnumerator FlyRoutine(Transform _start, Transform _end)
    {
        transform.SetPositionAndRotation(_start.position, _start.rotation);
        Coo();
        for (float i = 0; i < flyTime; i += Time.deltaTime)
        {
            float per = PigeonEase(i / flyTime);
            transform.SetPositionAndRotation(Vector3.Lerp(_start.position, _end.position, per),
                                             Quaternion.Lerp(_start.rotation, _end.rotation, per));
            yield return null;
        }
        transform.SetPositionAndRotation(_end.position, _end.rotation);
        Coo();
        yield break;
    }

    public void OnSendPigeon()
    {
        if (transform.position != perchTransform.position) return;
        FlyAway();
    }

    public void FlyAway()
    {
        StartCoroutine(FlyAwayRoutine());
    }

    private IEnumerator FlyAwayRoutine()
    {
        yield return StartCoroutine(FlyRoutine(perchTransform, startTransform));

        if (hasMission)
        {
            missionGrabber.CompleteMission();
            hasMission = false;
            DestroyHeldNote();
        }
        else
        {
            DestroyHeldNote();
            PigeonReady.Invoke();
        }
    }
    #endregion
}
