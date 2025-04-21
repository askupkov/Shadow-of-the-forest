using UnityEngine;

public class SceneFader : MonoBehaviour
{
    public static SceneFader Instance { get; private set; }
    private Animator animator;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void FadeToLevel()
    {
        animator.SetBool("Fade", true);
    }

    public void FadeFromLevel()
    {
        animator.SetBool("Fade", false);
    }
}
