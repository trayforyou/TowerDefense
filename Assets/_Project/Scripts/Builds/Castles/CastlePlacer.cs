using System;
using UnityEngine;
using static UnityEngine.Object;

namespace _Project.Scripts.Builds.Castles
{
    public static class CastlePlacer
    {
        public static Castle PlaceAtScreenCenter(Castle castle, Camera camera, LayerMask groundLayer)
        {
            if (castle == null || camera == null)
                throw new NullReferenceException();

            Vector3 screenCenter = new Vector3(Screen.width / 2f, Screen.height / 2f, 0f);
            Ray ray = camera.ScreenPointToRay(screenCenter);

            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, groundLayer))
                return Instantiate(castle, hit.point, Quaternion.identity);

            throw new Exception("Земля не найдена");
        }
    }
}