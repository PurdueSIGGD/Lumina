using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>
/// Tool helps to convert normal Texture2D to Sprite automatically
/// </summary>
public class SpriteProcessor : AssetPostprocessor {

	void OnPostprocessTexture(Texture2D texture)
    {
        //get asset from AssetPostprocessor
        string lowerCaseAssetPath = assetPath.ToLower();

        //see if this asset is in folder "/sprites" or "/sprite"
        bool isInSpritesDirectory = 
            lowerCaseAssetPath.IndexOf("/sprites/") != -1 
            || lowerCaseAssetPath.IndexOf("/sprite/") != -1;

        //if in 
        if (isInSpritesDirectory)
        {
            //make it to Sprites
            TextureImporter textureImporter = (TextureImporter)assetImporter;
            textureImporter.textureType = TextureImporterType.Sprite;

        }

    }
}
