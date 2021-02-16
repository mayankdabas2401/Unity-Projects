using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossHair : MonoBehaviour
{
    public float rotationSpeed = -40f;
    
    public SpriteRenderer dot;
    public LayerMask targetMask;

    private Color originalColor;
    private Color highlightColor;

    // Start is called before the first frame update
    void Start()
    {
        // Make the cursor disappear
        Cursor.visible = false;

        originalColor = dot.color;
        highlightColor = Color.red;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);        
    }

    public void TargetDetector(Ray ray)
    {
        if (Physics.Raycast(ray, 100f, targetMask)) dot.color = highlightColor;
        else dot.color = originalColor;
    }
}
