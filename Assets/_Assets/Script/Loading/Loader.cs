using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Loader
{
    public enum Scene{
        GameMenu,
        LoadingScence,
        GamePlay,

    }
    private static Scene targetSence;
    public static void Load(Scene targetSence){
        Loader.targetSence=targetSence;

        SceneManager.LoadScene(Scene.LoadingScence.ToString());
    }

    public static void LoaderCallback(){
        SceneManager.LoadScene(targetSence.ToString());
    }
}
