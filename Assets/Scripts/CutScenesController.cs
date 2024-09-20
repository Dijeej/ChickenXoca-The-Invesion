using System.Collections; // Adicionado para usar IEnumerator
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class CutscenesController : MonoBehaviour
{
    public Image cutsceneImage;   // A imagem PNG que será exibida
    public string nextSceneName;  // O nome da próxima cena
    public float displayDuration; // Duração em segundos que a imagem será exibida

void Start()
{
    
    StartCoroutine(DisplayCutscene());
}

IEnumerator DisplayCutscene()
{

    cutsceneImage.gameObject.SetActive(true);
    yield return new WaitForSeconds(displayDuration);
    cutsceneImage.gameObject.SetActive(false);

    if(CompareTag("MainCamera")){
        SceneManager.LoadScene(nextSceneName);
    }
}

private void OnCollisionEnter(Collision collision)
{
    if(collision.gameObject.CompareTag("Player")){
        SceneManager.LoadScene(nextSceneName);

    }
}

}