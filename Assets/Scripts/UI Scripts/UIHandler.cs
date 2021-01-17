using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour
{
    public static UIHandler Instance;

    private void Awake()
    {
        if (Instance != null) Destroy(this);
        else Instance = this;
    }

    public GameObject HelperObject;
    public Color defaultHeaderColor;

    [Space]
    [Header("Time")]
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI dateText;

    private void Update()
    {
        timeText.text = DateTime.Now.ToString("hh:mm");
        dateText.text = DateTime.Now.ToString("dd.MM.yyyy");
    }
}
