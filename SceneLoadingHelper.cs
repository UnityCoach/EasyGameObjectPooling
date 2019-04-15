//
//  SceneLoadingHelper.cs
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

using UnityEngine;
using UnityEngine.SceneManagement;
using System;

namespace UnityCoach.ObjectPooling
{
	/// <summary>
	/// Scene loading static helper.
	/// Use this helper to load scenes when using persistant object pools,
	/// so that object pools can regather their objects before they eventually get destroyed by scene loading operations.
	/// Features static methods easily accessible from any scripts.
	/// SceneLoadingHelper features instance methods accessible from Unity Events.
	/// </summary>
	public static class SceneLoading
	{
		static public Action OnSceneLoad;

		#region static methods
		static public void LoadScene (string scene)
		{
			if (OnSceneLoad != null)
				OnSceneLoad.Invoke();
			SceneManager.LoadScene (scene);
		}

		static public void LoadScene (int sceneIndex)
		{
			if (OnSceneLoad != null)
				OnSceneLoad.Invoke();
			SceneManager.LoadScene (sceneIndex);
		}

		static public void LoadNextScene ()
		{
			int index = SceneManager.GetActiveScene().buildIndex +1;
			if (index >= SceneManager.sceneCountInBuildSettings)
				return;

			if (OnSceneLoad != null)
				OnSceneLoad.Invoke();

			SceneManager.LoadScene (index);
		}

		static public void LoadSceneAdditive (string scene)
		{
			SceneManager.LoadScene (scene, LoadSceneMode.Additive);
		}

		static public void LoadSceneAdditive (int sceneIndex)
		{
			SceneManager.LoadScene (sceneIndex, LoadSceneMode.Additive);
		}

		static public void LoadSceneAsync (string scene)
		{
			if (OnSceneLoad != null)
				OnSceneLoad.Invoke();

			SceneManager.LoadSceneAsync (scene);
		}

		static public void LoadSceneAsync (int sceneIndex)
		{
			if (OnSceneLoad != null)
				OnSceneLoad.Invoke();

			SceneManager.LoadSceneAsync (sceneIndex);
		}

		static public void LoadSceneAsyncAdditive (string scene)
		{
			SceneManager.LoadSceneAsync (scene, LoadSceneMode.Additive);
		}

		static public void LoadSceneAsyncAdditive (int sceneIndex)
		{
			SceneManager.LoadSceneAsync (sceneIndex, LoadSceneMode.Additive);
		}
		#endregion
	}

	/// <summary>
	/// Scene loading helper.
	/// Use this helper to load scenes when using persistant object pools,
	/// so that object pools can regather their objects before they eventually get destroyed by scene loading operations.
	/// Features instance methods accessible from Unity Events.
	/// </summary>
	[AddComponentMenu ("UnityCoach/Object Pooling/Scene Loading Helper")]
	public class SceneLoadingHelper : MonoBehaviour
	{
		#region instance methods
		public void LoadScene (string scene)
		{
			SceneLoading.LoadScene (scene);
		}

		public void LoadScene (int sceneIndex)
		{
			SceneLoading.LoadScene (sceneIndex);
		}

		public void LoadNextScene ()
		{
			SceneLoading.LoadNextScene ();
		}

		public void LoadSceneAdditive (string scene)
		{
			SceneLoading.LoadSceneAdditive (scene);
		}

		public void LoadSceneAdditive (int sceneIndex)
		{
			SceneLoading.LoadSceneAdditive (sceneIndex);
		}

		public void LoadSceneAsync (string scene)
		{
			SceneLoading.LoadSceneAsync (scene);
		}

		public void LoadSceneAsync (int sceneIndex)
		{
			SceneLoading.LoadSceneAsync (sceneIndex);
		}

		public void LoadSceneAsyncAdditive (string scene)
		{
			SceneLoading.LoadSceneAsyncAdditive (scene);
		}

		public void LoadSceneAsyncAdditive (int sceneIndex)
		{
			SceneLoading.LoadSceneAsyncAdditive (sceneIndex);
		}
		#endregion
	}
}