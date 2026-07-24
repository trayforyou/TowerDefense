using System;
using System.Collections.Generic;
using _Project.Scripts.Builds.Castles;
using _Project.Scripts.Builds.Towers;
using _Project.Scripts.ScriptableObjects;
using _Project.Scripts.Session;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _Project.Scripts.Builds
{
    [RequireComponent(typeof(CastleUpper))]
    [RequireComponent(typeof(TowerBuilder))]
    public class InteractHandler : MonoBehaviour
    {
        [SerializeField] private LayerMask _groundLayer;
        [SerializeField] private LayerMask _castleLayer;

        private readonly HashSet<Vector3> _buildPositions = new();
        private CastleUpper _castleUpper;
        private TowerBuilder _towerBuilder;
        private Castle _castle;
        private Wallet _wallet;
        private GameConfig _config;
        private Camera _mainCamera;
        private Vector3 _buildPosition;

        private void Awake()
        {
            _castleUpper = GetComponent<CastleUpper>();
            _towerBuilder = GetComponent<TowerBuilder>();

            _towerBuilder.BuildingTower += AddNewPosition;
        }

        private void Start() =>
            _mainCamera = Camera.main;

        private void Update()
        {
            if (_towerBuilder.IsActive == false && _castleUpper.IsActive == false)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    if (EventSystem.current.IsPointerOverGameObject()) return;

                    HandleClick();
                }
            }
        }

        private void OnDestroy() => 
            _towerBuilder.BuildingTower -= AddNewPosition;

        public void SetParameters(GameConfig config, Wallet wallet, Castle castle)
        {
            if (wallet == null || config == null || castle == null)
                throw new ArgumentNullException();

            _wallet = wallet;
            _config = config;
            _castle = castle;

            _castleUpper.Initialize(_config, _wallet, _castle);
            _towerBuilder.Initialize(_wallet, _config);
        }

        public void Stop()
        {
            _towerBuilder.StopAttack();
            _castleUpper.TurnOff();
            _towerBuilder.TurnOff();
        }

        private void AddNewPosition(Vector3 position) =>
            _buildPositions.Add(position);

        private void HandleClick()
        {
            Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, _castleLayer))
            {
                _castleUpper.Activate();
            }
            else if (Physics.Raycast(ray, out hit, Mathf.Infinity, _groundLayer))
            {
                Vector3 clickedPoint = hit.point;

                float distance = Vector3.Distance(clickedPoint, _castle.transform.position);

                if (distance < _config.MinDistanceForBuilding)
                    return;

                foreach (Vector3 buildPosition in _buildPositions)
                {
                    distance = Vector3.Distance(clickedPoint, buildPosition);

                    if (distance < _config.MinDistanceForBuilding)
                        return;
                }

                _buildPosition = clickedPoint;
                _towerBuilder.Activate(_buildPosition);
            }
        }
    }
}