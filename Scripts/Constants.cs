using UnityEngine;
using System.Collections;

namespace Constants {

    public static class DBPathes {
        public static string ResourceDb = "resource.sqlite";
        public static string LocalDb = "local.sqlite";
    }

    public static class TextAssetPathes {
        public static string ResourcePath = "Databases/Resource";
    }

    public static class DOTweenConstant
    {
        public static int InfiniteLoop = -1;
    }

    public static class DefaultSetting {
        public static string DefaultName = "NoName";
        public static float DefaultNameR = 1.0f;
        public static float DefaultNameG = 1.0f;
        public static float DefaultNameB = 1.0f;
        public static float DefaultMusicVolume = 1.0f;
        public static float DefaultSoundVolume = 1.0f;
        public static float TargetFps = 60.0f;
    }
    
    public static class CalcHelper
    {
        public static float FloatTolerance = 0.001f;
    }
}