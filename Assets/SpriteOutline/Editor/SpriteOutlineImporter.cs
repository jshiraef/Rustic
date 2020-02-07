using UnityEngine;
using UnityEditor;

public class SpriteOutlineImporter : AssetPostprocessor {

	void OnPreprocessTexture() {
		TextureImporter importer = (TextureImporter)assetImporter;

		if (!assetPath.Contains (SpriteOutline2.IMAGE_EXT))
			return;

		importer.filterMode          = FilterMode.Point;
		importer.alphaIsTransparency = false;
	}

}
