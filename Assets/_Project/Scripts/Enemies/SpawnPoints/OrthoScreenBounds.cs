using UnityEngine;

namespace _Project.Scripts.Enemies.SpawnPoints
{
    public static class OrthoScreenBounds
    {
        private static (float halfW, float halfH) GetHalfExtents(Camera cam)
        {
            float halfH = cam.orthographicSize;
            float halfW = halfH * cam.aspect;

            return (halfW, halfH);
        }

        public static Bounds GetVisibleBounds(Camera cam, float groundY)
        {
            var (halfW, halfH) = GetHalfExtents(cam);

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