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

        private CastleUpper _castleUpper;
        private TowerBuilder _towerBuilder;
        private Camera _mainCamera;
        private ShooterConfig _shooterConfig;
        private CastleConfig _castleConfig;

        private void Awake()
        {
            _castleUpper = GetComponent<CastleUpper>();
            _towerBuilder = GetComponent<TowerBuilder>();
        }

        private void Start() =>
            _mainCamera = Camera.main;

        private void Update()
        {
            if (_towerBuilder.IsActive == false && _castleUpper.IsActive == false)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    if (EventSystem.current.IsPointerOverGameObject())
                        return;

                    HandleClick();
                }
            }
        }

        public void SetParameters(CastleConfig castleConfig, ShooterConfig shooterConfig, BuildConfig buildConfig,
            TowerConfig fastTowerConfig, TowerConfig strongTowerConfig, Wallet wallet, Castle castle)
        {
            _shooterConfig = shooterConfig;
            _castleConfig = castleConfig;

            _castleUpper.Initialize(_castleConfig, wallet, castle);
            _towerBuilder.Initialize(_shooterConfig, wallet, buildConfig, fastTowerConfig, strongTowerConfig, castle);
        }

        public void Stop()
        {
            _towerBuilder.StopAttack();
            _castleUpper.TurnOff();
            _towerBuilder.TurnOff();
        }

        private void HandleClick()
        {
            Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, _castleLayer))
                _castleUpper.Activate();
            else if (Physics.Raycast(ray, out hit, Mathf.Infinity, _groundLayer))
                _towerBuilder.Activate(hit.point);
        }
    }
}