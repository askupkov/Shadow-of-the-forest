using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public float detectionRadius = 5f; // Радиус обнаружения
    public LayerMask playerLayer; // Слой игрока
    public NoiseManager noiseManager; // Ссылка на менеджер шума

    private void Update()
    {
        CheckForPlayer();
    }

    private void CheckForPlayer()
    {
        // Проверяем, находится ли игрок в зоне видимости
        Collider2D player = Physics2D.OverlapCircle(transform.position, detectionRadius, playerLayer);

        if (player != null)
        {
            // Если игрок в зоне видимости и уровень шума высокий
            if (noiseManager.noiseBarFill.fillAmount > 0.7f)
            {
                Debug.Log("Стражник заметил игрока!");
                GameOver();
            }
        }
    }

    private void GameOver()
    {
        
    }

    private void OnDrawGizmosSelected()
    {
        // Визуализация зоны обнаружения в редакторе
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
