using UnityEngine;
using System;
using System.Threading; 
namespace BlasphemousExtractor
{
    public class Loader
    {
    	public static int DllMain(String arg)
        {
            new Thread(Init).Start();
            return 0;
        } 
        public static void Init()
        {
            _Load = new GameObject();
            _Load.AddComponent<Main>();
            GameObject.DontDestroyOnLoad(_Load);
        }
        public static void Unload()
        {
            _Unload();
        }
        private static void _Unload()
        {
            GameObject.Destroy(_Load);
        }
        private static GameObject _Load;
    }
}