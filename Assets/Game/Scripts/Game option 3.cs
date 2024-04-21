using UnityEngine;
using UnityEngine.UI;

public class FillText : MonoBehaviour
{
    public Text textToFill;
    public Button option1Button;
    public Button option2Button;
    public Button option3Button;
    public GameObject[] greenCheckImages;
    public GameObject[] redCheckImages;
    public Text scoreText;

    [SerializeField] private string option1Text = "Answer1";
    [SerializeField] private string option2Text = "Answer2";
    [SerializeField] private string option3Text = "Answer3";

    [SerializeField] private bool option1IsCorrectForFirstSlot = false;
    [SerializeField] private bool option2IsCorrectForFirstSlot = false;
    [SerializeField] private bool option3IsCorrectForFirstSlot = false;

    [SerializeField] private bool option1IsCorrectForSecondSlot = false;
    [SerializeField] private bool option2IsCorrectForSecondSlot = false;
    [SerializeField] private bool option3IsCorrectForSecondSlot = false;

    private bool firstSlotFilled = false;
    private bool secondSlotFilled = false;

    private int score = 0;

    async void Start()
    {
        option1Button.onClick.AddListener(() => OnOptionButtonClick(option1Text, option1IsCorrectForFirstSlot, option1IsCorrectForSecondSlot, 0));
        option2Button.onClick.AddListener(() => OnOptionButtonClick(option2Text, option2IsCorrectForFirstSlot, option2IsCorrectForSecondSlot, 1));
        option3Button.onClick.AddListener(() => OnOptionButtonClick(option3Text, option3IsCorrectForFirstSlot, option3IsCorrectForSecondSlot, 2));
        
        HideCheckImages();
        score = await AccountSystem.instance.ReadUserScore();
    }

    void OnOptionButtonClick(string optionText, bool isCorrectForFirstSlot, bool isCorrectForSecondSlot, int optionIndex)
    {
        if (!firstSlotFilled)
        { 
            textToFill.text = textToFill.text.ReplaceFirst("\" \"", $"\"{optionText}\"");
            firstSlotFilled = true;

            
            if (isCorrectForFirstSlot)
            {
                greenCheckImages[optionIndex].SetActive(true);
                score += 500;
            }
            else
            {
                redCheckImages[optionIndex].SetActive(true);
                score -= 500;
            }
        }
        else if (!secondSlotFilled)
        {
            
            textToFill.text = textToFill.text.ReplaceFirst("\" \"", $"\"{optionText}\"");
            secondSlotFilled = true;

            
            if (isCorrectForSecondSlot)
            {
                greenCheckImages[optionIndex].SetActive(true);
                score += 500;
            }
            else
            {
                redCheckImages[optionIndex].SetActive(true);
                score -= 500;
            }

            
            option1Button.interactable = false;
            option2Button.interactable = false;
            option3Button.interactable = false;

            
            UpdateScoreText();
        }
        AccountSystem.instance.SetUserScore(score);
        AccountSystem.instance.SetUserQuestion(3);
    }

    void HideCheckImages()
    {
        foreach (var greenCheckImage in greenCheckImages)
        {
            greenCheckImage.SetActive(false);
        }

        foreach (var redCheckImage in redCheckImages)
        {
            redCheckImage.SetActive(false);
        }
    }

    void UpdateScoreText()
    {
        scoreText.text = "Score: " + score.ToString();
    }
}

public static class StringExtensions
{
    public static string ReplaceFirst(this string text, string search, string replace)
    {
        int pos = text.IndexOf(search);
        if (pos < 0)
        {
            return text;
        }
        return text.Substring(0, pos) + replace + text.Substring(pos + search.Length);
    }
}
