using UnityEngine;

public class RandomPosterVariant : MonoBehaviour
{
    [Tooltip("Add your possible poster sprites/textures here.")]
    public Sprite[] posterImages;

    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        // Get the SpriteRenderer component attached to this poster
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (posterImages.Length > 0)
        {
            // Pick a random index
            int randomIndex = Random.Range(0, posterImages.Length);
            // Assign the chosen sprite to the poster
            spriteRenderer.sprite = posterImages[randomIndex];
        }
        else
        {
            Debug.LogWarning("No poster images assigned to the RandomPosterVariant script on " + gameObject.name);
        }
    }
}
