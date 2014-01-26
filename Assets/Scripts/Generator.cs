using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class Generator {

	static int numberPaths = 1;
	public static int numberGenerated = 0;
	static List<Vector3> pathLocations = new List<Vector3> ();
	static float chunkWidth = 40f;
	public static GameObject player;
	public static GameObject spikes;
	public static GameObject ground;
	public static GameObject air;
	public static GameObject ghost;


	public static void MakeChunk(PlayerDataCapsule data, GameObject platform, GameObject trigger)
	{
		int rand = Random.Range(0,3);
		if (rand == 1)
			new BasicGenerator ().GenerateChunk (data, platform, trigger);
		else if (rand == 0)
			new ScatterGenerator().GenerateChunk (data,platform,trigger);
		else 
			new FlatGenerator().GenerateChunk(data,platform,trigger);

		numberGenerated++;
	}

	public static void AddPath(Vector3 path)
	{
		pathLocations.Add (path);
	}

	public static void ClearPaths(){
		pathLocations.Clear ();
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
					if (Random.Range(0, numberGenerated*2) > 1+numberGenerated) 
					{
						int rando = Random.Range (0, 100);
						if (rando > 100 - Mathf.Min (numberGenerated,10f)){
							Object.Instantiate(ground, new Vector3(genLocation-platformWidth*scale/2, (platY + 3f)), Quaternion.identity);
						}else if (rando > 100 - Mathf.Min (numberGenerated*3.5f,35f))
						{
							GameObject enFly;
							enFly = Object.Instantiate(air, new Vector3(genLocation-platformWidth*scale/2, (platY + 3f)), Quaternion.identity) as GameObject;
							enFly.GetComponent<EnemyFlying>().player = player; 
						}
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
		}
	}

	class ScatterGenerator: ChunkGenerator
	{
		float lowestY = pathLocations[0].y;
		
		public void GenerateChunk(PlayerDataCapsule data, GameObject platform, GameObject trigger)
		{
			List<Vector3> newPaths = new List<Vector3> ();

			foreach (Vector3 path in pathLocations) 
			{
				float genLocation = path.x;
				float end = genLocation+chunkWidth;
				float platY = path.y ;
				float oldY = platY;
				float platformWidth = platform.GetComponent<BoxCollider2D>().size.x;
				float lowestYInPath = path.y;
				float highestYInPath = path.y;

				while (genLocation < end)
				{
					float scale = Random.Range (4f, (float)Mathf.Floor(data.curRunSpeed*8f))/10f;
					genLocation+=platformWidth*scale/1.5f;
					platY += Random.Range(-2f, (float)Mathf.Floor(data.curJumpHeight*3f)/10f);
					GameObject newPlat = (GameObject) Object.Instantiate(platform, new Vector3(genLocation, platY), Quaternion.identity);
					newPlat.transform.localScale = new Vector3(scale * Random.Range (.7f,1.3f), Random.Range (1f,3f), 1);
					newPlat.GetComponent<PlatformDestruction>().player = player;
					genLocation+=platformWidth*scale/1f;
					float vertScale = Mathf.Abs (platY - oldY);
					newPlat = (GameObject) Object.Instantiate(platform, new Vector3(genLocation + Random.Range(-platformWidth*scale,platformWidth*scale), (platY+oldY)/2), platform.transform.rotation);
					newPlat.transform.Rotate(new Vector3(0, 0, 1), 90);
					newPlat.transform.localScale = new Vector3(vertScale, Random.Range (1f,1.5f), 1);
					newPlat.GetComponent<PlatformDestruction>().player = player;

					if (data.curJumpHeight > 7.5){
						newPlat = Object.Instantiate (platform, new Vector3(genLocation + Random.Range (0f,platformWidth*scale),platY + Random.Range (2f,5f),0f),platform.transform.rotation) as GameObject;
						newPlat.transform.localScale = new Vector3(Random.Range(.3f,.7f),Random.Range(1f,2f),1f);
						newPlat.GetComponent<PlatformDestruction>().player = player;
					}

					                                          
					if (Random.Range(0, numberGenerated*2) > 1+numberGenerated) 
					{
						int rando = Random.Range (0, 100);
						if (rando > 100 - Mathf.Min (numberGenerated,10f)){
							Object.Instantiate(ground, new Vector3(genLocation-platformWidth*scale/2, (platY + 3f)), Quaternion.identity);
						}else if (rando > 100 - Mathf.Min (numberGenerated*3.5f,35f))
						{
							GameObject enFly;
							enFly = Object.Instantiate(air, new Vector3(genLocation-platformWidth*scale/2, (platY + 3f)), Quaternion.identity) as GameObject;
							enFly.GetComponent<EnemyFlying>().player = player; 
						}
					}
					oldY = platY;
					if (platY < lowestYInPath)
						lowestYInPath = platY;

					if (platY > highestYInPath)
					{ highestYInPath = platY;}

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
				Object.Instantiate(spikes, new Vector2(spikesStart, lowestY - 5), Quaternion.identity);
				spikesStart += spikesWidth;
			}
			GameObject newTrigger = (GameObject) Object.Instantiate(trigger, new Vector3(newPaths[0].x - chunkWidth/2, newPaths[0].y), trigger.transform.rotation);
			newTrigger.GetComponent<MakeNextChunk>().player = player;
			newTrigger.transform.localScale = new Vector3(1000, 1, 1);
			pathLocations = newPaths;

		}
	}

	class FlatGenerator : ChunkGenerator
	{
		public void GenerateChunk(PlayerDataCapsule data, GameObject platform, GameObject trigger)
		{
			List<Vector3> newPaths = new List<Vector3> ();
			foreach(Vector3 path in pathLocations)
			{
				float genLocation = path.x;
				float end = genLocation+chunkWidth;
				float platY = path.y ;
				float oldY = platY;
				float platformWidth = platform.GetComponent<BoxCollider2D>().size.x;
				float lowestYInPath = path.y;
				float highestYInPath = path.y;

				GameObject bigPlat = (GameObject) Object.Instantiate(platform, new Vector3(genLocation+chunkWidth/2, path.y), Quaternion.identity);
				bigPlat.transform.localScale = new Vector3(chunkWidth/platformWidth, 1, 1);
				while (genLocation < end)
				{
					genLocation+=Random.Range (data.curRunSpeed/2*3, data.curRunSpeed*3);
					float height = Random.Range(0, data.curJumpHeight/2-1);
					if (data.numberDashes > 5 && Random.Range (0, 2) == 0)
					{
						GameObject top = (GameObject) Object.Instantiate(platform, new Vector3(genLocation, path.y + 6 + height), Quaternion.identity);
						top.transform.Rotate (new Vector3(0, 0, 1), 90);
						top.transform.localScale = new Vector3(10/platformWidth, 1, 1);
						GameObject bottom = (GameObject) Object.Instantiate(platform, new Vector3(genLocation, path.y - 5 + height + Mathf.Min (0.5f, numberGenerated/10f)), Quaternion.identity);
						bottom.transform.Rotate (new Vector3(0, 0, 1), 90);
						bottom.transform.localScale = new Vector3(10/platformWidth, 1, 1);
					}
					else {
						GameObject wallPlat = (GameObject) Object.Instantiate(platform, new Vector3(genLocation, path.y + height/2), Quaternion.identity);
						wallPlat.transform.Rotate (new Vector3(0, 0, 1), 90);
						wallPlat.transform.localScale = new Vector3(height, 1, 1);
					}
				}
				genLocation -= chunkWidth;
				while ( genLocation < end)
				{
					genLocation+= Random.Range (2, Mathf.Max (1, 10 - numberGenerated));
					int rand = Random.Range (0, numberGenerated+2);
					if (rand < 2) { }
					else if (rand < 5)
						Object.Instantiate(ground, new Vector3(genLocation, path.y + 0.5f), Quaternion.identity);
					else
						Object.Instantiate(air, new Vector3(genLocation, path.y + 0.75f), Quaternion.identity);
				}

				newPaths.Add(new Vector3(end, platY));
			}
			GameObject newTrigger = (GameObject) Object.Instantiate(trigger, new Vector3(newPaths[0].x - chunkWidth/2, newPaths[0].y), trigger.transform.rotation);
			newTrigger.GetComponent<MakeNextChunk>().player = player;
			newTrigger.transform.localScale = new Vector3(100, 1, 1);
			pathLocations = newPaths;
		}
	}
}
