using System.Collections;
using UnityEngine;

public class GameLoopManager : MonoBehaviour
{
    public static GameLoopManager Instance;

    public bool ShouldLoopEnd { get; private set; }



    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }
    }

    private IEnumerator GameLoop()
    {
        while(ShouldLoopEnd == false)
        {
            //Spawn Enemies

            //Spawn Towers

            //Move Enemies

            //Tick Towers

            //Apply Effects

            //Damage Enemies

            //Remove Enemies

            //Remove Towers


            yield return null;
        }
    }
}
