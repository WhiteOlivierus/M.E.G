using UnityEngine;

public class EarthController : MonoBehaviour
{
    [SerializeField] private Material[] earthsMaterial;

    public void SetAllMaterials(float[] earthValues)
    {
        for (int i = 0; i < earthsMaterial.Length; i++)
        {
            earthsMaterial[i].SetFloat("_Amount", earthValues[i]);
        }
    }
}
