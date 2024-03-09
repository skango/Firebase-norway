using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HoverButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Button button;
    public Image hoverImage; // Reference to the hover image component

    private bool buttonClicked = false;

    void Start()
    {
        button = GetComponent<Button>();

        if (hoverImage != null)
        {
            HideImage(); // Initially hide the hover image
        }
        else
        {
            Debug.LogError("Hover image component not assigned in the Inspector.");
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!buttonClicked && hoverImage != null)
        {
            ShowImage(); // Show the hover image only if the button is not clicked
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!buttonClicked && hoverImage != null)
        {
            HideImage(); // Hide the hover image only if the button is not clicked
        }
    }

    public void OnButtonClick()
    {
        buttonClicked = true; // Set button clicked flag
        if (hoverImage != null)
        {
            ShowImage(); // Show the hover image when the button is clicked
        }
    }

    private void ShowImage()
    {
        hoverImage.enabled = true; // Show the hover image
    }

    private void HideImage()
    {
        hoverImage.enabled = false; // Hide the hover image
    }
}
