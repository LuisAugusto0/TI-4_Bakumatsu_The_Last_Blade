using System.IO;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class TexturePostprocessor : AssetPostprocessor
    {
        private const string ExtensionPNG = ".png";
        private const string PixelMapSuffix = ".map";
        private const string PixelWeightsPrefix = "weights.";

        private void OnPostprocessTexture(Texture2D texture)
        {
            var fileName = Path.GetFileNameWithoutExtension(assetPath);
            var extension = Path.GetExtension(assetPath);

            if (extension != ExtensionPNG)
                return;

            if (fileName.EndsWith(PixelMapSuffix))
                ProcessPixelMap(texture, fileName);
            else if (fileName.StartsWith(PixelWeightsPrefix))
                ProcessPixelWeights(texture, fileName.Replace(PixelWeightsPrefix, ""));
        }

        private void ProcessPixelWeights(Texture2D texture, string fileName)
        {
            if (!fileName.Contains('_'))
                return;

            var mapName = fileName.Split('_')[0] + PixelMapSuffix;
            var map = FindPixelMap(mapName);
            if (map == null)
                return;

            var skinData = texture.GetPixels32();
            for (var i = 0; i < skinData.Length; i++)
            {
                if (map.lookup.TryGetValue(skinData[i], out var position))
                {
                    skinData[i].r = (byte)position.x;
                    skinData[i].g = (byte)position.y;
                    skinData[i].b = 0;
                    skinData[i].a = 255;
                }
                else
                {
                    skinData[i] = Color.clear;
                }
            }

            var path = assetPath.Replace(PixelWeightsPrefix, "");
            var skinTexture = new Texture2D(texture.width, texture.height);
            skinTexture.SetPixels32(skinData);
            File.WriteAllBytes(path, skinTexture.EncodeToPNG());
            AssetDatabase.ImportAsset(path);
        }

        private void ProcessPixelMap(Texture2D texture, string fileName)
        {
            var map = FindPixelMap(fileName);
            if (map == null)
            {
                map = ScriptableObject.CreateInstance<PixelMap>();
                map.name = fileName;
                AssetDatabase.CreateAsset(
                    map,
                    Path.Combine(Path.GetDirectoryName(assetPath), $"{fileName}.asset")
                );
            }

            map.data = texture.GetPixels32();
            map.lookup.Clear();
            for (var i = 0; i < map.data.Length; i++)
                if (map.data[i].a > 0)
                {
                    map.lookup[map.data[i]] = new Vector2Int(
                        i % texture.width,
                        i / texture.width
                    );
                }

            EditorUtility.SetDirty(map);
            AssetDatabase.SaveAssets();
        }
        
        private static PixelMap FindPixelMap(string fileName)
        {
            var guids = AssetDatabase.FindAssets("t:" + nameof(PixelMap));

            foreach (var guid in guids)
            {
                var asset = AssetDatabase.LoadMainAssetAtPath(
                    AssetDatabase.GUIDToAssetPath(guid)
                ) as PixelMap;

                if (asset != null && asset.name == fileName)
                    return asset;
            }

            return null;
        }
    }
}