using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DoorScript
{
	public enum DoorColor
	{
		Yellow,
		Red,
		Black
	}

	[RequireComponent(typeof(AudioSource))]
	public class Door : MonoBehaviour
	{
		public bool open;
		public float smooth = 1.0f;
		float DoorOpenAngle = -90.0f;
		float DoorCloseAngle = 0.0f;

		public DoorColor doorColor;

		public AudioSource asource;
		public AudioClip openDoor, closeDoor;

		void Start()
		{
			asource = GetComponent<AudioSource>();
		}

		void Update()
		{
			var targetAngle = open ? DoorOpenAngle : DoorCloseAngle;
			var targetRotation = Quaternion.Euler(0, targetAngle, 0);
			transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, Time.deltaTime * 5 * smooth);
		}

		public void OpenDoor()
		{
			open = !open;
			asource.clip = open ? openDoor : closeDoor;
			asource.Play();

			if (open)
			{

				Game_Manager.instance.OnDoorEntered(doorColor);
			}
		}
	}
}