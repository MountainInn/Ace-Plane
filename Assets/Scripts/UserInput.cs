using System;
using UnityEngine;

public class UserInput : MonoBehaviour
{
    public event Action<Vector3> onTouch;

    private const int maxDistance = 15;
    Vector3 pastFramePosition;
    Vector3 deltaPosition;

    Vector3
        startPoint,
        dragPoint
        ;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            startPoint = Input.mousePosition;
        }
        else if (Input.GetMouseButton(0))
        {
            dragPoint = Input.mousePosition;

            Vector3 path = (dragPoint - startPoint);

            Vector3 direction = path.normalized;
            float distance = path.magnitude;

            if (distance > maxDistance)
            {
                startPoint = Vector3.Lerp(startPoint, dragPoint, 1f - distance / maxDistance);
            }

            onTouch?.Invoke(direction);
        }
    }
}
