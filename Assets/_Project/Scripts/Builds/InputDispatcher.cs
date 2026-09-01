using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _Project.Scripts.Builds
{
    public class InputDispatcher : IDisposable
    {
        private readonly CancellationTokenSource _tokenSource;

        public event Action OnPrimaryClick;

        public InputDispatcher()
        {
            _tokenSource = new CancellationTokenSource();
            Update(_tokenSource.Token).Forget();
        }

        public bool IsPointerOverGameObject() => 
            EventSystem.current.IsPointerOverGameObject();

        public void Dispose() => 
            _tokenSource.Clear();

        public Vector3 GetPointToRay() => 
            Input.mousePosition;
        
        private async UniTaskVoid Update(CancellationToken token)
        {
            try
            {
                while (!token.IsCancellationRequested)
                {
                    if (Input.GetMouseButtonDown(0))
                        OnPrimaryClick?.Invoke();
                    
                    await UniTask.Yield(PlayerLoopTiming.Update, cancellationToken: token);
                }
            }
            catch (OperationCanceledException)
            {
            }
        }
    }
}