using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingTexture : MonoBehaviour {

    public float ScrollX = 0;
    public float ScrollY = -1;
    Renderer targetRenderer; 
    
    void Awake()
    {
        targetRenderer = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        float OffsetX = Time.time * ScrollX;
        float OffsetY = Time.time * ScrollY;
        targetRenderer.material.mainTextureOffset = new Vector2(OffsetX, OffsetY);
    }
}
