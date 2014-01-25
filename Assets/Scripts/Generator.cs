using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class Generator {

	static int numberPaths = 1;
	static List<Vector3> pathLocations = new List<Vector3> ();
	static float chunkWidth = 30f;


	public static void MakeChunk(PlayerDataCapsule data, GameObject platform, GameObject trigger)
	{
		new BasicGenerator ().GenerateChunk (data, platform, trigger);
	}

	public static void AddPath(Vector3 path)
	{
		pathLocations.Add (path);
	}

	interface ChunkGenerator 
	{
		void GenerateChunk(PlayerDataCapsule data, GameObject platform, GameObject trigger);
	}

 	class BasicGenerator : ChunkGenerator
	{
		public void GenerateChunk(PlayerDataCapsule data, GameObject platform, GameObject trigger)
		{
			List<Vector3> newPaths = new List<Vector3> ();
			foreach (Vector3 path in pathLocations) 
			{
				float genLocation = path.x;
				float end = genLocation+chunkWidth;
				float platformWidth = platform.GetComponent<BoxCollider2D>().size.x;
				while (genLocation < end)
				{
					genLocation+=platformWidth+1f;
					Object.Instantiate(platform, new Vector3(genLocation, path.y), Quaternion.identity);
				}
				newPaths.Add(new Vector3(end, path.y));
			}
		}
	}
}
