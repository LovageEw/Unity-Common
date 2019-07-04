using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace UIs
{
    [RequireComponent(typeof(Image))]
    public class ImageLoader : MonoBehaviour
    {
        [SerializeField] private bool loadOnStart = true;
        [SerializeField] private string assetBundleFolderName = "AssetBundle";
        [SerializeField] private ImageLoadMode loadMode;
        [SerializeField] private string categoryPath;
        [SerializeField] private string fileName;

        public void Start()
        {
            if (loadOnStart)
            {
                Load(fileName);
            }
        }

        public void Load(string fileName)
        {
            var image = GetComponent<Image>();
            switch (loadMode)
            {
                case ImageLoadMode.StreamingAsset:
                    image.sprite = LoadSprite(Application.streamingAssetsPath, fileName);
                    break;
                case ImageLoadMode.Temporary:
                    image.sprite = LoadSprite(Application.temporaryCachePath, fileName);
                    break;
                case ImageLoadMode.Resource:
                    image.sprite = Resources.Load<Sprite>($"{categoryPath}/{fileName}");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private Sprite LoadSprite(string dataPath, string fileName)
        {
            var texture = Texture2DFromFile(dataPath, categoryPath, fileName);
            return texture ? SpriteFromTexture2D(texture) : null;
        }

        private class AssetBundleUsage
        {
            public AssetBundle AssetBundle { get; set; }
            public int UserCount { get; set; }
        }
        
        private static readonly Dictionary<string, AssetBundleUsage> AssetBundlesDict = new Dictionary<string, AssetBundleUsage>();
        private Texture2D Texture2DFromFile(string dataPath, string categoryPath, string fileName)
        {
            if (AssetBundlesDict.ContainsKey(categoryPath))
            {
                AssetBundlesDict[categoryPath].UserCount++;
                var texture = AssetBundlesDict[categoryPath].AssetBundle.LoadAsset<Texture2D>(fileName);
                StartCoroutine(Unload(categoryPath));
                return texture;
            }
            
            var assetBundle = AssetBundle.LoadFromFile($"{dataPath}/{categoryPath}");
            var usage = new AssetBundleUsage {AssetBundle = assetBundle, UserCount = 1};
            AssetBundlesDict.Add(categoryPath, usage);
            StartCoroutine(Unload(categoryPath));
            return AssetBundlesDict[categoryPath].AssetBundle.LoadAsset<Texture2D>(fileName);
        }

        private static IEnumerator Unload(string category)
        {
            yield return new WaitForSeconds(0.05f);
            
            if(!AssetBundlesDict.ContainsKey(category)){ yield break; }
            if (AssetBundlesDict[category].UserCount != 1)
            {
                AssetBundlesDict[category].UserCount--;
                yield break;
            }
            
            AssetBundlesDict[category].AssetBundle.Unload(false);
            AssetBundlesDict.Remove(category);
            UnityConsole.Log("Asset bundle " + category + " removed.");
        }

        private Sprite SpriteFromTexture2D(Texture2D texture)
        {
            return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
        }
    }
}