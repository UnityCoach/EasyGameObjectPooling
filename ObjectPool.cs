//
//  ObjectPool.cs
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

using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace UnityCoach.ObjectPooling
{
	/// <summary>
	/// Object Pooling system designed to minimize the work implementing it.
	/// </summary>
	[AddComponentMenu ("UnityCoach/Object Pooling/Object Pool")]
	public class ObjectPool : MonoBehaviour
	{
		const int DEFAULT_POOL_SIZE = 200;

		private List<GameObject> _objectList;

		[Tooltip ("Object Reference")]
		[SerializeField] private GameObject _objectToRecycle;
		private int _objectToRecycleID;

		[Tooltip ("Total # of Objects to Instantiate at Start")]
		[SerializeField] private int _totalObjectsAtStart = 200;

		[Tooltip ("Should the Pool survive scene loadings")]
        [SerializeField] private bool _persistent;

		private static Dictionary<int, ObjectPool> pools = new Dictionary<int, ObjectPool> ();
		private static Dictionary<GameObject, Transform> parenting = new Dictionary<GameObject, Transform> ();

		private void OnDestroy ()
		{
			if (_objectList == null)
				return;
			
			for (int i = 0 ; i < _objectList.Count ; i++)
			{
				parenting.Remove (_objectList[i]);
				Destroy (_objectList[i]);
			}
			pools.Remove (_objectToRecycle.GetInstanceID());
		}

		private void Awake ()
		{
			if (!_objectToRecycle)
				return;
			
			_objectToRecycleID = _objectToRecycle.GetInstanceID();
			Init();
		}

		private void Init ()
		{
			if (pools.ContainsKey (_objectToRecycleID))
				return;
			
			_objectList = new List<GameObject> (_totalObjectsAtStart);

			for (int i = 0 ; i < _totalObjectsAtStart ; i++)
			{
				InstantiateNewObject();
			}

			pools.Add (_objectToRecycleID, this);

			if (_persistent)
			{
				DontDestroyOnLoad (gameObject);
                SceneLoading.OnSceneLoad += Regather;
			}
		}

		private void Regather ()
		{
			for (int i = 0 ; i < _objectList.Count ; i++)
				Release (_objectList[i]);
		}

		private GameObject InstantiateNewObject (Vector3 position = default (Vector3), Quaternion rotation = default (Quaternion), bool deactivate = true)
		{
			GameObject newObject = Instantiate (_objectToRecycle, position, rotation, transform);
			newObject.SetActive (!deactivate);
			_objectList.Add (newObject);
			parenting.Add (newObject, transform);
			return newObject;
		}

		private GameObject PoolObject (Vector3 position = default (Vector3), Quaternion rotation = default (Quaternion), Transform parent = null, bool instantiateInWorldSpace = false)
		{
			var freeObject = (from item in _objectList
				where item.activeSelf == false
				select item).FirstOrDefault();

			if (freeObject == null)
			{
				freeObject = InstantiateNewObject (position, rotation, false);
			}
			else
			{
				freeObject.transform.position = position;
				freeObject.transform.rotation = rotation;
				freeObject.SetActive (true);
			}

			if (parent != null)
				freeObject.transform.SetParent(parent, !instantiateInWorldSpace);

			return freeObject;
		}

		/// <summary>
		/// Initialiases the Pool.
		/// </summary>
		/// <returns>The pool.</returns>
		/// <param name="original">Original.</param>
		/// <param name="poolSize">Pool size.</param>
		/// <param name="persistentPool">If set to <c>true</c> persistent pool.</param>
		public static int InitPool (GameObject original, int poolSize = DEFAULT_POOL_SIZE, bool persistentPool = false)
		{
			int id = original.GetInstanceID();
			if (!pools.ContainsKey (id))
			{
				ObjectPool pool = new GameObject ("ObjectPool: " + original.name, new System.Type[1] {typeof (ObjectPool)}).GetComponent<ObjectPool>();
				pool._objectToRecycle = original;
				pool._objectToRecycleID = id;
				pool._totalObjectsAtStart = poolSize;
                pool._persistent = persistentPool;
				pool.Init();
			}
			return id;
		}

		/// <summary>
		/// Activates and returns the next available object instance.
		/// </summary>
		/// <returns>the object instance.</returns>
		/// <param name="originalID">Original I.</param>
		/// <param name="position">Position.</param>
		/// <param name="rotation">Rotation.</param>
		/// <param name="parent">Parent.</param>
		/// <param name="instantiateInWorldSpace">If set to <c>true</c> instantiate in world space.</param>
		public static GameObject GetInstance (int originalID, Vector3 position = default (Vector3), Quaternion rotation = default (Quaternion), Transform parent = null, bool instantiateInWorldSpace = false)
		{
			return pools[originalID].PoolObject(position, rotation, parent, instantiateInWorldSpace);
		}

		/// <summary>
		/// Activates and returns the next available object instance.
		/// </summary>
		/// <returns>the object instance.</returns>
		/// <param name="original">Original.</param>
		/// <param name="position">Position.</param>
		/// <param name="rotation">Rotation.</param>
		/// <param name="parent">Parent.</param>
		/// <param name="instantiateInWorldSpace">If set to <c>true</c> instantiate in world space.</param>
		/// <param name="poolSize">Pool size.</param>
		/// <param name="persistantPool">If set to <c>true</c> persistant pool.</param>
		public static GameObject GetInstance (GameObject original, Vector3 position = default (Vector3), Quaternion rotation = default (Quaternion), Transform parent = null, bool instantiateInWorldSpace = false, int poolSize = DEFAULT_POOL_SIZE, bool persistantPool = false)
		{
			int id = ObjectPool.InitPool (original);
			return pools[id].PoolObject(position, rotation, parent, instantiateInWorldSpace);
		}

		/// <summary>
		/// Release the specified game object.
		/// </summary>
		/// <param name="obj">Object instance to release.</param>
		public static void Release (GameObject obj)
		{
			obj.transform.SetParent (parenting[obj]);
			obj.SetActive (false);
		}
	}
}