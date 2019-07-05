using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

/// <summary>
/// Controla los Datos de autentificacion de usuario.
/// </summary>
public class Authenticator : MonoBehaviour
{
    #region variables
    public InputField nameField;
    public InputField passwordField;
    public TextMeshProUGUI messageDisplay;

    // Diccionario alamacena Usuario y contraseña de manera local, en variables estaticas que no dependan de un objeto en escena.
    public static string currentUser;
    public static Dictionary<string, string> authenticatorData = new Dictionary<string, string>();
    #endregion

    /// <summary>
    /// Logica del boton Play en el menu principal.
    /// </summary>
    public void OnSubmit()
    {
        // Check de usuario valido.
        if (nameField.text == "")
        {
            messageDisplay.text = "Please enter a valid name";
        }

        // Check de contraseña valida.
        else if(passwordField.text == "")
        {
            messageDisplay.text = "Please enter a valid Password";
        }

        // Agrega nuevo usuario al diccionario si este no existe previamente y permite paso a la escena de juego.
        else if (!authenticatorData.ContainsKey(nameField.text))
        {
            authenticatorData.Add(nameField.text, passwordField.text);
            SceneManager.LoadScene("Level1");
            currentUser = nameField.text;
        }

        // Check de password correcto en el caso de que el usuario exista. 
        // Si el password es incorrecto, no se podra jugar.
        else if(authenticatorData.ContainsKey(nameField.text))
        {
            if (authenticatorData[nameField.text] != passwordField.text)
            {
                messageDisplay.text = "Incorrect Password" ;
            }
            else
            {
                SceneManager.LoadScene("Level1");
                currentUser = nameField.text;
            }
        }
    }
}
