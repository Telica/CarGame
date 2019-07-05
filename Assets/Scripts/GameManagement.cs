using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManagement : MonoBehaviour
{
    #region variables
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI usernameText;
    public GameObject canvas;
    public GameObject car;

    Transform cartransform;

    [Header ("Game Settings")]
    public float gameTime;
    public float setLife;

    public static float count;
    public static float life;
    public static int score;
    #endregion

    /// <summary>
    /// Adquiere las variables y referencias iniciales.
    /// </summary>
    void Awake()
    {
        count = gameTime;
        life = setLife;
        cartransform = car.GetComponent<Transform>();
    }
    
    /// <summary>
    /// Describe las situaciones en donde el juega termina, luego despliega Canvas con 
    /// la estadistica del juego.
    /// </summary>
    void Update() { 
        if (count <= 0 || life == 0 || car.transform.position.y <= -15)
        {
            canvas.SetActive(true);
            ShowStats();
        }
        else
        {
            count -= Time.deltaTime;
        }
    }

    /// <summary>
    /// Despliega datos actualizados en el Canvas, sobre usuario activo y puntaje 
    /// </summary>
    private void ShowStats()
    {
    usernameText.text = "User: " + Authenticator.currentUser;
    scoreText.text = "Score: " + GameManagement.score.ToString();
    }


    /// <summary>
    /// Retorna al menu principal, moviendo a la nueva escena que pide usuario y password.
    /// </summary>
    public void BackToMainMenu()
    {
        canvas.SetActive(false);
        GameManagement.score = 0;
        life = setLife;
        count = gameTime;
        SceneManager.LoadScene("Game Intro");
    }

    /// <summary>
    /// Resetea el juego, reiniciando las variables de puntaje, vida y tiempo.
    /// </summary>
    public void RestartGame()
    {
        canvas.SetActive(false);
        GameManagement.score = 0;
        life = setLife;
        count = gameTime;
        SceneManager.LoadScene("Level1");
    }
}
