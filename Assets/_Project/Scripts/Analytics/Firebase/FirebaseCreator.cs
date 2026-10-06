using System;
using Cysharp.Threading.Tasks;
using Firebase;
using UnityEngine;

namespace _Project.Scripts.Analytics.Firebase
{
    public class FirebaseCreator
    {
        public async UniTask<FirebaseApp> Initialize()
        {
            FirebaseApp app = await GetFireBase();

            if (app != null)
                return app;

            throw new Exception("Инициализация сорвалась. Ошибка сети.");
        }

        private async UniTask<FirebaseApp> GetFireBase()
        {
            try
            {
                if (FirebaseApp.DefaultInstance != null)
                    return FirebaseApp.DefaultInstance;
            }
            catch (System.InvalidOperationException)
            {
            }
            
            var dependencyStatus = await FirebaseApp.CheckAndFixDependenciesAsync().AsUniTask();

            if (dependencyStatus == DependencyStatus.Available)
                return FirebaseApp.DefaultInstance;

            Debug.LogError($"Не удалось исправить зависимости Firebase: {dependencyStatus}");
            return null;
        }
    }
}