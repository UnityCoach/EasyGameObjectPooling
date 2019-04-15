//
//  SpawnOnCollision.cs
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
#if USE_POOLING
using UnityCoach.ObjectPooling;
#endif

namespace UnityCoach.Spawning
{
    [AddComponentMenu("UnityCoach/Spawning/Spawn On Collision")]
    [RequireComponent(typeof(Collider))]
    public class SpawnOnCollision : MonoBehaviour
    {
        [SerializeField] string filterTag;
        [SerializeField] float magnitudeThreshold;
        [SerializeField] bool sticky;

        [Header("SPAWN")]
        [SerializeField] GameObject reference;
#if USE_POOLING
        int _referenceID;
        [SerializeField] int _poolTotalObjects;
#endif

        void Awake()
        {
#if USE_POOLING
            _referenceID = ObjectPool.InitPool(reference, _poolTotalObjects);
#endif
        }

        ContactPoint[] points = new ContactPoint[1];
        void OnCollisionEnter(Collision col)
        {
    		if (filterTag != string.Empty && !col.gameObject.CompareTag(filterTag))
    			return;

            if (col.relativeVelocity.magnitude < magnitudeThreshold)
                return;

            points = col.contacts;
            if (points.Length < 1)
                return;

#if USE_POOLING
            ObjectPool.GetInstance(_referenceID, points[0].point, Quaternion.LookRotation(Vector3.back, -points[0].normal), sticky ? transform : null);
#else
    		Instantiate (reference, points[0].point, Quaternion.LookRotation (Vector3.back, -points[0].normal), sticky ? transform : null);
#endif
        }
    }
}