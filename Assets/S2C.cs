using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S2C : MonoBehaviour
{    
    int longestDimensionOfPE;
    float howMuchUserRotated, prevYawRelativeToCenter, directionUserRotated, deltaYawRelativeToCenter, distanceFromCenter, howMuchToAccelerate;
    //double howMuchToAccelerate;
    Vector3 prevForwardVector;

    public Camera camera;
    public GameObject VRTrackingOrigin;

    // Start is called before the first frame update
    void Start()
    {
        prevForwardVector = camera.transform.forward;
        prevYawRelativeToCenter = angleBetweenVectors(camera.transform.forward, VRTrackingOrigin.transform.position - camera.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        longestDimensionOfPE = 1;
        howMuchUserRotated = angleBetweenVectors(prevForwardVector, camera.transform.forward);
        directionUserRotated = (direction(camera.transform.position+prevForwardVector, camera.transform.position, camera.transform.position + camera.transform.forward) < 0)?-1:1;
        deltaYawRelativeToCenter = prevYawRelativeToCenter - angleBetweenVectors(camera.transform.forward, VRTrackingOrigin.transform.position - camera.transform.position);
        distanceFromCenter = (camera.transform.position - VRTrackingOrigin.transform.position).magnitude;
        prevForwardVector=camera.transform.forward;
        prevYawRelativeToCenter=angleBetweenVectors(camera.transform.forward,VRTrackingOrigin.transform.position-camera.transform.position);
        howMuchToAccelerate=((deltaYawRelativeToCenter<0)? - 0.13f : 0.30f) * howMuchUserRotated * directionUserRotated * Mathf.Clamp(distanceFromCenter/longestDimensionOfPE/2,0,1);
        if(Mathf.Abs(howMuchToAccelerate) > 0) {
            VRTrackingOrigin.transform.RotateAround(camera.transform.position, new Vector3(0.0f, 1.0f, 0.0f), (float)howMuchToAccelerate);
        }
    } 

    float angleBetweenVectors(Vector3 A, Vector3 B) {
        return Mathf.Acos(Vector3.Dot(A.normalized, B.normalized))*180/Mathf.PI;
    }

    float direction(Vector3 A, Vector3 B, Vector3 C) {
        return (A.x-B.x)*(C.z-B.z)-(A.z-B.z)*(C.x-B.x);
    }
}
