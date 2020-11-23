using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveGround : MonoBehaviour
{
    public Material currentMaterial;
    public float scrollSpeed;
    float offset;

    void Start()
    {
        currentMaterial = GetComponent<Renderer>().material;
    }

    void FixedUpdate()
    {
        // float offset = Time.time * scrollSpeed; //this code works too in same way (code of Unity Docs)
        offset += Time.deltaTime * scrollSpeed;
        currentMaterial.SetTextureOffset("_MainTex", new Vector2(offset, 0));
    }
}
