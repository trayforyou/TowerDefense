using UnityEngine;

namespace _Project.Scripts.Savers
{
    public class Saver
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
            if (PlayerPrefs.HasKey(_key))
            {
                string json = PlayerPrefs.GetString(_key);

                return JsonUtility.FromJson<SaveData>(json);
            }

            return new SaveData(0);
        }
    }
}