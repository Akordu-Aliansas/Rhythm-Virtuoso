using System.Collections;
using UnityEngine;

public class DelayedSetActive : MonoBehaviour
{
    [Header("Delay before activating/deactivating GameObjects")]
    public float delay = 0.15f;

    [Header("Page References - Drag your page GameObjects here")]
    public GameObject page1;
    public GameObject page2;

    // Methods for OnClick events
    public void DelayedActivateObject(GameObject targetObject)
    {
        StartCoroutine(SetActiveAfterDelay(targetObject, true));
    }

    public void DelayedDeactivateObject(GameObject targetObject)
    {
        StartCoroutine(SetActiveAfterDelay(targetObject, false));
    }

    // Method to handle both activate and deactivate in one call
    public void DelayedSetActiveObjects(GameObject objectToActivate, GameObject objectToDeactivate)
    {
        StartCoroutine(SetMultipleActiveAfterDelay(objectToActivate, objectToDeactivate));
    }

    // Special version with extra delay for problematic buttons
    public void GoToPreviousPageDelayed()
    {
        StartCoroutine(DelayedPreviousPage());
    }

    private IEnumerator DelayedPreviousPage()
    {
        yield return new WaitForSeconds(0.2f); // Extra delay for sound to play
        GoToPreviousPage();
    }

    // Specific methods for your page switching (easier for Unity inspector)
    public void GoToPreviousPage()
    {
        Debug.Log($"GoToPreviousPage - Page1 active: {page1.activeInHierarchy}, Page2 active: {page2.activeInHierarchy}");

        // If on page 2, go to page 1
        if (page2.activeInHierarchy && !page1.activeInHierarchy)
        {
            Debug.Log("Going to Previous Page (Page 1)");
            StartCoroutine(SetMultipleActiveAfterDelay(page1, page2));
        }
        else if (page1.activeInHierarchy && !page2.activeInHierarchy)
        {
            Debug.Log("Already on first page, staying on Page 1");
            // Do nothing - stay on page 1
        }
        else if (!page1.activeInHierarchy && !page2.activeInHierarchy)
        {
            Debug.Log("No pages active, enabling Page 1");
            StartCoroutine(SetActiveAfterDelay(page1, true));
        }
    }

    public void GoToNextPage()
    {
        Debug.Log($"GoToNextPage - Page1 active: {page1.activeInHierarchy}, Page2 active: {page2.activeInHierarchy}");

        // If on page 1, go to page 2
        if (page1.activeInHierarchy && !page2.activeInHierarchy)
        {
            Debug.Log("Going to Next Page (Page 2)");
            StartCoroutine(SetMultipleActiveAfterDelay(page2, page1));
        }
        else if (page2.activeInHierarchy && !page1.activeInHierarchy)
        {
            Debug.Log("Already on last page, staying on Page 2");
            // Do nothing - stay on page 2
        }
        else if (!page1.activeInHierarchy && !page2.activeInHierarchy)
        {
            Debug.Log("No pages active, enabling Page 2");
            StartCoroutine(SetActiveAfterDelay(page2, true));
        }
    }

    private IEnumerator SetActiveAfterDelay(GameObject targetObject, bool activeState)
    {
        yield return new WaitForSeconds(delay);
        if (targetObject != null)
        {
            targetObject.SetActive(activeState);
        }
    }

    private IEnumerator SetMultipleActiveAfterDelay(GameObject objectToActivate, GameObject objectToDeactivate)
    {
        yield return new WaitForSeconds(delay);

        if (objectToDeactivate != null)
        {
            objectToDeactivate.SetActive(false);
        }

        if (objectToActivate != null)
        {
            objectToActivate.SetActive(true);
        }
    }
}