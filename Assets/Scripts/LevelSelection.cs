
using TMPro;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelSelection : MonoBehaviour
{
    public Button[] lvlbuttons;

    private int levelNumber;
    void Start()
    {
        int levelAt = PlayerPrefs.GetInt("levelAt",1);

        for(int i = 0; i < lvlbuttons.Length; i++)
        {
            if(i+1 > levelAt)
            {
                lvlbuttons[i].interactable = false;
            }
        }

    }

    public void Levelload(Button button)
    {
        TMP_Text tmpText = button.GetComponentInChildren<TMP_Text>(true);
        levelNumber = int.Parse(new string(tmpText.text.Where(char.IsDigit).ToArray()));
        SceneManager.LoadScene("Level"+levelNumber);
    }
 
}
