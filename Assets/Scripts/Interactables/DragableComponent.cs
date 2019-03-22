using UnityEngine;
using System.Collections;

public class DragableComponent : MonoBehaviour, IInteractable
{
    [SerializeField] private float snapRange = 1f;
    private Vector3 endLocation;
    private Quaternion endRotation;
    private GameObject graphics;
    private Rigidbody rb;
    private Vector3 screenPoint;
    private float distanceFromCamera = 0;
    private Vector3 offset;
    private GameManager gm;

    private void Awake()
    {
        gm = FindObjectOfType<GameManager>();
        graphics = transform.GetChild(0).gameObject;
        endLocation = gm.chargePort.transform.position;
        endRotation = gm.chargePort.transform.rotation;
        rb = graphics.GetComponent<Rigidbody>();
        gm = FindObjectOfType<GameManager>();
    }

    private void Update()
    {
        distanceFromCamera += Input.GetAxis("Mouse ScrollWheel");
        distanceFromCamera = Mathf.Clamp(distanceFromCamera, -2f, 2f);
    }

    public void OnClick()
    {
        rb.useGravity = false;
        screenPoint = Camera.main.WorldToScreenPoint(graphics.transform.position);
        offset = graphics.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, screenPoint.z));
    }

    public void OnDrag()
    {
        Vector3 cursorPoint = new Vector3(Screen.width / 2, Screen.height / 2, screenPoint.z + distanceFromCamera);
        Vector3 cursorPosition = Camera.main.ScreenToWorldPoint(cursorPoint) + offset;
        graphics.transform.position = cursorPosition;
    }

    public void OnRelease()
    {
        bool canCharge = gm.InitGame(this);

        if (!canCharge)
        {
            rb.useGravity = true;
            return;
        }

        if (Vector3.Distance(graphics.transform.position, endLocation) < snapRange)
        {
            rb.isKinematic = true;
            graphics.transform.position = endLocation;
            graphics.transform.rotation = endRotation;
            graphics.layer = LayerMask.NameToLayer("Default");
        }
    }

    public void ReleaseBatery()
    {
        rb.useGravity = true;
        rb.isKinematic = false;
        Destroy(gameObject, 5f);
    }
}
