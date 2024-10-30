namespace UIHandlerScripts
{
    using UnityEngine;
    using UnityEngine.UI;

    public class BgScroller : MonoBehaviour
    {

        [SerializeField] private RawImage rawImage;
        [SerializeField] private float scrollSpeedX, scrollSpeedY;

        private void Update()
        {
            // Move the texture offset by the scroll speed
            rawImage.uvRect = new Rect(rawImage.uvRect.position + new Vector2(scrollSpeedX, scrollSpeedY) * Time.deltaTime, rawImage.uvRect.size);
        }
    }  
}

