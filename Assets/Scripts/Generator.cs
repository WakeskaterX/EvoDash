using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class Generator {

	static int numberPaths = 1;
	static List<Vector3> pathLocations = new List<Vector3> ();
	static float chunkWidth = 30f;


	public static void MakeChunk(PlayerDataCapsule data, GameObject platform, GameObject trigger)
	{

	}

	interface ChunkGenerator 
	{
		void GenerateChunk(PlayerDataCapsule data, GameObject platform, GameObject trigger);
	}

 	class BasicGenerator : ChunkGenerator
	{
		public void GenerateChunk(PlayerDataCapsule data, GameObject platform, GameObject trigger)
		{
			foreach (Vector3 path in pathLocations) 
			{
				float genLocation = path.x;
				float end = start+chunkWidth;
				
			}
		}
	}
}
