using UnityEngine;
using System.Collections;

public class FishMovement : MonoBehaviour
{
    public float speed = 2;
    public float scaleX = 2;
    public float scaleY = 3;
    public float offsetX = 0;
    public float offsetY = 0;

    public bool isLinkOffsetScalePositiveX = false;
    public bool isLinkOffsetScaleNegativeX = false;
    public bool isLinkOffsetScalePositiveY = false;
    public bool isLinkOffsetScaleNegativeY = false;
    public bool isFigure8 = true;

    private float phase;
    private float m_2PI = Mathf.PI * 2;
    private Vector3 originalPosition;
    private Vector3 pivot;
    private Vector3 pivotOffset;
    private bool isInverted = false;
    private bool isMoving = false;


    void Start()
    {
        pivot = transform.position;

        originalPosition = transform.position;

        isMoving = true;

        if (isLinkOffsetScalePositiveX)
            phase = 3.14f / 2f + 3.14f;
        else if (isLinkOffsetScaleNegativeX)
            phase = 3.14f / 2f;
        else if (isLinkOffsetScalePositiveY)
            phase = 3.14f;
        else
            phase = 0;
    }

    void Update()
    {
        pivotOffset = Vector3.up * 2 * scaleY;

        phase += speed * Time.deltaTime;

        if (isFigure8)
        {
            if (phase > m_2PI)
            {
                //Debug.Log("phase " + phase + " over 2pi: " + isInverted);
                isInverted = !isInverted;
                phase -= m_2PI;
            }
            if (phase < 0)
            {
                //Debug.Log("phase " + phase + " under 0");
                phase += m_2PI;
            }
        }

        Vector3 nextPosition = new Vector3(0f, 0f, 0f);
        if (isInverted)
        {
            nextPosition = pivot + pivotOffset;
        }
        else
        {
             nextPosition = pivot + Vector3.zero;
        }
        transform.position = new Vector3(nextPosition.x + Mathf.Sin(phase) * scaleX + offsetX, nextPosition.y + Mathf.Cos(phase) * (isInverted ? -1 : 1) * scaleY + offsetY, nextPosition.z);
    }
}