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
					foreach (ARPlane plane in addedPlanes)
					{
						GameObject instance = Instantiate(spawnObject, plane.center, plane.transform.rotation);
						spawnObjects.Add(plane.trackableId, instance);
					}
				}

				List<ARPlane> removedPlanes = obj.removed;

				if (removedPlanes.Count > 0)
				{
					foreach (ARPlane plane in removedPlanes)
					{
						GameObject destoryTarget = spawnObjects[plane.trackableId];
						Destroy(destoryTarget);
					}
				}

			};
			_arPlaneManager.planeAdded += OnPlaneAdded;
			_arPlaneManager.planeUpdated += OnPlaneUpdated;
		}

		void OnPlaneAdded(ARPlaneAddedEventArgs eventArgs)
		{
			Debug.Log("Plane Added!!");
			UpdateARPlane(eventArgs.plane);
		}

		void OnPlaneUpdated(ARPlaneUpdatedEventArgs eventArgs)
		{
			Debug.Log("Plane Updated!!");
			UpdateARPlane(eventArgs.plane);
		}

		void UpdateARPlane(ARPlane arPlane)
		{

			if (_didCacheId == false)
			{
				_planeId = arPlane.boundedPlane.Id;
				Debug.Log("Plane Id " + _planeId.ToString());
				_didCacheId = true;
			}

			if (arPlane.boundedPlane.Id == _planeId)
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
