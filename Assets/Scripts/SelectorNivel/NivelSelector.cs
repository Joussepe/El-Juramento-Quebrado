using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class NivelSelector : MonoBehaviour
{
    public GameObject nivelButtonPrefab;
    public Transform buttonContainer;
    public int totalLevels = 3;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GenerateLevelButtons();
    }

    void GenerateLevelButtons()
    {
        for(int i = 1; i <= totalLevels; i++)
        {
            GameObject buttonObj = Instantiate(nivelButtonPrefab, buttonContainer);
            buttonObj.GetComponentInChildren<TextMeshProUGUI>().text = "Nivel " + i;

            int levelIndex = i;
            buttonObj.GetComponent<Button>().onClick.AddListener(() =>
            {
                Time.timeScale = 1f;
                SceneManager.LoadScene("Nivel_" + levelIndex);
            });
        }
    }
}
