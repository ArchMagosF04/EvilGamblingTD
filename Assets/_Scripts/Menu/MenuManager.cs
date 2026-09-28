using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private Material fadeMaterial;
    [SerializeField] private float fadeInDuration = .5f;


    private void Start() 
    {
        fadeMaterial.SetFloat("_Fade", 1);
        fadeMaterial.DOFloat(0, "_Fade", fadeInDuration);
    }

    public void SceneFadeChange(string sceneName)
    {
        StartCoroutine(ChangeScene(sceneName));
    }
    

    private IEnumerator ChangeScene(string sceneName)
    {
        fadeMaterial.DOFloat(1, "_Fade", 1);
        yield return new WaitForSeconds(1.5f);
        //fadeMaterial.SetFloat("_Fade", 0);
        SceneManager.LoadScene(sceneName);
    }


}
