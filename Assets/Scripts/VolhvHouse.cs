using UnityEngine;

public class VolhvHouse : MonoBehaviour
{
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        AudioSetting.Instance.RegisterSfx(audioSource);
    }

    private void startSound()
    {
        audioSource.Play();
    }
}
