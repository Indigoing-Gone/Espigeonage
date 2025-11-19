using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent (typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{

    public static SoundManager Instance;

    #region SFX

    [SerializeField] private AudioClip objPickUp;
    [SerializeField] private AudioClip objPlaceShelf;
    [SerializeField] private AudioClip objPlaceDesk;
    [SerializeField] private AudioClip objDrag;

    [SerializeField] private AudioClip deskEnter;
    [SerializeField] private AudioClip deskExit;

    [SerializeField] private AudioClip moveSpy;
    [SerializeField] private AudioClip draw;
    [SerializeField] private AudioClip undo;

    [SerializeField] private AudioClip openBook;
    [SerializeField] private AudioClip flipPage;
    [SerializeField] private AudioClip closeBook;

    [SerializeField] private List<AudioClip> pigeonSounds;
    [SerializeField] private AudioClip giveBlueprint;
    [SerializeField] private AudioClip recieveMessage;
    [SerializeField] private AudioClip missionSuccess;
    [SerializeField] private AudioClip missionFailure;

    public enum SFXType
    {
        OBJ_PICKUP,
        OBJ_PLACE_SHELF,
        OBJ_PLACE_DESK,
        OBJ_DRAG,

        DESK_ENTER,
        DESK_EXIT,

        MOVE_SPY,
        DRAW,
        UNDO,

        OPEN_BOOK,
        FLIP_PAGE,
        CLOSE_BOOK,

        PIGEON,
        GIVE_BLUEPRINT,
        RECIEVE_MESSAGE,
        MISSION_SUCCESS,
        MISSION_FAILURE
    }

    #endregion

    #region BGM

    private AudioSource bgmSource;

    [SerializeField ] private AudioClip officeBGM;

    public enum BGMType
    {
        OFFICE
    };

    #endregion

    #region Init

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        bgmSource = GetComponent<AudioSource>();
        bgmSource.loop = true;
    }

    #endregion

    public void PlaySFX(SFXType _type) { PlaySFX(_type, transform.position); }

    public void PlaySFX(SFXType _type, Vector3 position)
    {
        AudioClip _clip = _type switch
        {
            SFXType.OBJ_PICKUP => objPickUp,
            SFXType.OBJ_PLACE_SHELF => objPlaceShelf,
            SFXType.OBJ_PLACE_DESK => objPlaceDesk,
            SFXType.OBJ_DRAG => objDrag,

            SFXType.DESK_ENTER => deskEnter,
            SFXType.DESK_EXIT => deskExit,

            SFXType.MOVE_SPY => moveSpy,
            SFXType.DRAW => draw,
            SFXType.UNDO => undo,

            SFXType.OPEN_BOOK => openBook,
            SFXType.FLIP_PAGE => flipPage,
            SFXType.CLOSE_BOOK => closeBook,

            SFXType.PIGEON => pigeonSounds[UnityEngine.Random.Range(0, pigeonSounds.Count)],
            SFXType.GIVE_BLUEPRINT => giveBlueprint,
            SFXType.RECIEVE_MESSAGE => recieveMessage,
            SFXType.MISSION_SUCCESS => missionSuccess,
            SFXType.MISSION_FAILURE => missionFailure,

            _ => throw new ArgumentException(_type + " NOT A VALID SFX TYPE")
        };

        AudioSource.PlayClipAtPoint(_clip, position);
    }



    public void PlayBGM(BGMType _type)
    {
        bgmSource.Stop();

        bgmSource.clip = _type switch
        {
            BGMType.OFFICE => officeBGM,

            _ => throw new ArgumentException(_type + " NOT A VALID BGM TYPE")
        };

        bgmSource.Play();

    }

    public void StopBGM()
    {
        bgmSource.Stop();
    }
}
