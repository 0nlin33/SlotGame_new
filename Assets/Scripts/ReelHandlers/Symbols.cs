using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Symbols : MonoBehaviour
{
    //set symbol name as well if and when needed
    public Sprite symbolImage;
    public Sprite symbolWinImage;

    public int symbolValue;
    
    public SpriteRenderer symbolRenderer;

    private void Start()
    {
        symbolRenderer.sprite = symbolImage;
    }
}
