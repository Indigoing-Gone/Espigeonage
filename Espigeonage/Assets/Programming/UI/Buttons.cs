using UnityEngine;
using UnityEngine.SceneManagement;

public class Buttons : MonoBehaviour
{
   public void LoadGame()
    {
        SceneManager.LoadScene("MainRoom");
    }
}
