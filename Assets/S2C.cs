using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public Camera camera;
public Vector2 VRTrackingOrigin;

public class S2C : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        public angleBetweenVectors(Vector2 A, Vector2 B) {
            return Math.Acos(Vecotr2.Dot(Vector2.Normalize(A), Vector2.Normalize(B)));
        }
        prevForwardVector = camera.transform.forward;
        prevYawRelativeToCenter = angleBetweenVectors(camera.transform.forward, VRTrackingOrigin.transform.position - camera.transform.position);
        public d(Vector2 A, Vector2 B, Vector2 C) {
            return (A.x-B.x)*(C.y-B.y)-(A.y-B.y)*(C.x-B.x);
        }
    }

    // Update is called once per frame
    void Update()
    {
        howMuchUserRotated = angleBetweenVectors(prevForwardVector, camera.transform.forward);
        directionUserRotated = (d(camera.transform.position+prevForwardVector, camera.transform.position, camera.transform.position + camera.transform.forward) < 0)?1:-1;
        deltaYawRelativeToCenter = prevYawRelativeToCenter - angleBetweenVectors(camera.transform.forward, VRTrackingOrigin.transform.position - camera.transform.position);
        distanceFromCenter = (camera.transform.position - VRTrackingOrigin.transform.position).magnitude;
        longestDimensionOfPE = ???
    }
}
