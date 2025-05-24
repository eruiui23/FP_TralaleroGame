using Unity.Mathematics;
using UnityEngine;

public class ParralaxEffect : MonoBehaviour
{
    public Camera cam;
    public Transform followTarget;

    Vector2 startingPosition; //starting postion of the parralex object
    float startingZ; // startign Z positoin of the parralex object
    Vector2 camMoveSinceStart => (Vector2)cam.transform.position - startingPosition; // => update 
    float zDistanceFromTarget => transform.position.z - followTarget.position.z;
    float clippingPlane => (cam.transform.position.z + (zDistanceFromTarget > 0 ? cam.farClipPlane : cam.nearClipPlane));
    float parallaxFactor => math.abs(zDistanceFromTarget) / clippingPlane;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startingPosition = transform.position;
        startingZ = transform.position.z;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 newPosition = startingPosition + camMoveSinceStart * parallaxFactor;

        transform.position = new Vector3(newPosition.x, newPosition.y, startingZ);
    }
}
