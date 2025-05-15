using UnityEngine;
using System.IO;
using static SceneController;

public class SaverPosition : MonoBehaviour
{
    public static SaverPosition Instance { get; private set; }
    public VectorValue playerStorage;

    private string SavePath => Path.Combine(Application.persistentDataPath, "VectorValue.json");

    private void Awake()
    {
        Instance = this;
        Load();
    }
    public void Save(Vector2 position)
    {
        if (playerStorage == null)
        {
            Debug.LogError("VectorValue не назначен!");
            return;
        }

        VectorData saveData = new VectorData
        {
            x = position.x,
            y = position.y
        };
        string json = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(SavePath, json);
        Debug.Log("Сохранено в: " + SavePath);
    }
    private void Load()
    {
        if (File.Exists(SavePath))
        {
            string json = File.ReadAllText(SavePath);
            VectorData data = JsonUtility.FromJson<VectorData>(json);

            if (playerStorage != null)
            {
                playerStorage.initialValue = new Vector3(data.x, data.y);
                Debug.Log("Загружено: " + playerStorage.initialValue);
            }
        }
        else
        {
            Debug.Log("Файл сохранения не найден.");
        }
    }

    public void DestroySave()
    {
        if (Directory.Exists(Application.persistentDataPath))
        {
            foreach (string file in Directory.GetFiles(Application.persistentDataPath, "*.json"))
            {
                File.Delete(file);
            }
        }
    }
}
