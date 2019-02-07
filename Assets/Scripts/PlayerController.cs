using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private SliderComponent selectedSlider;
    private Color lastColor;
    private GameManager gm;

    private void Start()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    private void Update()
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

        if (selectedSlider)
        {
            if (Input.GetKey(KeyCode.UpArrow))
            {
                selectedSlider.ChangePosition(-1);
                gm.ConstrainPoints(selectedSlider);
            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                selectedSlider.ChangePosition(1);
                gm.ConstrainPoints(selectedSlider);
            }
        }
    }
}
