﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FMODUnity;

public class SliderComponent : MonoBehaviour, IInteractable
{
    public float value;
    public List<SpriteRenderer> sliderName;
    public TextMesh precisionMonitor;
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
    FMOD.Studio.EventDescription slideEventDescription;
    FMOD.Studio.PARAMETER_DESCRIPTION sliderParameterDescription;
    FMOD.Studio.PARAMETER_ID slidingOnParameterId, sliderOnParameterId;
    //ValueSlide
    FMOD.Studio.PARAMETER_DESCRIPTION slideValueParameterDescription;
    FMOD.Studio.PARAMETER_ID slidingValueParameterId, sliderValueParameterId;


    private void Awake()
    {
        slide = FMODUnity.RuntimeManager.CreateInstance(Event);
        slideEventDescription = FMODUnity.RuntimeManager.GetEventDescription(Event);
        slide.getDescription(out slideEventDescription);
        slideEventDescription.getParameterDescriptionByName("Sliding", out sliderParameterDescription);
        slideEventDescription.getParameterDescriptionByName("Value", out slideValueParameterDescription);
        slide.start();
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(slide, GetComponent<Transform>(), GetComponent<Rigidbody>());
        slidingOnParameterId = sliderParameterDescription.id;
        slidingValueParameterId = slideValueParameterDescription.id;

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
        //RuntimeManager.PlayOneShot(PlayerStateEvent);

       //slide.setParameterByID(Sliding, 0f);
    }

    public void OnDrag()
    {

        MoveSlider();
        SetMousePosition();
        UpdateSlider();
        CalculateValueOfSlider();
        LogValue();
        slide.setParameterByName("Sliding", 0);
        slide.setParameterByName("Value", value / 100);
        UnityEngine.Debug.Log(value / 100);
    }

    public void OnRelease() {

        slide.setParameterByName("Sliding", 1);
        return; 

        }


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
