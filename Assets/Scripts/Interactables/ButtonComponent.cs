using UnityEngine;

public class ButtonComponent : MonoBehaviour, IInteractable
{
    private GameManager gm;

    private void Awake()
    {
        gm = FindObjectOfType<GameManager>();
    }

    public void OnClick()
    {
        gm.CheckGameState();
    }

    public void OnDrag() { }

    public void OnRelease() { }
}
