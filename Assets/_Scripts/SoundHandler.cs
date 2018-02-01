using UnityEngine;
using System.Collections;

public class SoundHandler : MonoBehaviour
{
    #region public
    [Header("Impact")]
    public AudioSource paddleImpactSound = null;
	public AudioSource wallImpactSound = null;
	public AudioSource goalImpactSound = null;

    [Header("FX")]
    public AudioSource kickOffSound = null;
	public AudioSource playerReadySound = null;
	public AudioSource playerGoSound = null;

    [Header("UI")]
    public AudioSource uiHoverSound = null;
	public AudioSource uiKlickSound = null;

    [Header("Voices")]

    public AudioSource player1Ready = null;
    public AudioSource player2Ready = null;

    [Space]

    public AudioSource player1Wins = null;
    public AudioSource player2Wins = null;

    [Space]

    public AudioSource _3_2_1_Start = null;
    #endregion

    private AudioSource audioSource = null;

    public static SoundHandler Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }

        Instance = this;

        DontDestroyOnLoad(gameObject);

        audioSource = GetComponent<AudioSource>();
    }

    public void PlayAudio(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Play();
    }
}