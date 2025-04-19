using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CameraDoorScript
{
	public class CameraOpenDoor : MonoBehaviour
	{
		public float DistanceOpen = 3;
		public GameObject text;
		public Inventory playerInventory;

		void Update()
		{
			RaycastHit hit;
			if (Physics.Raycast(transform.position, transform.forward, out hit, DistanceOpen))
			{
				var door = hit.transform.GetComponent<DoorScript.Door>();
				var interactable = hit.transform.GetComponent<Interactable>();

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
						playerInventory.AddItem(interactable);
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
