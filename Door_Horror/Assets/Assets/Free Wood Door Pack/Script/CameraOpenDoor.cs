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
		
		private ScreenManager currentlyLookedScreen;

		private Camera mainCam;

		private void Start()
		{
			mainCam = Camera.main;
		}

		void Update()
		{
			Ray ray = new Ray(mainCam.transform.position, mainCam.transform.forward);
			RaycastHit hit;
			
			bool hitScreen = false;
			

			if (Physics.Raycast(ray, out hit, DistanceOpen))
			{
				var door = hit.transform.GetComponent<DoorScript.Door>();
				var interactable = hit.transform.GetComponent<Interactable>();
				var candle = hit.transform.GetComponent<Candle>();
				var screen = hit.transform.GetComponent<ScreenManager>();

				if (screen != null)
				{
					hitScreen = true;

					// Only set looking if it's a new screen or we weren't already looking
					if (currentlyLookedScreen != screen)
					{
						if (currentlyLookedScreen != null)
							currentlyLookedScreen.SetLooking(false); // Stop looking at the previous one

						screen.SetLooking(true);
						currentlyLookedScreen = screen;
					}
				}

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
						interactable.Interact(playerInventory);
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
