using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Experimental.XR;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR;
using UnityEngine.XR.ARSubsystems;

namespace UnityARInterface
{
	public class ARPlaneHandler : MonoBehaviour
	{
		public static Action resetARPlane;
		public static Action<ARPlane> returnARPlane;
		private TrackableId _planeId;
		private ARPlane _cachedARPlane;
		private bool _didCacheId = false;

		ARPlaneManager _arPlaneManager;

		void Awake()
		{
			_arPlaneManager = GetComponent<ARPlaneManager>();
			_arPlaneManager.planesChanged += (ev) =>
			{
				List<ARPlane> addedPlanes = ev.added;
				if (addedPlanes.Count > 0)
				{
					Debug.Log("Plane Added!!");
					foreach( var pl in addedPlanes)
					{
						UpdateARPlane(pl);
					}
					
				}

				List<ARPlane> updatedPlanes = ev.updated;

				if (updatedPlanes.Count > 0)
				{
					foreach (var pl in updatedPlanes)
					{
						UpdateARPlane(pl);
					}
				}
				//{
				//	foreach (ARPlane plane in removedPlanes)
				//	{
				//		GameObject destoryTarget = spawnObjects[plane.trackableId];
				//		Destroy(destoryTarget);
				//	}
				

			};
			//_arPlaneManager.planeAdded += OnPlaneAdded;
			//_arPlaneManager.planeUpdated += OnPlaneUpdated;
		}

		//void OnPlaneAdded(ARPlaneAddedEventArgs eventArgs)
		//{
		//	Debug.Log("Plane Added!!");
		//	UpdateARPlane(eventArgs.plane);
		//}

		//void OnPlaneUpdated(ARPlaneUpdatedEventArgs eventArgs)
		//{
		//	Debug.Log("Plane Updated!!");
		//	UpdateARPlane(eventArgs.plane);
		//}

		void UpdateARPlane(ARPlane arPlane)
		{

			if (_didCacheId == false)
			{

				_planeId = arPlane.trackableId; // arPlane.boundedPlane.Id;
				Debug.Log("Plane Id " + _planeId.ToString());
				_didCacheId = true;
			}

			if (arPlane.trackableId == _planeId)//arPlane.boundedPlane.Id 
			{
				_cachedARPlane = arPlane;
				Debug.Log("Cached Plane " + _planeId.ToString());
			}

			if (returnARPlane != null)
			{
				returnARPlane(_cachedARPlane);
			}
		}
	}
}
