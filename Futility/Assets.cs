﻿using System.IO;
using System.Reflection;
using UnityEngine;

namespace FUtility
{
    public class Assets
    {
        public static Texture2D LoadTexture(string name, string directory = null)
        {
            Texture2D texture = null;
            if (directory == null)
                directory = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "assets");
            var texFile = Path.Combine(directory, name + ".png");

            if (File.Exists(texFile))
            {
                var data = File.ReadAllBytes(texFile);
                texture = new Texture2D(1, 1);
                texture.LoadImage(data);
            }
            else
                Debug.LogError($"Could not load texture at path {texFile}.");
            return texture;
        }

        public static TextureAtlas GetCustomAtlas(string fileName, string folder, TextureAtlas tileAtlas)
        {
            string path = Utils.ModPath;
            if (folder != null)
                path = Path.Combine(path, folder);
            var tex = LoadTexture(fileName, path);
            if (tex == null) return null;

            TextureAtlas atlas;
            atlas = ScriptableObject.CreateInstance<TextureAtlas>();
            atlas.texture = tex;
            atlas.scaleFactor = tileAtlas.scaleFactor;
            atlas.items = tileAtlas.items;

            return atlas;
        }


        public static AssetBundle LoadAssetBundle(string assetBundleName)
        {
            foreach (var bundle in AssetBundle.GetAllLoadedAssetBundles())
            {
                if (bundle.name == assetBundleName)
                {
                    return bundle;
                }
            }

            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "assets", assetBundleName);
            AssetBundle assetBundle = AssetBundle.LoadFromFile(path);

            if (assetBundle == null)
            {
                Log.Warning($"Failed to load AssetBundle from path {path}");
                return null;
            }

            return assetBundle;
        }

        public static Mesh Square(GameObject parent, float width = 1f, float height = 1f)
        {
            MeshFilter meshFilter = parent.AddComponent<MeshFilter>();
            Mesh mesh = new Mesh
            {
                vertices = new Vector3[4]
                {
                    new Vector3(0, 0, 0),
                    new Vector3(width, 0, 0),
                    new Vector3(0, height, 0),
                    new Vector3(width, height, 0)
                },

                triangles = new int[6]
                {
                    0, 2, 1,
                    2, 3, 1
                },

                normals = new Vector3[4]
                {
                    -Vector3.forward,
                    -Vector3.forward,
                    -Vector3.forward,
                    -Vector3.forward
                },

                uv = new Vector2[4]
                {
                    new Vector2(0, 0),
                    new Vector2(1, 0),
                    new Vector2(0, 1),
                    new Vector2(1, 1)
                }
            };

            meshFilter.mesh = mesh;

            return mesh;
        }
    }
}
