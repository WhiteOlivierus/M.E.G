using UnityEngine;
using System.Collections;

public class DragableComponent : MonoBehaviour, IInteractable
{
    [SerializeField] private float snapRange = 1f;
    private Vector3 endLocation;
    private GameObject graphics;
    private Rigidbody rb;
    private Vector3 screenPoint;
    private float distanceFromCamera = 3f;
    private Vector3 offset;
    private GameManager gm;

    private void Awake()
    {
        gm = FindObjectOfType<GameManager>();
        graphics = transform.GetChild(0).gameObject;
        endLocation = gm.chargePort.transform.position;
        rb = graphics.GetComponent<Rigidbody>();
        gm = FindObjectOfType<GameManager>();
    }

    private void Update()
    {
        distanceFromCamera += Input.GetAxis("Mouse ScrollWheel");
        distanceFromCamera = Mathf.Clamp(distanceFromCamera, 2f, 10f);
    }

    public void OnClick()
    {
        rb.useGravity = false;
        screenPoint = Camera.main.WorldToScreenPoint(graphics.transform.position);
        offset = graphics.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, screenPoint.z));
        Debug.Log(screenPoint.z);
    }

    public void OnDrag()
    {
        Vector3 cursorPoint = new Vector3(Screen.width / 2, Screen.height / 2, distanceFromCamera);
        Vector3 cursorPosition = Camera.main.ScreenToWorldPoint(cursorPoint) + offset;
        graphics.transform.position = cursorPosition;
    }

    public void OnRelease()
    {
        if (Vector3.Distance(graphics.transform.position, endLocation) > snapRange)
        {
            rb.useGravity = true;
            return;
        }
        else
        {
            rb.isKinematic = true;
            graphics.transform.position = endLocation;
            graphics.transform.rotation = Quaternion.identity;
            graphics.layer = LayerMask.NameToLayer("Default");
            gm.InitGame(this);
        }
    }

    IEnumerator ReleaseBatery()
    {
        rb.useGravity = true;
        rb.isKinematic = false;
        yield return new WaitForSeconds(5f);
        Destroy(gameObject);
    }
}
