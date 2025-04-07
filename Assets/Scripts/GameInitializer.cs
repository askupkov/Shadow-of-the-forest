using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NewBehaviourScript : MonoBehaviour
{
    public GameObject persistentPrefab;
    [SerializeField] int scene;

    void Start()
    {
        // Инстанциируем персистентный объект
        Instantiate(persistentPrefab);

        SceneManager.LoadScene(scene);
    }
}
