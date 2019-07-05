using TMPro;
using UnityEngine;

/// <summary>
/// Despliega en un canvas las variables de vida, puntaje y tiempo restante de juego.
/// </summary>
public class UIDisplay : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI lifeText;
    public TextMeshProUGUI TimeText;

    void Update()
    {
        scoreText.text = "Score: " + GameManagement.score.ToString();
        TimeText.text = GameManagement.count.ToString("N0");
        lifeText.text = "Life: " + GameManagement.life.ToString();
    }
}
