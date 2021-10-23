using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Scene0Manager : MonoBehaviour
{
    public InputField InfuraURL;
    public InputField GalaxyTokenAddress;
    public InputField SpaceMafiaAddress;

    public void StartGame() {
        GameManager.Instance.InfuraUrl = this.InfuraURL.text;
        GameManager.Instance.GalaxyTokenContractAddress = this.GalaxyTokenAddress.text;
        GameManager.Instance.SpaceMafiaContractAddress = this.SpaceMafiaAddress.text;

        SceneManager.LoadScene("Scene1");
    }
}
