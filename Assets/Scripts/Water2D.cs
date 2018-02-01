﻿using UnityEngine;
using System.Collections;


/// Water surface script (update the shader properties).
public class Water2D : MonoBehaviour {

    public Vector2 speed = new Vector2(0.01f, 0f);

    private Renderer rend;
    private Material mat;

    void Awake()
    {
        rend = GetComponent<Renderer>();
        mat = rend.material;
    }

    void LateUpdate()
    {
        Vector2 scroll = Time.deltaTime * speed;

        mat.mainTextureOffset += scroll;

    }
}
