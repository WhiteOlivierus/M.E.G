using UnityEngine;

public class SpawnBatteries : MonoBehaviour
{
    public GameObject baterry;
    public int amountOfBaterries;

    private void Start()
    {
        for (int i = 0; i < amountOfBaterries; i++)
        {
            SpawnBaterry();
        }
    }

    public void SpawnBaterry()
    {
        GameObject go = Instantiate(baterry, transform.position, Quaternion.identity);
        go.transform.SetParent(transform);
    }
}
