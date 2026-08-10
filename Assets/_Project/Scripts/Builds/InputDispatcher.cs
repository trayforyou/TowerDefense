using System;
using UnityEngine;

namespace _Project.Scripts.Builds
{
    public class InputDispatcher : MonoBehaviour
    {
        public event Action OnPrimaryClick;

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
                OnPrimaryClick?.Invoke();
        }
    }
}