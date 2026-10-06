using System;
using Cysharp.Threading.Tasks;
using Firebase;
using Firebase.Analytics;
using UnityEngine;

namespace _Project.Scripts.Analytics.Firebase
{
    public class FirebaseCreator
    {
        public async UniTask<FirebaseApp> Initialize()
        {
            Debug.Log("Запуск инициализации игры...");

            // 1. Получаем инстанс (из кэша или через новую проверку)
            FirebaseApp app = await GetFireBase();

            if (app != null)
            {
                Debug.Log("Firebase успешно готов к работе!");

                SendTestAnalytics();

                return app;
            }
            else
            {
                throw new Exception("Инициализация сорвалась. Ошибка сети.");
            }
        }

        private async UniTask<FirebaseApp> GetFireBase()
        {
            try
            {
                if (FirebaseApp.DefaultInstance != null)
                {
                    Debug.Log("Firebase уже инициализирован ранее. Возвращаем существующий инстанс.");
                    return FirebaseApp.DefaultInstance;
                }
            }
            catch (System.InvalidOperationException)
            {
            }

            Debug.Log("Первый запуск: проверка зависимостей Firebase Google Play Services...");
            var dependencyStatus = await FirebaseApp.CheckAndFixDependenciesAsync().AsUniTask();

            if (dependencyStatus == DependencyStatus.Available)
            {
                return FirebaseApp.DefaultInstance;
            }
            else
            {
                Debug.LogError($"Не удалось исправить зависимости Firebase: {dependencyStatus}");
                return null;
            }
        }

        private void SendTestAnalytics()
        {
            Debug.Log("[Analytics] Вызываем метод отправки события из C#...");

            // Отправляем уникальное событие, чтобы его было легко найти в панели
            FirebaseAnalytics.LogEvent("pc_test_session_start");

            // Отправляем событие с параметрами
            FirebaseAnalytics.LogEvent("pc_test_click", new Parameter[]
            {
                new Parameter("button_name", "test_button"),
                new Parameter("os_platform", "windows_editor")
            });
        }

        private void StartGame(FirebaseApp firebaseApp)
        {
            // Логика запуска игры (например, загрузка меню или передача firebaseApp дальше)
        }
    }
}