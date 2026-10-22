using UnityEngine;

namespace _Project.Scripts.Savers
{
    public class Saver
    {
        private readonly string _moneyKey = "money";
        private readonly string _levelsKey = "levels";

        public void SaveMoney(SaveDataMoney dataMoney) =>
            Save(dataMoney, _moneyKey);

        public void SaveLevels(SaveDataLevels dataLevels) =>
            Save(dataLevels, _levelsKey);

        public SaveDataMoney LoadDataMoney() =>
            Load<SaveDataMoney>(_moneyKey);

        public SaveDataLevels LoadDataLevels() =>
            Load<SaveDataLevels>(_levelsKey);

        private void Save<T>(T saveData, string key) where T : ISaveData
        {
            string json = JsonUtility.ToJson(saveData);
            PlayerPrefs.SetString(key, json);
            PlayerPrefs.Save();
        }

        private T Load<T>(string key) where T : ISaveData, new()
        {
            if (PlayerPrefs.HasKey(key))
            {
                string json = PlayerPrefs.GetString(key);

                return JsonUtility.FromJson<T>(json);
            }

            return new T();
        }
    }
}