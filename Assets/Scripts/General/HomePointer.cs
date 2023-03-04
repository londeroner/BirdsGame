using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomePointer : MonoBehaviour
{
    [SerializeField] Transform _playerTransform;
    [SerializeField] Camera _camera;
    [SerializeField] Transform _pointerIconTransform;

    void Update()
    {
        Vector3 fromPlayerToEnemy = transform.position - _playerTransform.position;
        Ray ray = new Ray(_playerTransform.position, fromPlayerToEnemy);

        Debug.DrawRay(_playerTransform.position, fromPlayerToEnemy);

        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(_camera);

        float minDistance = Mathf.Infinity;

        for (int i = 0; i < 4; i++)
        {
            if (planes[i].Raycast(ray, out float distance))
            {
                if (distance < minDistance)
                    minDistance = distance;
            }
        }

        minDistance = Mathf.Clamp(minDistance, 0f, fromPlayerToEnemy.magnitude);

        Vector3 worldPosition = ray.GetPoint(minDistance);

        _pointerIconTransform.position = _camera.WorldToScreenPoint(worldPosition);
    }
}
