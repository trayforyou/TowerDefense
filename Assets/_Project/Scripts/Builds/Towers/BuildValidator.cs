using System.Collections.Generic;
using _Project.Scripts.Builds.Castles;
using UnityEngine;

namespace _Project.Scripts.Builds.Towers
{
    public class BuildValidator
    {
        private readonly Castle _castle;
        private readonly float _minDistanceForBuilding;

        public BuildValidator(Castle castle, float minDistanceForBuilding)
        {
            _minDistanceForBuilding = minDistanceForBuilding;
            _castle = castle;
        }

        public bool TryValidateBuildPoint(Vector3 buildPosition, List<Tower> towers)
        {
            float distance = Vector3.Distance(buildPosition, _castle.transform.position);

            if (distance < _minDistanceForBuilding)
                return false;

            foreach (Tower tower in towers)
            {
                distance = Vector3.Distance(tower.transform.position, buildPosition);

                if (distance < _minDistanceForBuilding)
                    return false;
            }

            return true;
        }
    }
}