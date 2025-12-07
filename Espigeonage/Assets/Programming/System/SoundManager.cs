using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{

    public static SoundManager Instance;

    #region SFX

    [SerializeField] private AudioSource sfxSource;

    [SerializeField] private AudioClip objPickUp;
    [SerializeField] private AudioClip objPlaceShelf;
    [SerializeField] private AudioClip objPlaceDesk;

    [SerializeField] private List<AudioClip> moveSounds;
    [SerializeField] private AudioClip undo;

    [SerializeField] private AudioClip openBook;
    [SerializeField] private AudioClip flipPage;
    [SerializeField] private AudioClip closeBook;

    [SerializeField] private List<AudioClip> pigeonSounds;

    public enum SFXType
    {
        OBJ_PICKUP,
        OBJ_PLACE_SHELF,
        OBJ_PLACE_DESK,

        MOVE_SPY,
        UNDO,

        OPEN_BOOK,
        FLIP_PAGE,
        CLOSE_BOOK,

        PIGEON,
    }

    #endregion

    #region BGM

    [SerializeField] private AudioSource bgmSource;

    [SerializeField] private AudioClip officeBGM;

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
        DontDestroyOnLoad(gameObject);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sfxSource = GetComponent<AudioSource>();

        bgmSource.loop = true;
        PlayBGM(BGMType.OFFICE);
    }

    #endregion

    public T PickRandom<T>(List<T> _list) { return _list[UnityEngine.Random.Range(0, _list.Count)]; }

    public void PlaySFX(SFXType _type)
    {
        AudioClip _clip = _type switch
        {
            SFXType.OBJ_PICKUP => objPickUp,
            SFXType.OBJ_PLACE_SHELF => objPlaceShelf,
            SFXType.OBJ_PLACE_DESK => objPlaceDesk,

            SFXType.MOVE_SPY => PickRandom(moveSounds),
            SFXType.UNDO => undo,

            SFXType.OPEN_BOOK => openBook,
            SFXType.FLIP_PAGE => flipPage,
            SFXType.CLOSE_BOOK => closeBook,

            SFXType.PIGEON => PickRandom(pigeonSounds),

            _ => throw new ArgumentException(_type + " NOT A VALID SFX TYPE")
        };

        if (_clip == null) return;

        sfxSource.PlayOneShot(_clip);
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
