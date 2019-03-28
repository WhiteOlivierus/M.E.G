using UnityEngine;
using FMODUnity;

public class ButtonComponent : MonoBehaviour, IInteractable
{
    [EventRef]
    public string Event = "";
    FMOD.Studio.EventInstance button;

    public Color hightlightColor { get; set; } = Color.red;

    private GameManager gm;

    private void Awake()
    {
        button = RuntimeManager.CreateInstance(Event);
        gm = FindObjectOfType<GameManager>();
    }

    public void OnClick()
    {
        RuntimeManager.PlayOneShot(Event);
        button.start();
        gm.CheckGameState();
    }

    public void OnDrag() { }

    public void OnRelease() { }
}
