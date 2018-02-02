using UnityEngine;
using System.Collections;


/// Water surface script (update the shader properties).
public class Water2D : MonoBehaviour {

    public Vector2 speed = new Vector2(0.01f, 0f);

    private Renderer rend;
    private Material mat;
    private float offsetStart, offsetEnd;

    void Awake()
    {
        rend = GetComponent<Renderer>();
        mat = rend.material;
        offsetStart = -.2f;
        offsetEnd = .5f;
    }

    void LateUpdate()
    {
        Vector2 scroll = Time.deltaTime * speed;

        mat.mainTextureOffset += scroll;

        if(mat.mainTextureOffset.x > offsetEnd)
        {
            mat.mainTextureOffset = new Vector2 (offsetStart, 0);
        }

    }
}
