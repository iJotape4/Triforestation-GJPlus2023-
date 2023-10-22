using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SwipeBiome : MonoBehaviour
{
    Button swipeButton;

    private void Start()
    {
        swipeButton = GetComponent<Button>();
        swipeButton.onClick.AddListener(SelectToSwipe);
    }

    private void SelectToSwipe()
    {
        
    }
}
