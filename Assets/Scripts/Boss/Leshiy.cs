using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Tilemaps;

public class Leshiy : MonoBehaviour
{
    [SerializeField] Animator animatorLeshiy;
    [SerializeField] Animator RootsLeft;
    [SerializeField] Animator RootsRight;
    private bool playerInRange;

    [SerializeField] private GameObject spikePrefab;
    [SerializeField] private Collider2D[] spawnAreas;
    private int spikesCount = 20; // Количество шипов за атаку
    [SerializeField] private float spikeLifetime; // Время жизни шипов
    private float nextAttackTime = 0f;
    private float attackRate = 1f;
    private List<GameObject> activeSpikes = new List<GameObject>();
    [SerializeField] private float minDistance = 2f;

    private void Start()
    {
        StartCoroutine(ExecuteWave());
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

    private IEnumerator ExecuteWave()
    {
        yield return new WaitForSeconds(1f);
        string[] attackTypes = { "Lightning", "Spikes", "RootsLeft", "RootsRight", "RootsHit" };
        int waveLength = 50; // Количество атак в волне

        for (int i = 0; i < waveLength; i++)
        {
            string attackType = attackTypes[Random.Range(0, attackTypes.Length)];

            switch (attackType)
            {
                case "Lightning":
                    Lightning.Instance.TriggerLightningAttack();
                    break;

                case "Spikes":
                    TriggerSpikesAttack();
                    break;

                case "RootsLeft":
                    animatorLeshiy.SetTrigger("Left");
                    break;

                case "RootsRight":
                    animatorLeshiy.SetTrigger("Right");
                    break;

                case "RootsHit":
                    animatorLeshiy.SetTrigger("Hit");
                    break;
            }

            yield return new WaitForSeconds(2f);
        }
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


    public void TriggerSpikesAttack()
    {
        StartCoroutine(SpawnSpikes());
    }

    private IEnumerator SpawnSpikes()
    {
        for (int i = 0; i < spikesCount; i++)
        {
            SpawnSingleSpike();
            yield return new WaitForSeconds(0.2f);
        }
    }

    private void SpawnSingleSpike()
    {
        Vector2 randomPosition = GetRandomPointInZone();
        GameObject spike = Instantiate(spikePrefab, randomPosition, Quaternion.identity);
        Destroy(spike, spikeLifetime);
    }

    public Vector2 GetRandomPointInZone()
    {
        Collider2D randomCollider = spawnAreas[Random.Range(0, spawnAreas.Length)];
        Bounds bounds = randomCollider.bounds;

        float x = Random.Range(bounds.min.x, bounds.max.x);
        float y = Random.Range(bounds.min.y, bounds.max.y);

        return new Vector2(x, y);
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
