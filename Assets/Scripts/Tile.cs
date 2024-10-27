using System.Collections;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public bool hasAlly;
    public GameObject allyObject; // Reference to the placed ally object
    public SpriteRenderer spriteRenderer; // Reference to the tile's sprite renderer
    private Coroutine growShrinkCoroutine;
    private float animationTime;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        // Start the grow and shrink animation coroutine
        growShrinkCoroutine = StartCoroutine(GrowShrinkAnimation());
    }

    private void OnDisable()
    {
        // Stop the grow and shrink animation coroutine
        if (growShrinkCoroutine != null)
        {
            StopCoroutine(growShrinkCoroutine);
        }
    }

    private IEnumerator GrowShrinkAnimation()
    {
        while (true)
        {
            animationTime += Time.deltaTime * 1.0f; // Increase speed by a factor of 10
            float scale = Mathf.PingPong(animationTime, 0.1f) + 0.25f; // Scale between 0.25 and 0.35
            transform.localScale = new Vector3(scale, scale, scale);
            yield return null; // Wait for the next frame
        }
    }
}