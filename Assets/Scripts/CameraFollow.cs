using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject player;
    public Vector3 distance;
    public GameObject lookAtTarget;
    public Transform lookAtCenter;
    private Vector3[] targets;
    private void Start()
    {
        targets = new Vector3[2];
    }
    private void LateUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, player.transform.position + distance, Time.deltaTime * 5f);
        targets[0] = player.transform.position;
        targets[1] = lookAtTarget.transform.position;
        lookAtCenter.position = CenterOfVectors(targets);
        transform.LookAt(lookAtCenter);
    }

    public Vector3 CenterOfVectors(Vector3[] vectors)
    {
        Vector3 sum = Vector3.zero;
        if (vectors == null || vectors.Length == 0)
        {
            return sum;
        }

        foreach (Vector3 vec in vectors)
        {
            sum += vec;
        }
        return sum / vectors.Length;
    }
}