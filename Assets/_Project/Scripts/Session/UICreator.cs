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

        public IUIElement Create(Component component)
        {
            if (component is not IUIElement)
                throw new ArgumentException();

            return (IUIElement)Instantiate(component, _canvas.transform);
        }
    }
}