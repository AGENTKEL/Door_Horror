using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CameraDoorScript
{
	public class CameraOpenDoor : MonoBehaviour
	{
		public float DistanceOpen = 3f;
		public GameObject text;
		public Inventory playerInventory;
		public Transform noteTarget;

		private Camera mainCam;

		private void Start()
		{
			mainCam = Camera.main;
		}

		void Update()
		{
			Ray ray = new Ray(mainCam.transform.position, mainCam.transform.forward);
			RaycastHit hit;

			if (Physics.Raycast(ray, out hit, DistanceOpen))
			{
				var door = hit.transform.GetComponent<DoorScript.Door>();
				var interactable = hit.transform.GetComponent<Interactable>();
				var candle = hit.transform.GetComponent<Candle>();

				if (door != null)
				{
					text.SetActive(true);
					if (Input.GetKeyDown(KeyCode.E))
					{
						door.TryOpenWithKey(playerInventory);
					}
				}
				else if (interactable != null)
				{
					text.SetActive(true);
					if (Input.GetKeyDown(KeyCode.E))
					{
						if (interactable.itemType == ItemType.Note)
						{
							interactable.MoveTo(noteTarget);
						}
						else
						{
							playerInventory.AddItem(interactable);
						}
					}
				}
				else if (candle != null)
				{
					text.SetActive(true);
					if (Input.GetKeyDown(KeyCode.E))
					{
						candle.Interact();
					}
				}
				else
				{
					text.SetActive(false);
				}
			}
			else
			{
				text.SetActive(false);
			}
		}
	}
}
