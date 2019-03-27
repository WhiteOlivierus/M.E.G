﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FMODUnity;
using FMOD;

public class SliderComponent : MonoBehaviour, IInteractable
{
    [HideInInspector] public float value;
    public List<SpriteRenderer> sliderName;
    [HideInInspector] public TextMesh precisionMonitor;
    [SerializeField] private int maxRange = 1;
    private Transform graphics;
    private Vector3 minSliderPosition;
    private Vector3 maxSliderPosition;
    private Vector3 lastMousePosition;
    private TextMesh monitor;
    private float stepping = .05f;
    public Color hightlightColor { get; set; } = Color.red;

    [EventRef]
    public string Event = "";
    FMOD.Studio.EventInstance slide;

    private void Awake()
    {
        slide = RuntimeManager.CreateInstance(Event);
        RuntimeManager.PlayOneShot(Event);
        slide.start();

        graphics = transform.GetChild(0).transform;
        minSliderPosition = transform.GetChild(1).transform.localPosition;
        maxSliderPosition = transform.GetChild(2).transform.localPosition;
        monitor = transform.GetChild(3).GetComponent<TextMesh>();
    }

    private void Start()
    {
        stepping = (Vector3.Distance(minSliderPosition, maxSliderPosition) / maxRange);
        CalculateValueOfSlider();
        monitor.text = value.ToString();
    }

    public void OnClick()
    {
        SetMousePosition();
        slide.setParameterByName("Sliding", 1f);

    }

    public void OnDrag()
    {
        MoveSlider();
        SetMousePosition();
        UpdateSlider();
        CalculateValueOfSlider();
        LogValue();
    }

    public void OnRelease() { return; }

    private void MoveSlider()
    {
        Vector3 delta = Input.mousePosition - lastMousePosition;
        Vector3 pos = graphics.localPosition;
        pos.x += delta.y * stepping;
        graphics.localPosition = pos;
    }

    private void SetMousePosition()
    {
        lastMousePosition = Input.mousePosition;
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
            UnityEngine.Debug.Log(value);
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

    public void SetPrecisionMonitor(int j)
    {
        switch (j)
        {
            case -1:
                precisionMonitor.text = "+";
                monitor.color = Color.red;
                break;
            case 0:
                precisionMonitor.text = "OK";
                monitor.color = Color.green;
                break;
            case 1:
                precisionMonitor.text = "-";
                monitor.color = Color.red;
                break;
            default:
                precisionMonitor.text = "";
                monitor.color = Color.black;
                break;
        }
    }
}
