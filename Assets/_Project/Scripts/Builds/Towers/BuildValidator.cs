using System.Collections.Generic;
using _Project.Scripts.Builds.Castles;
using UnityEngine;

namespace _Project.Scripts.Builds.Towers
{
    public class BuildValidator
    {
        private readonly Castle _castle;
        private readonly float _minDistanceForBuilding;

        private HashSet<Tower> _towers = new();

        public BuildValidator(Castle castle, float minDistanceForBuilding)
        {
            _minDistanceForBuilding = minDistanceForBuilding;
            _castle = castle;
        }

        public void AddTower(Tower tower) =>
            _towers.Add(tower);

        public bool TryValidateBuildPoint(Vector3 buildPosition)
        {
            float distance = Vector3.Distance(buildPosition, _castle.transform.position);

            if (distance < _minDistanceForBuilding)
                return false;

            foreach (Tower tower in _towers)
            {
                distance = Vector3.Distance(tower.transform.position, buildPosition);

                if (distance < _minDistanceForBuilding)
                    return false;
            }

            return true;
        }
    }
}