using _Project.Scripts.Builds.Castles;
using _Project.Scripts.Builds.Towers;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _Project.Scripts.Builds
{
    public class InteractHandler
    {
        private LayerMask _groundLayer;
        private LayerMask _castleLayer;
        private CastleUpper _castleUpper;
        private BuildHandler _buildHandler;
        private Camera _mainCamera;
        private InputDispatcher _inputDispatcher;

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

        public void UnSubscribe() =>
            _inputDispatcher.OnPrimaryClick -= TryHandleClick;

        private void TryHandleClick()
        {
            if (_buildHandler.IsActive == false && _castleUpper.IsActive == false)
            {
                    if (EventSystem.current.IsPointerOverGameObject())
                        return;

                    HandleClick();
            }
        }

        private void HandleClick()
        {
            Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, _castleLayer))
                _castleUpper.Activate();
            else if (Physics.Raycast(ray, out hit, Mathf.Infinity, _groundLayer))
                _buildHandler.Activate(hit.point);
        }
    }
}