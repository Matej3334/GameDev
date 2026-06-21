using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NextLevel : MonoBehaviour
{
    [SerializeField] private Image loadingScreen;
    private float transparency;
    private Color imageColor = Color.black;

    private void Start()
    {
        transparency = 0f;
        imageColor.a = transparency;
        loadingScreen.color = imageColor;
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(LoadingScreen());
            StartCoroutine(LoadSceneAsync());
        }
    }
    IEnumerator LoadingScreen()
    {
        float elapsedTime = 0f;

        while (elapsedTime < 2f)
        {

            float progress = elapsedTime / 2f;

            transparency = Mathf.Lerp(0f, 1f, progress);
            
            imageColor.a = transparency;
            loadingScreen.color = imageColor;

            elapsedTime += Time.deltaTime;
            yield return null; 
        }


        transparency = 1f;
        imageColor.a = transparency;
        loadingScreen.color = imageColor;

    }

    IEnumerator LoadSceneAsync()
    {
        yield return new WaitForSeconds(5f);
        Scene currentScene = SceneManager.GetActiveScene();

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("SampleScene", LoadSceneMode.Additive);

        
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        Scene newScene = SceneManager.GetSceneByName("SampleScene");
        SceneManager.SetActiveScene(newScene);

        yield return SceneManager.UnloadSceneAsync(currentScene);
    }
}
