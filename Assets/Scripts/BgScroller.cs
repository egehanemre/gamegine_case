using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BgScroller : MonoBehaviour
{

    [SerializeField] private RawImage rawImage;
    [SerializeField] private float scrollSpeedX, scrollSpeedY;

    private void Update()
    {
        rawImage.uvRect = new Rect(rawImage.uvRect.position + new Vector2(scrollSpeedX, scrollSpeedY) * Time.deltaTime, rawImage.uvRect.size);
    }
}
