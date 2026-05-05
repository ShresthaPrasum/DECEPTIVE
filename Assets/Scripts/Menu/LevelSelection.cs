
using System;
using TMPro;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelSelection : MonoBehaviour
{
    public Button[] lvlbuttons;
    [SerializeField] private int levelOffset = 0;

    private int levelNumber;
    void Start()
    {
        int levelAt = PlayerPrefs.GetInt("levelAt",2);

        for(int i = 0; i < lvlbuttons.Length; i++)
        {
            int globalLevel = levelOffset + i + 1;
            if(globalLevel > levelAt)
            {
                lvlbuttons[i].interactable = false;
            }
        }

    }

    public void Levelload(Button button)
    {
        int index = Array.IndexOf(lvlbuttons, button);
        if (index >= 0)
        {
            levelNumber = levelOffset + index + 1;
        }
        else
        {
            TMP_Text tmpText = button.GetComponentInChildren<TMP_Text>(true);
            levelNumber = int.Parse(new string(tmpText.text.Where(char.IsDigit).ToArray()));
        }
        SceneManager.LoadScene("Level"+levelNumber);
    }

    public void levels()
    {
        SceneManager.LoadScene("levels");
    }
    public void levels1()
    {
        SceneManager.LoadScene("levels1");
    }
    public void levels2()
    {
        SceneManager.LoadScene("levels2");
    }
 
}
