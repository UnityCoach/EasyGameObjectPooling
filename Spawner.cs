//
//  Spawner.cs
//
//  Author:
//       Fred Moreau <info@unitycoach.ca>
//
//  Copyright (c) 2019 Frederic Moreau - Unity Coach / Jikkou Publishing Inc.
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.

#define USE_POOLING
using UnityEngine;
using System.Collections;
#if USE_POOLING
using UnityCoach.ObjectPooling;
#endif

namespace UnityCoach.Spawning
{
	[AddComponentMenu("UnityCoach/Spawning/Spawner")]
	public class Spawner : MonoBehaviour
	{
		[Header("SPAWN")]
		[SerializeField] GameObject reference;
#if USE_POOLING
		int _referenceID;
		[SerializeField] int _poolTotalObjects;
#endif

		[Header("SPAWNING")]
		[Range(0.001f, 100f)] public float minRate = 1.0f;
		[Range(0.001f, 100f)] public float maxRate = 1.0f;
		[SerializeField] bool infinite;
		[SerializeField] int number = 5;

		[Header("LOCATIONS")]
		[SerializeField] bool _2D;
		[SerializeField] SpawnLocation[] locations;
		Transform player;
		[SerializeField] float minDistanceFromPlayer;
		[SerializeField] bool randomOrientations;

		int _remaining;

		void Awake()
		{
			if (locations.Length == 0)
			{
				locations = new SpawnLocation[1];
				locations[0] = gameObject.AddComponent<SpawnLocation>();
			}
#if USE_POOLING
			_referenceID = ObjectPool.InitPool(reference, _poolTotalObjects);
#endif
		}

		Vector3 GetRandomPosition(Vector3 center, float radius, bool _in2D)
		{
			if (_in2D)
				return center + (Vector3)Random.insideUnitCircle * radius;
			else
				return center + Random.insideUnitSphere * radius;
		}

		IEnumerator Start()
		{
			_remaining = number;

			if (minDistanceFromPlayer > 0)
			{
				GameObject playerGO = GameObject.FindGameObjectWithTag("Player");
				if (playerGO)
					player = playerGO.transform;
				else
					Debug.LogWarning("No Player Found. Assign a Player tag to the Player Object");
			}

			while (infinite || _remaining > 0)
			{
				SpawnLocation location = locations[Random.Range(0, locations.Length)];
				Vector3 _position = GetRandomPosition(location.transform.position, location.radius, _2D);

				if (player && Vector3.Distance(_position, player.position) < minDistanceFromPlayer)
				{
					_position = (_position - player.position).normalized * minDistanceFromPlayer;
				}

#if USE_POOLING
				ObjectPool.GetInstance(_referenceID, _position,
					randomOrientations ?
						_2D ? Quaternion.AngleAxis(Random.Range(-180f, 180f), Vector3.forward)
						: Quaternion.Slerp(Quaternion.identity, Quaternion.Inverse(Quaternion.identity), Random.Range(0f, 1f))
					: transform.rotation);
#else
    			Instantiate (reference, _position,
	    			randomOrientations ?
						_2D ? Quaternion.AngleAxis(Random.Range(-180f, 180f), Vector3.forward)
						: Quaternion.Slerp(Quaternion.identity, Quaternion.Inverse(Quaternion.identity), Random.Range(0f, 1f))
					: transform.rotation);
#endif

				_remaining--;

				yield return new WaitForSeconds(1 / Random.Range(minRate, maxRate));
			}

			gameObject.SetActive(false);
		}

		void OnDrawGizmos()
		{
			if (locations.Length == 0)
				return;

			Gizmos.color = Color.gray;
			for (int i = 0; i < locations.Length; i++)
				Gizmos.DrawLine(transform.position, locations[i].transform.position);
		}
	}
}