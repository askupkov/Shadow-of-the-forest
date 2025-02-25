using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    void Awake()
    {
        PlayerPrefs.DeleteAll();
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject); // ”ничтожаем дубликат
        }
    }
    public void DestroyPersistentObject()
    {
        if (Instance != null)
        {
            Destroy(Instance.gameObject);
            Instance = null; // ќчищаем ссылку
        }
    }

}

