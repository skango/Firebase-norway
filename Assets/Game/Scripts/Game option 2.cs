using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class MultipleChoiceManager : MonoBehaviour
{
    public Button[] optionButtons;
    public GameObject[] greenCheckImages;
    public GameObject[] redCheckImages;
    public Text scoreText;
    public Button NesteOppgave;

    public bool[] isCorrectOption;

    private bool[] optionSelected;
    private int score = 0;
    private int selectedCount = 0;
    private int correctCount = 0;
    private bool gameEnded = false;

    async void Start()
    {
        optionSelected = new bool[optionButtons.Length];
        NesteOppgave.gameObject.SetActive(false);
        HideCheckImages();
        UpdateScoreUI();

        for (int i = 0; i < optionButtons.Length; i++)
        {
            int index = i;
            optionButtons[i].onClick.AddListener(() => OnOptionButtonClick(index));
        }
        score = await AccountSystem.instance.ReadUserScore();
       
    }

    void OnOptionButtonClick(int index)
    {
        if (!optionSelected[index])
        {
            selectedCount++;
            optionSelected[index] = true;

            if (isCorrectOption[index])
            {
                score += 50;
                correctCount++;
                greenCheckImages[index].SetActive(true);
            }
            else
            {
                score -= 50;
                redCheckImages[index].SetActive(true);
            }
           
            if (selectedCount == 2)
            {
                gameEnded = true;
                DisableButtonFunctionality();
                UpdateScoreUI();
                NesteOppgave.gameObject.SetActive(true);
            }
            else
            {
                UpdateScoreUI();
            }
            AccountSystem.instance.SetUserScore(score);
            AccountSystem.instance.SetUserQuestion(2);
        }
    }

    void DisableButtonFunctionality()
    {
        foreach (var button in optionButtons)
        {
            button.onClick.RemoveAllListeners();
        }
    }

    void Update()
    {
        if (gameEnded)
        {
            DisableButtonFunctionality();
        }
    }

    void UpdateScoreUI()
    {
        scoreText.text = "Score: " + score.ToString();
        Debug.Log("Updating score UI with score: " + score);
        Debug.Log("Score text after update: " + scoreText.text);
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
}
