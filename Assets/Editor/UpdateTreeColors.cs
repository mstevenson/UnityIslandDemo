using UnityEditor;
using UnityEngine;

public class UpdateTreeColors : MonoBehaviour
{
	[MenuItem("Terrain/Update Tree Lightmap Color")]
	static void RebuildWithLightmap () {
		Texture2D tex = Selection.activeObject as Texture2D;
		if (tex)
		{
			if (Terrain.activeTerrain == null || Terrain.activeTerrain.terrainData == null)
			{
				EditorUtility.DisplayDialog("No active terrain in the scene", "No active terrain in the scene", "Ok");
				return;
			}
			
			Undo.RegisterUndo(Terrain.activeTerrain.terrainData, "Set Tree colors");
			//UnityEditor.TerrainLightmapper.UpdateTreeLightmapColor(tex, Terrain.activeTerrain.terrainData); /**/
		}
		else
			EditorUtility.DisplayDialog("Select a lightmap", "Select a lightmap", "Ok");
	}

	[MenuItem("Terrain/Update Tree Color")]
	static void RebuildWithColor () {
		var tex = Selection.activeObject as Texture2D;
		if (tex)
		{
			if (Terrain.activeTerrain == null || Terrain.activeTerrain.terrainData == null)
			{
				EditorUtility.DisplayDialog("No active terrain in the scene", "No active terrain in the scene", "Ok");
				return;
			}
			
			Undo.RegisterUndo(Terrain.activeTerrain.terrainData, "Set Tree colors");
			//UnityEditor.TerrainLightmapper.UpdateTreeColor(tex, Terrain.activeTerrain.terrainData); /**/
		}
		else
			EditorUtility.DisplayDialog("Select a lightmap", "Select a lightmap", "Ok");
	}	
}
