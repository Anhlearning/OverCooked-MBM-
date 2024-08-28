using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Loader
{
    public enum Scene{
        GameMenu,
        LoadingScence,
        GamePlay,
        Lobby,
        SelectedCharacter
    }
    private static Scene targetSence;
    public static void Load(Scene targetSence){
        Loader.targetSence=targetSence;

        SceneManager.LoadScene(Scene.LoadingScence.ToString());
    }
    public static void NetworkLoad(Scene targerScene){
        NetworkManager.Singleton.SceneManager.LoadScene(targerScene.ToString(),LoadSceneMode.Single);
    }

    public static void LoaderCallback(){
        SceneManager.LoadScene(targetSence.ToString());
    }
}
