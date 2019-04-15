//
//  ReleaseOnRigidbodySleeping.cs
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

using System.Collections;
using UnityEngine;

namespace UnityCoach.ObjectPooling
{
	[AddComponentMenu ("UnityCoach/Object Pooling/Release On Rigidbody Sleeping")]
	[RequireComponent (typeof (Rigidbody))]
	public class ReleaseOnRigidbodySleeping : MonoBehaviour
	{
		Rigidbody _rb;
		[SerializeField] float _delay;
		WaitForSeconds waitForDelay;

		void Awake ()
		{
			_rb = GetComponent<Rigidbody>();
			waitForDelay = new WaitForSeconds (_delay);
		}

		void FixedUpdate ()
		{
			if (!_rb.IsSleeping())
				return;

			if (_delay <= 0)
				ObjectPool.Release (gameObject);
			else
				StartCoroutine (Release());
		}

		IEnumerator Release ()
		{
			yield return waitForDelay;
			ObjectPool.Release (gameObject);
		}
	}
}