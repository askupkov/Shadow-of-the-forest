using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance { get; private set; }

    // Пример объекта, который мы будем сохранять
    public GameObject player;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject); // Уничтожаем дубликат
        }
    }
    public void DestroyPersistentObject()
    {
        if (Instance != null)
        {
            Destroy(Instance.gameObject);
            Instance = null; // Очищаем ссылку
        }
    }

}

