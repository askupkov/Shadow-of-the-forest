using System.Collections;
using UnityEngine;

public class Leshiy : MonoBehaviour
{
    [SerializeField] Animator animatorLeshiy;
    [SerializeField] Animator RootsLeft;
    [SerializeField] Animator RootsRight;
    [SerializeField] TextAsset inkJSON;
    private bool playerInRange;

    [SerializeField] GameObject spikePrefab;
    [SerializeField] Collider2D[] spawnAreas;
    [SerializeField] AudioClip[] sounds;
    private int spikesCount = 15; // ���������� ����� �� �����
    [SerializeField] private float spikeLifetime; // ����� ����� �����
    private float nextAttackTime = 0f;
    private float attackRate = 1f;
    private int waveLength;
    private AudioSource audioSource;

    private void Start()
    {
        StartCoroutine(FirstWave());
        audioSource = GetComponent<AudioSource>();
        AudioSetting.Instance.RegisterSfx(audioSource);
    }

    private void Update()
    {
        if (playerInRange)
        {
            RootsAttack();
        }
    }

    private void RootsAttack()
    {
        if (Time.time > nextAttackTime)
        {
            PlayerVisual.Instance.TriggerDamage();
            Healthbar.Instance.TakeDamage(10);
            nextAttackTime = Time.time + attackRate;
        }
    }

    private IEnumerator FirstWave()
    {
        yield return new WaitForSeconds(1f);
        string[] attackTypes = { "RootsLeft", "RootsRight" };
        string[] spikesTypes = { "Spikes1", "Spikes", "Spikes2" };
        waveLength = 15; // ���������� ���� � �����
        for (int i = 0; i < waveLength; i++)
        {
            string attackType = attackTypes[Random.Range(0, attackTypes.Length)];
            switch (attackType)
            {
                case "RootsLeft":
                    animatorLeshiy.SetTrigger("Left");
                    yield return new WaitForSeconds(0.3f);
                    audioSource.PlayOneShot(sounds[0]);
                    string spikesType = spikesTypes[Random.Range(0, 2)];
                    switch (spikesType)
                    {
                        case "Spikes":
                            TriggerSpikesAttack(0);
                            break;

                        case "Spikes1":
                            TriggerSpikesAttack(1);
                            break;
                    }

                    break;

                case "RootsRight":
                    animatorLeshiy.SetTrigger("Right");
                    yield return new WaitForSeconds(0.3f);
                    audioSource.PlayOneShot(sounds[0]);
                    spikesType = spikesTypes[Random.Range(1, 3)];
                    switch (spikesType)
                    {
                        case "Spikes":
                            TriggerSpikesAttack(0);
                            break;

                        case "Spikes2":
                            TriggerSpikesAttack(2);
                            break;
                    }
                    break;
            }
            yield return new WaitForSeconds(3f);
        }
        yield return new WaitForSeconds(2f);
        DialogueManager.Instance.StartDialog(inkJSON, "leshiy3");
        while (DialogueManager.Instance.dialogPanelOpen)
        {
            yield return null;
        }
        StartCoroutine(SecondWave());
    }

    private IEnumerator SecondWave()
    {
        yield return new WaitForSeconds(10f);
        string[] attackTypes = { "RootsLeft", "RootsRight" };
        string[] spikesTypes = { "Spikes1", "Spikes", "Spikes2" };
        waveLength = 20; // ���������� ���� � �����
        for (int i = 0; i < waveLength; i++)
        {
            string attackType = attackTypes[Random.Range(0, attackTypes.Length)];
            switch (attackType)
            {
                case "RootsLeft":
                    animatorLeshiy.SetTrigger("Left");
                    yield return new WaitForSeconds(0.3f);
                    audioSource.PlayOneShot(sounds[0]);
                    TriggerSpikesAttack(0);
                    TriggerSpikesAttack(1);
                    break;

                case "RootsRight":
                    animatorLeshiy.SetTrigger("Right");
                    yield return new WaitForSeconds(0.3f);
                    audioSource.PlayOneShot(sounds[0]);
                    TriggerSpikesAttack(0);
                    TriggerSpikesAttack(2);
                    break;
            }
            if(i % 5 == 0)
            {
                Startlight();
            }
            yield return new WaitForSeconds(3f);
        }
        StartCoroutine(ThirdWave());
    }

    private IEnumerator ThirdWave()
    {
        yield return new WaitForSeconds(10f);
        string[] attackTypes = { "LeftMid", "MidRight", "LeftRight" };
        waveLength = 20; // ���������� ���� � �����
        for (int i = 0; i < waveLength; i++)
        {
            string attackType = attackTypes[Random.Range(0, attackTypes.Length)];
            switch (attackType)
            {
                case "LeftMid":
                    animatorLeshiy.SetTrigger("Hit");
                    yield return new WaitForSeconds(0.3f);
                    audioSource.PlayOneShot(sounds[0]);
                    TriggerSpikesAttack(0);
                    TriggerSpikesAttack(1);
                    break;

                case "MidRight":
                    animatorLeshiy.SetTrigger("Hit");
                    yield return new WaitForSeconds(0.3f);
                    audioSource.PlayOneShot(sounds[0]);
                    TriggerSpikesAttack(0);
                    TriggerSpikesAttack(2);
                    break;

                case "LeftRight":
                    animatorLeshiy.SetTrigger("Hit");
                    yield return new WaitForSeconds(0.3f);
                    audioSource.PlayOneShot(sounds[0]);
                    TriggerSpikesAttack(1);
                    TriggerSpikesAttack(2);
                    break;
            }
            if (i % 4 == 0)
            {
                Startlight();
            }
            yield return new WaitForSeconds(3f);
        }
        yield return new WaitForSeconds(2f);
        LoadScene();
    }

    private void LoadScene()
    {
        PlayerPrefs.SetInt("finalScene", 1);
        PlayerPrefs.Save();
        SceneController.Instance.StartLoadScene(20);
    }

    private void Startlight()
    {
        Lightning.Instance.TriggerLightningAttack();
    }

    private void TriggerRight()
    {
        int handSide = Random.Range(0, 2);
        if (handSide == 0)
        {
            RootsRight.SetTrigger("Left");
        }
        else
        {
            RootsRight.SetTrigger("Right");
        }

    }

    private void TriggerLeft()
    {
        int handSide = Random.Range(0, 2);
        if (handSide == 0)
        {
            RootsLeft.SetTrigger("Left");
        }
        else
        {
            RootsLeft.SetTrigger("Right");
        }
    }

    public void TriggerSpikesAttack(int spawn)
    {
        StartCoroutine(SpawnSpikes(spawn));
    }

    private IEnumerator SpawnSpikes(int spawn)
    {
        if (spawn == 0)
        {
            spikesCount = 15;
        }
        else
        {
            spikesCount = 7;
        }
        for (int i = 0; i < spikesCount; i++)
        {
            Collider2D randomCollider = spawnAreas[spawn];
            Bounds bounds = randomCollider.bounds;

            audioSource.PlayOneShot(sounds[1]);

            float x = Random.Range(bounds.min.x, bounds.max.x);
            float y = Random.Range(bounds.min.y, bounds.max.y);
            Vector2 randomPosition = new Vector2(x, y);
            GameObject spike = Instantiate(spikePrefab, randomPosition, Quaternion.identity);
            Destroy(spike, spikeLifetime);
            yield return new WaitForSeconds(0.2f);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
}
