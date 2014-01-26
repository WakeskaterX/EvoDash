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
				float oldY = platY;
				float platformWidth = platform.GetComponent<BoxCollider2D>().size.x;
				while (genLocation < end)
				{
					int scale = Random.Range (1, (int)Mathf.Floor(data.curRunSpeed*10))/10;
					genLocation+=platformWidth*scale/2;
					platY += Random.Range(-1, (int)Mathf.Floor(data.curJumpHeight*10)/20);
					GameObject newPlat = (GameObject) Object.Instantiate(platform, new Vector3(genLocation, platY), Quaternion.identity);
					newPlat.transform.localScale = new Vector3(scale, 1, 1);
					genLocation+=platformWidth*scale/2;
					float vertScale = Mathf.Abs (platY - oldY);
					newPlat = (GameObject) Object.Instantiate(platform, new Vector3(genLocation-platformWidth*scale, (platY+oldY)/2), platform.transform.rotation);
					newPlat.transform.Rotate(new Vector3(0, 0, 1), 90);
					oldY = platY;
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
