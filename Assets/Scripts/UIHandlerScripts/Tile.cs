namespace UIHandlerScripts
{
    using System.Collections;
    using UnityEngine;

    public class Tile : MonoBehaviour
    {
        [Header("Tile Settings")]
        public bool hasAlly;
        public GameObject allyObject; // Reference to the placed ally object
        public SpriteRenderer spriteRenderer; // Private reference to the tile's sprite renderer

        [Header("Animation Settings")]
        public float animationSpeed = 1.0f;
        public float baseScale = 0.25f;
        public float scaleRange = 0.1f;

        private Coroutine _growShrinkCoroutine;
        private float _animationTime;
        private GameObject _tilesParent;

        private void Awake()
        {
            _tilesParent = GameObject.Find("PlacementTiles");
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void OnEnable()
        {
            StartGrowShrinkAnimation();
        }

        private void OnDisable()
        {
            StopGrowShrinkAnimation();
        }

        private void StartGrowShrinkAnimation()
        {
            _growShrinkCoroutine = StartCoroutine(GrowShrinkAnimation());
        }

        private void StopGrowShrinkAnimation()
        {
            if (_growShrinkCoroutine != null)
            {
                StopCoroutine(_growShrinkCoroutine);
                _growShrinkCoroutine = null;
            }
        }

        private IEnumerator GrowShrinkAnimation()
        {
            // Loop the animation while the tiles parent is active
            while (_tilesParent.activeSelf)
            {
                _animationTime += Time.deltaTime * animationSpeed;
                float scale = Mathf.PingPong(_animationTime, scaleRange) + baseScale;
                transform.localScale = new Vector3(scale, scale, scale);
                yield return null; // Wait for the next frame
            }
        }

        public void ResetTile()
        {
            hasAlly = false;
            allyObject = null;
            transform.localScale = new Vector3(baseScale, baseScale, baseScale);
        }
    }
}
