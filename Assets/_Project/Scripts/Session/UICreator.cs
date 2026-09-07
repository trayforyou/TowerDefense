using System;
using UnityEngine;
using static UnityEngine.Object;

namespace _Project.Scripts.Session
{
    public class UICreator
    {
        private readonly Canvas _canvas;

        public UICreator(Canvas canvas) =>
            _canvas = canvas;

        public T Create<T>(T component) where T : Component, IUIElement
        {
            if (component == null)
                throw new ArgumentNullException();

            return Instantiate(component, _canvas.transform);
        }
    }
}