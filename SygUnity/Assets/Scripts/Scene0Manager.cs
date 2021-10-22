using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Scene0Manager : MonoBehaviour
{
    public InputField InfuraURL;

    public void StartGame() {
        GameManager.Instance.InfuraUrl = this.InfuraURL.text;
        SceneManager.LoadScene("Scene1");
    }
}
