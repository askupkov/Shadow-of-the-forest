using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public float detectionRadius = 5f; // ������ �����������
    public LayerMask playerLayer; // ���� ������
    public NoiseManager noiseManager; // ������ �� �������� ����

    private void Update()
    {
        CheckForPlayer();
    }

    private void CheckForPlayer()
    {
        // ���������, ��������� �� ����� � ���� ���������
        Collider2D player = Physics2D.OverlapCircle(transform.position, detectionRadius, playerLayer);

        if (player != null)
        {
            // ���� ����� � ���� ��������� � ������� ���� �������
            if (noiseManager.noiseBarFill.fillAmount > 0.7f)
            {
                Debug.Log("�������� ������� ������!");
                GameOver();
            }
        }
    }

    private void GameOver()
    {
        
    }

    private void OnDrawGizmosSelected()
    {
        // ������������ ���� ����������� � ���������
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
