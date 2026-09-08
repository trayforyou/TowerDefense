using System;
using _Project.Scripts.Builds.Castles;
using _Project.Scripts.Builds.Towers;
using UnityEngine;

namespace _Project.Scripts.Builds
{
    public class InteractHandler : IDisposable
    {
        private readonly LayerMask _groundLayer;
        private readonly LayerMask _castleLayer;
        private readonly CastleUpper _castleUpper;
        private readonly BuildHandler _buildHandler;
        private readonly Camera _mainCamera;
        private readonly InputDispatcher _inputDispatcher;

        public InteractHandler(LayerMask groundLayer, LayerMask castleLayer, CastleUpper castleUpper,
            BuildHandler buildHandler, Camera mainCamera, InputDispatcher inputDispatcher)
        {
            _inputDispatcher = inputDispatcher;
            _mainCamera = mainCamera;
            _castleUpper = castleUpper;
            _buildHandler = buildHandler;
            _groundLayer = groundLayer;
            _castleLayer = castleLayer;
            inputDispatcher.OnPrimaryClick += TryHandleClick;
        }

        private void TryHandleClick()
        {
            if (_buildHandler.IsActive == false && _castleUpper.IsActive == false)
            {
                if (_inputDispatcher.IsPointerOverGameObject())
                    return;

                HandleClick();
            }
        }

        private void HandleClick()
        {
            Ray ray = _mainCamera.ScreenPointToRay(_inputDispatcher.GetPointToRay());

            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, _castleLayer))
                _castleUpper.Activate();
            else if (Physics.Raycast(ray, out hit, Mathf.Infinity, _groundLayer))
                _buildHandler.Activate(hit.point);
        }

        public void Dispose() =>
            _inputDispatcher.OnPrimaryClick -= TryHandleClick;
    }
}