using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ChoiceManager : MonoBehaviour
{
    public Button jaButton;
    public Button noButton;
    public Button fortsettButton;
    public GameObject jaImage;
    public GameObject neiImage;
    public GameObject ganingPoints;
    public GameObject losingPoitns;


    void Start()
    {
        // Hide "fortsett" button
        fortsettButton.gameObject.SetActive(false);
        jaImage.SetActive(false);
        neiImage.SetActive(false);
        ganingPoints.SetActive(false);
        losingPoitns.SetActive(false);
        
        jaButton.onClick.AddListener(() => OnJaButtonClick());
        noButton.onClick.AddListener(() => OnNeiButtonClick());

        fortsettButton.onClick.AddListener(LoadForklaringScene);
    }

    void OnJaButtonClick()
    {

        ganingPoints.SetActive(false);
        losingPoitns.SetActive(true);
        jaImage.SetActive(true);
        neiImage.SetActive(false);
        fortsettButton.gameObject.SetActive(true);
        noButton.interactable = false;
    }

    void OnNeiButtonClick()
    {
        ganingPoints.SetActive(true);
        losingPoitns.SetActive(false);
        neiImage.SetActive(true);
        jaImage.SetActive(false);
        fortsettButton.gameObject.SetActive(true);
        jaButton.interactable = false;
    }

    void LoadForklaringScene()
    {
        // Load the "Forklaring" scene
        SceneManager.LoadScene("Forklaring");
    }
}