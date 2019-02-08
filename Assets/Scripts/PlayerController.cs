using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private SliderComponent selectedSlider;
    private Color lastColor;
    private Color lastSelectedColor;
    private GameManager gm;
    private bool nextKeyStroke = true;
    private GameObject lastSelectedSlider;

    private void Start()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    private void Update()
    {
        // HighLightObject();
        SelectSlider();
        if (selectedSlider && nextKeyStroke)
        {
            if (Input.GetKey(KeyCode.UpArrow))
            {
                StartCoroutine("KeyDelay", -1);
                nextKeyStroke = false;
            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                StartCoroutine("KeyDelay", 1);
                nextKeyStroke = false;
            }
        }
    }

    private void HighLightObject()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (lastSelectedSlider != hit.collider.gameObject)
                lastSelectedSlider.GetComponent<Renderer>().material.color = lastSelectedColor;

            lastSelectedSlider = hit.collider.gameObject;
            lastSelectedColor = lastSelectedSlider.GetComponent<Renderer>().material.color;
            lastSelectedSlider.GetComponent<Renderer>().material.color = Color.green;
        }
        else
        {
            lastSelectedSlider.GetComponent<Renderer>().material.color = lastSelectedColor;
        }
    }

    void SelectSlider()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                print("hit");
                if (hit.collider.tag == "Slider")
                {
                    if (selectedSlider)
                        selectedSlider.GetComponent<Renderer>().material.color = lastColor;

                    lastColor = hit.collider.GetComponent<Renderer>().material.color;
                    hit.collider.GetComponent<Renderer>().material.color = Color.red;
                    selectedSlider = hit.collider.GetComponent<SliderComponent>();
                }
                else if (hit.collider.tag == "Button")
                {
                    gm.CheckScenario();
                }
            }
        }
    }

    IEnumerator KeyDelay(int i)
    {
        selectedSlider.ChangePosition(i);
        gm.ConstrainPoints(selectedSlider);
        yield return new WaitForSeconds(.1f);
        nextKeyStroke = true;
    }
}
