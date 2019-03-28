using UnityEngine;

public class Bobbing : MonoBehaviour
{
    private Vector3 pos1;
    private Vector3 pos2;

    private Vector3 offset;

    public float moveSpeed = 1f;

    private Vector3 moveTo;

    private void Start()
    {
        offset = new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), Random.Range(-1, 1));
        pos1 = transform.position;
        pos2 = transform.position + offset;
    }

    private void Update()
    {
        if (transform.position == pos1)
        {
            moveTo = pos2;
        }
        if (transform.position == pos2)
        {
            moveTo = pos1;
            pos2 = pos1 + new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), Random.Range(-1, 1));
        }

        transform.position = Vector3.MoveTowards(transform.position, moveTo, moveSpeed / 100);
    }
}
