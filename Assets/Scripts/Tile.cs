using System.Collections;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public bool hasAlly;
    public GameObject allyObject; // Reference to the placed ally object
    public SpriteRenderer spriteRenderer; // Reference to the tile's sprite renderer
    private Coroutine _growShrinkCoroutine;
    private float _animationTime;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        _growShrinkCoroutine = StartCoroutine(GrowShrinkAnimation());
    }

    private void OnDisable()
    {
        if (_growShrinkCoroutine != null)
        {
            StopCoroutine(_growShrinkCoroutine);
        }
    }

    private IEnumerator GrowShrinkAnimation()
    {
        while (true)
        {
            _animationTime += Time.deltaTime * 1.0f; 
            float scale = Mathf.PingPong(_animationTime, 0.1f) + 0.25f; 
            transform.localScale = new Vector3(scale, scale, scale);
            yield return null; // Wait for the next frame
        }
    }
    public void ResetTile()
    {
        hasAlly = false;
    }
}