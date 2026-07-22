using UnityEngine;

namespace TowerDefense.Saver
{
    public class Saver : MonoBehaviour
    {
        private readonly string _key = "data";
        
        public void Save(SaveData data)
        {
            string json = JsonUtility.ToJson(data);
            PlayerPrefs.SetString(_key, json);
            PlayerPrefs.Save();
        }

        public SaveData Load()
        {
            string json = PlayerPrefs.GetString(_key);

            if (string.IsNullOrEmpty(json))
                return new SaveData(0);
            
            return JsonUtility.FromJson<SaveData>(json);
        }
    }
}