using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossHairManager : MonoBehaviour
{
    [Header("UI Settings")]
    [SerializeField] private bool enabledConst = false;
    [SerializeField] private bool enabledDraw = false;
    
    [Header("UI")]
    [SerializeField] private UnityEngine.UI.Image imageConst;
    [SerializeField] private UnityEngine.UI.Image imageDraw;
    
    [Header("Input Reader")] 
    [SerializeField] private InputReader inputReader;
    
    private void OnEnable()
    {
        inputReader.DoubleTap += EnableOrDisableConstCrossHairDraw;
    }
    
    private void OnDisable()
    {
        inputReader.DoubleTap -= EnableOrDisableConstCrossHairDraw;
    }
    private void EnableOrDisableConstCrossHair(bool performed)
    {
        if (!performed) return;
        enabledConst = !enabledConst;
        imageConst.enabled = enabledConst;
        if (!enabledConst)
        {
            imageDraw.enabled = false;
            enabledDraw = false;
        }
    }
    
    private void EnableOrDisableConstCrossHairDraw()
    {
        if (!enabledConst) return;
        enabledDraw = !enabledDraw;
        imageDraw.enabled = enabledDraw;
    }

    private void Start()
    {
        imageConst.enabled = enabledConst;
        imageDraw.enabled = enabledDraw;
    }
}
