using UnityEngine;

public interface IInteractable
{
    Color hightlightColor { get; set; }

    void OnClick();
    void OnDrag();
    void OnRelease();
}
