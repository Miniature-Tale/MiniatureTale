using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    
     public Transform playerTransform;
    public float smoothSpeed = 0.125f;

    private void LateUpdate()
    {
        if (playerTransform != null)
        {
            Vector3 targetPosition = new Vector3(playerTransform.position.x, transform.position.y, transform.position.z);
            transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);
        }
    }
}
