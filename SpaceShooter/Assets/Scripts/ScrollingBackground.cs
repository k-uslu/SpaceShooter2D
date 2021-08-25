using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingBackground : MonoBehaviour
{
    [SerializeField] float scrollSpeed = 0.5f;
    Material material;
    Vector2 offSet;

    // Start is called before the first frame update
    void Start()
    {
        material = GetComponent<Renderer>().material;
        offSet = new Vector2(0, scrollSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        material.mainTextureOffset += offSet * Time.deltaTime;
    }
}
