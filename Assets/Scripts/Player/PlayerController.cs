using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Material highlightColor;
    private Renderer lastSelectedSliderMaterial;
    private Material lastSelectedMaterial;
    private GameManager gm;
    private Transform hit;
    private bool selectedInteractable;

    private void Awake()
    {
        gm = FindObjectOfType<GameManager>();
    }

    private void Update()
    {
        if (!selectedInteractable)
        { hit = GetObject(1 << LayerMask.NameToLayer("Interactables")); }

        HighlightObject(hit);

        selectedInteractable = InteractWithHit(hit);
    }

    private bool InteractWithHit(Transform hit)
    {
        if (hit == null) { return false; }

        IInteractable interactable = hit.parent.GetComponent<IInteractable>();

        if (interactable == null) { return false; }

        if (Input.GetMouseButtonDown(0))
        {
            interactable.OnClick();
            return true;
        }
        else if (Input.GetMouseButton(0))
        {
            interactable.OnDrag();
            return true;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            interactable.OnRelease();
            return false;
        }
        return false;
    }

    private void HighlightObject(Transform hit)
    {
        if (lastSelectedSliderMaterial)
            lastSelectedSliderMaterial.material = lastSelectedMaterial;

        if (hit == null) { return; }

        Renderer hitRenderer = hit.GetComponent<Renderer>();

        if (hitRenderer == null) { return; }

        lastSelectedMaterial = hitRenderer.material;
        hitRenderer.material = highlightColor;
        lastSelectedSliderMaterial = hitRenderer;
    }

    private Transform GetObject(int layerMask)
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, layerMask))
        {
            return hit.collider.transform;
        }

        return null;
    }
}