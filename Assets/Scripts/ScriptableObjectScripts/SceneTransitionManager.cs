using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/**
 * Class to handle scene Transistions. 
 */
[CreateAssetMenu(menuName = "Manager/SceneTransitionManager")]
public class SceneTransitionManager : SingletonScriptableObject<SceneTransitionManager>
{

    // Loads a new scene.
    public void Transition(int scene)
    {
        SceneManager.LoadScene(scene);
    }

    // Exit the game by quitting the application.
    public void ExitGame()
    {
        Application.Quit();
    }
    
}
