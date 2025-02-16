using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Symbols : MonoBehaviour
{
    //set symbol name as well if and when needed
    public Sprite symbolImage;
    public Sprite symbolWinImage;

    
    public string symbolName;
    
    private Vector3 pos;
    private Vector3 oldPos;
    
    public SpriteRenderer symbolRenderer;

    private void Start()
    {
        /*pos = transform.position;
        oldPos = transform.position;*/
        
        symbolRenderer.sprite = symbolImage;
        symbolName = symbolRenderer.sprite.name;
    }
}
