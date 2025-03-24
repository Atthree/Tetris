using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private int score = 0;
    private int lines;
    public int level = 1;

    [SerializeField] private int numberOfLevel = 5;

    private int minLine = 1;
    private int maxLine = 4;

    [SerializeField] private TextMeshProUGUI lineTxt;
    [SerializeField] private TextMeshProUGUI levelTxt;
    [SerializeField] private TextMeshProUGUI scoreTxt;

    public bool isLevelPassed = false;

    private void Start()
    {
        ResetFnc();
    }
    public void ResetFnc()
    {
        level = 1;
        lines = numberOfLevel * level;
        UpdateText();
    }
    public void LineScore(int n)
    {
        isLevelPassed = false;
        n = Mathf.Clamp(n, minLine, maxLine);

        switch(n)
        {
            case 1:
                score += 30 * level;
                break;
            case 2:
                score += 50 * level;
                break;
            case 3:
                score += 150 * level;
                break;
            case 4:
                score += 500 * level;
                break;
        }
        lines -= n;

        if(lines <0)
        {
            UpdateLevel();
        }
        UpdateText();
    }

    private void UpdateText()
    {
        if(scoreTxt)
        {
            scoreTxt.text = AddZero(score, 5);
        }
        if (lineTxt)
        {
            lineTxt.text = lines.ToString();
        }
        if (levelTxt)
        {
            levelTxt.text = level.ToString();
        }
    }

    string AddZero(int scor , int numbers)
    {
        string scoreStr = scor.ToString();

        while(scoreStr.Length < numbers)
        {
            scoreStr = "0" + scoreStr;
        }
        return scoreStr;
    }

    public void UpdateLevel()
    {
        level++;
        lines = numberOfLevel + level;
        isLevelPassed = true;
    }
}
