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

    // Update is called once per frame
    public void SpawnBaterry()
    {
        Instantiate(baterry, transform.position, Quaternion.identity);
    }
}
