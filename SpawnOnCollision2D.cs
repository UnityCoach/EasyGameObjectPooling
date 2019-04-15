//
//  SpawnOnCollision2D.cs
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
    [AddComponentMenu("UnityCoach/Spawning/Spawn On Collision 2D")]
    [RequireComponent(typeof(Collider2D))]
    public class SpawnOnCollision2D : MonoBehaviour
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

        ContactPoint2D[] points = new ContactPoint2D[1];
        void OnCollisionEnter2D(Collision2D col)
        {
            if (filterTag != string.Empty && !col.gameObject.CompareTag(filterTag))
                return;

            if (col.relativeVelocity.magnitude < magnitudeThreshold)
                return;

            if (col.GetContacts(points) < 1)
                return;

#if USE_POOLING
            ObjectPool.GetInstance(_referenceID, points[0].point, Quaternion.LookRotation(Vector3.back, -points[0].normal), sticky ? transform : null);
#else
		    Instantiate (reference, points[0].point, Quaternion.LookRotation (Vector3.back, -points[0].normal), sticky ? transform : null);
#endif
        }
    }
}