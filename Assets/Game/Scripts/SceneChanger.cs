using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public void ChangeToForsideScene()
    {
        Debug.Log("Changing to Moduler scene");
        SceneManager.LoadScene("Forside");
    }

    public void LoadScene(int sceneId)
    {
        SceneManager.LoadScene(sceneId);
    }

    public void ChangeToProfilScene()
    {
        Debug.Log("Changing to Profil scene");
        SceneManager.LoadScene("Profil");
    }

    public void ChangeToDuellScene()
    {
        Debug.Log("Changing to Duell scene");
        SceneManager.LoadScene("Duell");
    }

    public void ChangeToLedertavleScene()
    {
        Debug.Log("Changing to Ledertavle scene");
        SceneManager.LoadScene("Ledertavle");
    }
    public void SwitchToGameOption1()
    {
        SceneManager.LoadScene("Game Option 1");
    }
    public void SwitchToFørsteModul()
    {
        SceneManager.LoadScene(7);
    }

    public void SwitchToGameOption2()
    {
        SceneManager.LoadScene("Game Option 2");
    }
        public void SwitchToGameOption3()
    {
        SceneManager.LoadScene("Game Option 3");
    }
}
