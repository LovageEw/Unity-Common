// Create an AssetBundle for Windows.

using UnityEngine;
using UnityEditor;

public class AssetBundleBuilder : MonoBehaviour
{
    [MenuItem("Tools/Build Asset Bundles")]
    static void BuildABs()
    {
        // Put the bundles in a folder called "ABs" within the Assets folder.
#if UNITY_IOS
        BuildPipeline.BuildAssetBundles("Assets/StreamingAssets", BuildAssetBundleOptions.None, BuildTarget.iOS);
#else
        BuildPipeline.BuildAssetBundles("Assets/Projects/AssetBundles", BuildAssetBundleOptions.None, BuildTarget.Android);
#endif
    }
}