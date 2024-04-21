using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class ChoiceManager : MonoBehaviour
{
    public Button riktigButton;
    public Button feilButton;
    public Button fortsettButton;
    public GameObject greenCheckImageForRiktig;
    public GameObject redCheckImageForRiktig;
    public GameObject greenCheckImageForFeil;
    public GameObject redCheckImageForFeil;
    public Text scoreText;
    public Text correctText; 
    public Text wrongText;   
    public Text answerMessageCorrectText; 
    public Text answerMessageWrongText;

    private int score = 0;
    public string correctAnswer = ""; 

    private bool answerSelected = false;

    async void Start()
    {
        fortsettButton.gameObject.SetActive(false);
        HideCheckImages();

        riktigButton.onClick.AddListener(() => OnRiktigButtonClick());
        feilButton.onClick.AddListener(() => OnFeilButtonClick());

        fortsettButton.onClick.AddListener(LoadForklaringScene);
        //score = await AccountSystem.instance.ReadUserScore();
    }

    void OnRiktigButtonClick()
    {
        if (!answerSelected) 
        {
            score += 100;
            UpdateScoreUI();
            AccountSystem.instance.SetUserScore(score);
            AccountSystem.instance.SetUserQuestion(1);
            greenCheckImageForRiktig.SetActive(true);
            redCheckImageForRiktig.SetActive(false);
            answerSelected = true;

            correctText.gameObject.SetActive(true);
            correctText.text = "Forklaring på svar";
            wrongText.gameObject.SetActive(false);
            fortsettButton.gameObject.SetActive(true);
            answerMessageCorrectText.text = "Riktig!";
            StartCoroutine(LoadGame2());
        }
    }

    void OnFeilButtonClick()
    {
        if (!answerSelected) 
        {
            score -= 100;
            UpdateScoreUI();
            AccountSystem.instance.SetUserScore(score);
            redCheckImageForRiktig.SetActive(true);
            greenCheckImageForRiktig.SetActive(false);
            answerSelected = true;

            wrongText.gameObject.SetActive(true);
            wrongText.text = "Riktig svar er:\n" + correctAnswer;
            correctText.gameObject.SetActive(false);

            fortsettButton.gameObject.SetActive(true);
            answerMessageWrongText.text = "Feil!";
            StartCoroutine(LoadGame2());
        }
    }

    IEnumerator LoadGame2()
    {
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene(2);
    }

    void UpdateScoreUI()
    {
        scoreText.text = "Score: " + score.ToString();
        Debug.Log("Updating score UI with score: " + score);
        Debug.Log("Score text after update: " + scoreText.text);
    }

    void LoadForklaringScene()
    {
        SceneManager.LoadScene("Forklaring");
    }

    public void LoadScene(int index)
    {
        SceneManager.LoadScene(index);
    }

    void HideCheckImages()
    {
        greenCheckImageForRiktig.SetActive(false);
        redCheckImageForRiktig.SetActive(false);
        greenCheckImageForFeil.SetActive(false);
        redCheckImageForFeil.SetActive(false);
    }

    public void SetCorrectAnswer(string answer)
    {
        correctAnswer = answer;
    }
}
