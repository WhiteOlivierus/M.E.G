using UnityEngine;

public class ButtonComponent : MonoBehaviour, IInteractable
{
    public Color hightlightColor { get; set; } = Color.red;

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
