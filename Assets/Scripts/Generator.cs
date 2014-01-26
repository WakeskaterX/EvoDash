using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class Generator {

	static int numberPaths = 1;
	static int numberGenerated = 0;
	static List<Vector3> pathLocations = new List<Vector3> ();
	static float chunkWidth = 40f;
	public static GameObject player;
	public static GameObject spikes;
	public static GameObject ground;
	public static GameObject air;
	public static GameObject ghost;


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
		float lowestY = pathLocations[0].y;

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
				float lowestYInPath = path.y;
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
					newPlat.transform.localScale = new Vector3(vertScale, 1, 1);
					if (Random.Range(0, numberGenerated*3) > 2+numberGenerated) 
					{
						int rando = Random.Range (0, 1+numberGenerated);
						if (rando < 2)
							Object.Instantiate(ground, new Vector3(genLocation-platformWidth*scale/2, (platY + 1f)), Quaternion.identity);
						else if (rando <  5)
							Object.Instantiate(air, new Vector3(genLocation-platformWidth*scale/2, (platY + 1f)), Quaternion.identity);
						else
							Object.Instantiate(ghost, new Vector3(genLocation-platformWidth*scale/2, (platY + 1f)), Quaternion.identity);
					}
					oldY = platY;
					if (platY < lowestYInPath)
						lowestYInPath = platY;
				}
				if (lowestYInPath < lowestY)
				{
					newPaths.Add(new Vector3(end+2, platY+1));
					if (Random.Range (1, 7) < 3) newPaths.Add(new Vector3(end+2, platY-2));
					lowestY = lowestYInPath;
				}
				else if (Random.Range(1, 4) < 3 ) 
					newPaths.Add(new Vector3(end+2, platY+2));
			}
			float spikesStart = pathLocations [0].x;
			float spikesEnd = spikesStart + chunkWidth;
			float spikesWidth = spikes.GetComponent<BoxCollider2D> ().size.x;
			while (spikesStart < spikesEnd) 
			{
				Object.Instantiate(spikes, new Vector2(spikesStart, lowestY - 2), Quaternion.identity);
				spikesStart += spikesWidth;
			}
			GameObject newTrigger = (GameObject) Object.Instantiate(trigger, new Vector3(newPaths[0].x - chunkWidth/2, newPaths[0].y), trigger.transform.rotation);
			newTrigger.GetComponent<MakeNextChunk>().player = player;
			newTrigger.transform.localScale = new Vector3(100, 1, 1);
			pathLocations = newPaths;
			Debug.Log (pathLocations.ToString ());
			numberGenerated++;
		}
	}
}
