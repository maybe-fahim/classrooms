using UnityEngine;
using System.Collections.Generic;

public class LibraryPuzzleManager : MonoBehaviour
{
    [Header("Book Prefabs")]
    [Tooltip("Array of 5 different Book prefabs you want to place randomly.")]
    public GameObject[] bookPrefabs; // Must have length = 5

    // A list to keep track of all BookAnchor transforms found among child shelves.
    private List<Transform> allBookAnchors = new List<Transform>();

    private void Start()
    {
        // Ensure we have at least 5 book prefabs assigned.
        if (bookPrefabs == null || bookPrefabs.Length < 5)
        {
            Debug.LogError("Please assign 5 Book Prefabs in the Inspector.");
            return;
        }

        // Step 1: Gather all BookAnchors under the shelves that are children of this object.
        PopulateBookAnchors();

        // Step 2: Place books on 5 randomly selected anchors.
        PlaceBooksOnRandomAnchors();
    }

    private void PopulateBookAnchors()
    {
        // Clear any existing anchors (if this script somehow runs again).
        allBookAnchors.Clear();

        // Loop over the immediate children of this object (BookshelvesBottom).
        foreach (Transform shelf in transform)
        {
            // Check if the child's name starts with the expected bookshelf prefix
            // (e.g., "Bookshelf1", "Bookshelf2", "Bookshelf3", "Bookshelf4", or "BookshelfEmpty").
            if (shelf.name.StartsWith("Bookshelf") || shelf.name.StartsWith("BookshelfEmpty"))
            {
                // Get all child transforms (including nested ones) of this shelf.
                Transform[] childTransforms = shelf.GetComponentsInChildren<Transform>(true);

                foreach (Transform child in childTransforms)
                {
                    // If the child's name contains "BookAnchor", store it.
                    if (child.name.Contains("BookAnchor"))
                    {
                        allBookAnchors.Add(child);
                    }
                }
            }
        }
    }

    private void PlaceBooksOnRandomAnchors()
    {
        // We need at least 5 anchors for the puzzle to work.
        if (allBookAnchors.Count < 5)
        {
            Debug.LogError("Not enough BookAnchors found to place 5 books!");
            return;
        }

        // Shuffle the list of all BookAnchors, then pick the first 5.
        ShuffleList(allBookAnchors);

        for (int i = 0; i < 5; i++)
        {
            Transform anchor = allBookAnchors[i];
            GameObject bookPrefab = bookPrefabs[i];

            // Create the book as a child of the selected anchor.
            Instantiate(bookPrefab, anchor.position, anchor.rotation, anchor);

            // Optionally snap it exactly to local position/rotation:
            // spawnedBook.transform.localPosition = Vector3.zero;
            // spawnedBook.transform.localRotation = Quaternion.identity;
        }
    }

    /// <summary>
    /// Fisher-Yates shuffle to randomize the elements in a list in-place.
    /// </summary>
    private void ShuffleList<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int randomIndex = Random.Range(i, list.Count);
            T temp = list[i];
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }
}
