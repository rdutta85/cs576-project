using OpenCover.Framework.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
 
 For Tele: Implement any additional items here.
 
 */


public class JunkochanControl : MonoBehaviour
{
	private float InputH;//Horizontal Input (A & D key)
	private float InputV;//Vertical Input (W & S key)
	private GameObject JKCCam;//Camera which chases Junkochan
	private CharacterController JKCController;//Character controller component attached to Junkochan
	private Animator JKCAnim;//Animator component attached to Junkochan
	private float MoveSpeed = 3f;//Horizontal move speed
	private float move_velocity = 0;
	private Vector3 MoveDirection;
	private float Height;//Current height of Junkochan (y value of transform.position)
	public ParticleSystem ParticleSystem;
	public float Health = 100;
	public float VertSpeed = 0;
	public float AttackRange = 1.5f;
	public float[] AttackDamage = { 25f, 33f };
	private string CurrentScene;
	private Vector3 movement_direction;
	public float t;

	// Use this for initialization
	void Start()
	{
		JKCCam = GameObject.Find("Main Camera");
		JKCController = this.GetComponent<CharacterController>();
		JKCAnim = this.GetComponent<Animator>();
		JKCAnim.SetBool("Jump", false);
		JKCAnim.SetBool("Grounded", true);
		JKCAnim.SetBool("Fall", false);

		CurrentScene = SceneManager.GetActiveScene().name;
		t = 1;
	}

	// Update is called once per frame
	void Update()
	{

		#region ACTION		
		if (
			JKCAnim.GetCurrentAnimatorStateInfo(1).IsName("Sword_Iai") ||
			JKCAnim.GetCurrentAnimatorStateInfo(1).IsName("Sword_Guard") ||
			JKCAnim.GetCurrentAnimatorStateInfo(1).IsName("Sword_Store")
		)
		{
			// While the attacking / guarding animation is playing, 
			// do not go to below "MOVEMENT" process  
			return;
		}

		JKCAnim.SetBool("IsDamaged", false);

		if (Health == 0)
		{
			JKCAnim.SetBool("IsDead", true);
		}
		#endregion

		float ang = Mathf.Deg2Rad * transform.rotation.eulerAngles.y;

		// Update Forward/Back Movement Direction
		float xdirection = Mathf.Sin(ang);
		float zdirection = Mathf.Cos(ang);
		movement_direction = new Vector3(xdirection, 0.0f, zdirection);

		// Left shift speeds up character and effects rate at which Junko speeds up
		if (Input.GetKey(KeyCode.W))
		{
			JKCAnim.SetBool("Move", true);
			JKCAnim.SetBool("Run", true);

			if (Input.GetKey(KeyCode.LeftShift))
            {
				if (t < 2f)
				{
					t += 0.01f;
				}
				JKCController.Move(movement_direction * 2.5f * t * Time.deltaTime);
			}
			else
            {
				if (t < 2f)
				{
					t += 0.001f;
				}
				JKCController.Move(movement_direction * 1.5f * t * Time.deltaTime);
			}
		}
		else if (Input.GetKey(KeyCode.S))
		{
			JKCAnim.SetBool("Move", true);
			JKCAnim.SetBool("Walk", true);
			if (Input.GetKey(KeyCode.LeftShift))
			{
				if (t < 2f)
				{
					t += 0.01f;
				}
				JKCController.Move(movement_direction * -2f * t * Time.deltaTime);
			}
			else
			{
				if (t < 2f)
				{
					t += 0.001f;
				}
				JKCController.Move(movement_direction * -1f * t * Time.deltaTime);
			}
		}
		else
        {
			// Reset animations and speeding up
			t = 1;
			JKCAnim.SetBool("Move", false);
			JKCAnim.SetBool("Run", false);
			JKCAnim.SetBool("Grounded", false);
		}

		/* Change Direction towards left direction
		Can be used with forwards */
		if (Input.GetKey(KeyCode.A))
		{
			JKCAnim.SetBool("Move", true);
			transform.Rotate(0f, -100f * Time.deltaTime, 0f);
		}

		/* Change Direction towards right direction
		Can be used with forwards */
		else if (Input.GetKey(KeyCode.D))
		{
			JKCAnim.SetBool("Move", true);
			transform.Rotate(0f, 100f * Time.deltaTime, 0f);
		}


		if (!JKCController.isGrounded)
		{
			JKCController.Move(new Vector3(0f, -1f, 0f) * 9.81f * Time.deltaTime);
		}
	}

	bool CheckGrounded()
	{//Judge whether Junkochan is on the ground or not
		Ray ray = new Ray(this.transform.position + Vector3.up * 0.05f, Vector3.down * 1f);//Shoot ray at 0.05f upper from Junkochan's feet position to the ground with its length of 0.1f
		return Physics.Raycast(ray, 0.1f);//If the ray hit the ground, return true
	}

	public void ChordStrike()
	{
		ParticleSystem.Play();
		JKCAnim.SetBool("Guard", true); //Play "Sword_Guard" animation in "ActionLayer" in Animator
		Invoke(nameof(SetGuardFalse), 0.1f);
		Invoke(nameof(StopParticles), 0.5f);
	}


	void SetGuardFalse()
	{
		JKCAnim.SetBool("Guard", false);
	}

	void StopParticles()
	{
		ParticleSystem.Stop();
	}

	public void TakeDamage(float dmg)
	{
		Health -= dmg;
		JKCAnim.SetBool("IsDamaged", true);
	}

	public float GetVelocity()
	{
		return move_velocity;
	}
	public Vector3 GetMoveDirection()
	{
		return MoveDirection;
	}
}
