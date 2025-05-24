using UnityEngine;

public class SurfaceZone : MonoBehaviour
{
    public static SurfaceZone Instance { get; private set; }
    public string surface;
    private AudioSource audioSource;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        AudioSetting.Instance.RegisterMusic(audioSource);
    }
}
