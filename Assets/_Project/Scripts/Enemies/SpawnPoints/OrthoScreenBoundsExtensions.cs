using UnityEngine;

namespace _Project.Scripts.Enemies.SpawnPoints
{
    public static class OrthoScreenBoundsExtensions
    {
        private static (float halfW, float halfH) GetHalfExtents(this Camera cam)
        {
            float halfH = cam.orthographicSize;
            float halfW = halfH * cam.aspect;

            return (halfW, halfH);
        }

        public static Bounds GetVisibleBounds(this Camera cam, float groundY)
        {
            var (halfW, halfH) = cam.GetHalfExtents();

            Vector3 cameraPosition = cam.transform.position;

            Vector3 center = new Vector3(
                cameraPosition.x,
                groundY,
                cameraPosition.z
            );

            return new Bounds(center, new Vector3(halfH * 2, 0, halfW * 2)
            );
        }
    }
}