using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Material highlightMaterial;
    private Renderer lastSelectedSliderMaterial;
    private Material lastSelectedMaterial;
    private GameManager gm;
    private Transform hit;
    private IInteractable objectToInteract;
    private bool selectedInteractable;

    private void Awake()
    {
        gm = FindObjectOfType<GameManager>();
    }

    private void Update()
    {
        if (!selectedInteractable)
        { hit = GetObject(1 << LayerMask.NameToLayer("Interactables")); }

        if (hit != null)
        {
            objectToInteract = hit.parent.GetComponent<IInteractable>();

            highlightMaterial.color = objectToInteract.hightlightColor;
            HighlightObject(hit, highlightMaterial);

            selectedInteractable = InteractWithHit(objectToInteract);
        }
    }

    private bool InteractWithHit(IInteractable interactable)
    {
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

    private void HighlightObject(Transform hit, Material highlightMaterial)
    {
        if (lastSelectedSliderMaterial)
            lastSelectedSliderMaterial.material = lastSelectedMaterial;

        if (hit == null) { return; }

        Renderer hitRenderer = hit.GetComponent<Renderer>();

        if (hitRenderer == null) { return; }

        lastSelectedMaterial = hitRenderer.material;
        hitRenderer.material = highlightMaterial;
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