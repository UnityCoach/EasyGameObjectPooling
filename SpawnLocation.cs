//
//  SpawnLocation.cs
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

namespace UnityCoach.Spawning
{
    [AddComponentMenu("UnityCoach/Spawning/Spawn Location")]
    public class SpawnLocation : MonoBehaviour
    {
        public float radius = 1;
        [SerializeField] Color _gizmoColor = new Color(1, 1, 0, 0.75F);

        void OnDrawGizmos()
        {
            Gizmos.color = _gizmoColor;
            Gizmos.DrawWireSphere(transform.position, radius);
        }
    }
}