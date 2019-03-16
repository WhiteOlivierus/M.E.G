using System;
using UnityEngine;
using UnityEngine.UI;

public class SliderComponent : MonoBehaviour, IInteractable
{
    public float value;

    [SerializeField] private int maxRange = 1;
    private Transform graphics;
    private Vector3 minSliderPosition;
    private Vector3 maxSliderPosition;
    private Vector3 lastMousePosition;
    private TextMesh monitor;
    private TextMesh precisionMonitor;
    private float stepping = .01f;

    private void Awake()
    {
        graphics = transform.GetChild(0).transform;
        minSliderPosition = transform.GetChild(1).transform.localPosition;
        maxSliderPosition = transform.GetChild(2).transform.localPosition;
        precisionMonitor = transform.GetChild(5).GetComponent<TextMesh>();
        monitor = transform.GetChild(3).GetComponent<TextMesh>();
    }

    private void Start()
    {
        stepping = (Vector3.Distance(minSliderPosition, maxSliderPosition) / maxRange) / 2f;
        CalculateValueOfSlider();
        monitor.text = value.ToString();
    }

    public void OnClick()
    {
        SetMousePosition();
    }

    public void OnDrag()
    {
        MoveSlider();
        SetMousePosition();
        UpdateSlider();
        CalculateValueOfSlider();
        LogValue();
    }

    public void OnRelease() { }

    private void MoveSlider()
    {
        Vector3 delta = Input.mousePosition - lastMousePosition;
        Vector3 pos = graphics.localPosition;
        pos.x += delta.x * stepping;
        graphics.localPosition = pos;
    }

    private void SetMousePosition()
    {
        lastMousePosition = Input.mousePosition;
    }

    private void SetInitialPosition()
    {
        throw new NotImplementedException();
    }

    private void UpdateSlider()
    {
        graphics.localPosition = Utilities.ClampVector(graphics.localPosition, maxSliderPosition, minSliderPosition);
    }

    private void LogValue()
    {
        //Display the value
        if (monitor)
        {
            monitor.text = value.ToString();
        }
        else
        {
            Debug.Log(value);
        }
    }

    private void CalculateValueOfSlider()
    {
        //get the distance from begin to current position to map to value
        float dist = Vector3.Distance(minSliderPosition, maxSliderPosition);
        float distToMin = Vector3.Distance(minSliderPosition, graphics.localPosition);
        float currentToMinDist = dist - distToMin;

        //make the value workable
        value = (int)Utilities.Map(currentToMinDist, 0, dist, 0, maxRange);
    }

    public void SetPrecisionMonitor(int i)
    {
        switch (i)
        {
            case -1:
                precisionMonitor.text = ">";
                break;
            case 0:
                precisionMonitor.text = "=";
                break;
            case 1:
                precisionMonitor.text = "<";
                break;
            default:
                precisionMonitor.text = "";
                break;
        }
    }
}
