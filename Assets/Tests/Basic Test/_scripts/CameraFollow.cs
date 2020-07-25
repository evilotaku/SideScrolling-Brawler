using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour
{
    GameObject[] targets;
    float currentDistance;
    float largestDistance;
    Camera theCamera;
    float height = 5f;
    Vector3 avgDistance;
    public float speed = 1f;
    public Vector3 offset;

    void Start()
    {
        theCamera = GetComponent<Camera>();
    }

    void LateUpdate()
    {
        targets = GameObject.FindGameObjectsWithTag("Player");
        if(targets.Length == 0) return;

        Vector3 sum = Vector3.zero;

        for (var n = 0; n < targets.Length; n++)
        {
            sum += targets[n].transform.position;
        }
        avgDistance = sum / targets.Length;

        var largestDifference  = returnLargestDifference();

        height = Mathf.Lerp(height, largestDifference , Time.deltaTime * speed);

         theCamera.transform.position = new Vector3(avgDistance.x, height + offset.y, avgDistance.z - offset.z + largestDifference);
         theCamera.transform.LookAt(avgDistance + offset);

    }

    float returnLargestDifference()
    {
        currentDistance = 0f;
        largestDistance = 0f;

        for (var i = 0; i < targets.Length; i++)
        {
            for (var j = 0; j < targets.Length; j++)
            {
                currentDistance = Vector3.Distance(targets[i].transform.position, targets[j].transform.position);
                if (currentDistance > largestDistance)
                {
                    largestDistance = currentDistance;
                }
            }
        }
        return largestDistance;
    }
}
