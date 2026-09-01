using UnityEngine;

namespace _Project.Scripts.Enemies.SpawnPoints
{
    public class PointsGenerator
    {
        private const int UpSide = 0;
        private const int DownSide = 1;
        private const int LeftSide = 2;
        private const int RightSide = 3;

        private Bounds _bounds;
        private readonly float _y;

        public PointsGenerator(float offset, Camera camera, float groundY)
        {
            _y = groundY;
            FindBounds(camera, offset, groundY);
        }

        private void FindBounds(Camera camera, float offset, float groundY)
        {
            _bounds = camera.GetVisibleBounds(groundY);
            _bounds.size += new Vector3(offset * 2, 0, offset * 2);
        }

        public Vector3 GetCenter() => 
            _bounds.center;

        public Vector3 GetRandom()
        {
            int side = Random.Range(0, 4);

            float x;
            float z;

            switch (side)
            {
                case UpSide:
                    x = _bounds.max.x;
                    z = Random.Range(_bounds.min.z, _bounds.max.z);
                    break;

                case DownSide:
                    x = _bounds.min.x;
                    z = Random.Range(_bounds.min.z, _bounds.max.z);
                    break;

                case LeftSide:
                    x = Random.Range(_bounds.min.x, _bounds.max.x);
                    z = _bounds.max.z;
                    break;

                case RightSide:
                    x = Random.Range(_bounds.min.x, _bounds.max.x);
                    z = _bounds.min.z;
                    break;

                default:
                    x = 0f;
                    z = 0f;
                    break;
            }

            return new Vector3(x, _y, z);
        }
    }
}