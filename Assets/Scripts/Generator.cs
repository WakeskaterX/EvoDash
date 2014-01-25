using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class Generator {

	static int numberPaths = 1;
	static List<Vector3> pathLocations = new List<Vector3> ();
	static float chunkWidth = 30f;
	public static GameObject player;


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
				float platY = path.y;
				float platformWidth = platform.GetComponent<BoxCollider2D>().size.x;
				while (genLocation < end)
				{
					int scale = Random.Range (1, (int)Mathf.Floor(data.curRunSpeed));
					genLocation+=(platformWidth+0.25f)*scale;
					platY += Random.Range(-1, (int)Mathf.Floor(data.curJumpHeight));
					GameObject newPlat = (GameObject) Object.Instantiate(platform, new Vector3(genLocation, platY), Quaternion.identity);
					newPlat.transform.localScale = new Vector3(scale, 1, 1);
				}
				GameObject newTrigger = (GameObject) Object.Instantiate(trigger, new Vector3(end - chunkWidth/2, path.y), trigger.transform.rotation);
				newTrigger.GetComponent<MakeNextChunk>().player = player;
				newTrigger.transform.localScale = new Vector3(100, 1, 1);
				newPaths.Add(new Vector3(end, platY));
			}
			pathLocations = newPaths;
			Debug.Log (pathLocations.ToString ());
		}
	}
}
