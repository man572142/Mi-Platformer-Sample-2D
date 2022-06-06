using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RefreshUI : MonoBehaviour
{
    [SerializeField] Slider velocitySlider = null;

    [SerializeField] Movement movement = null;

    private void OnEnable()
    {
        movement.OnRefreshUI += VelocityChange;
    }

    private void VelocityChange(float value)
    {
        velocitySlider.value = value;
    }

    private void OnDisable()
    {
        movement.OnRefreshUI -= VelocityChange;
    }
}
