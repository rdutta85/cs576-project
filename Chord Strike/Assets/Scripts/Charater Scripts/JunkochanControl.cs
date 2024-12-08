﻿using OpenCover.Framework.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 
 For Tele: Implement any additional items here.
 
 */


public class JunkochanControl : MonoBehaviour {
	private float InputH;//Horizontal Input (A & D key)
	private float InputV;//Vertical Input (W & S key)
	private GameObject JKCCam;//Camera which chases Junkochan
	private CharacterController JKCController;//Character controller component attached to Junkochan
	private Animator JKCAnim;//Animator component attached to Junkochan
	private float MoveSpeed;//Horizontal move speed
	private float Height;//Current height of Junkochan (y value of transform.position)
	public ParticleSystem ParticleSystem;
	public float Health = 3;
	public float VertSpeed = 0;
	private Vector3 LastMoveDirection;

    // Use this for initialization
    void Start () {
		JKCCam = GameObject.Find("Main Camera");
		JKCController = this.GetComponent<CharacterController>();
		JKCAnim = this.GetComponent<Animator>();
        JKCAnim.SetBool("Jump", false);
        JKCAnim.SetBool("Grounded", true);
        JKCAnim.SetBool("Fall", false);
    }

    // Update is called once per frame
    void Update() {

#region ACTION		
		if (JKCAnim.GetCurrentAnimatorStateInfo(1).IsName("Sword_Iai") || JKCAnim.GetCurrentAnimatorStateInfo(1).IsName("Sword_Guard") || JKCAnim.GetCurrentAnimatorStateInfo(1).IsName("Sword_Store")) {
			return;//While the attacking / guarding animation is playing, do not go to below "MOVEMENT" process  
		}

        JKCAnim.SetBool("IsDamaged", false);

        if (Health == 0)
		{
			JKCAnim.SetBool("IsDead", true);
		}
#endregion


#region MOVEMENT
		InputH = Input.GetAxis("Horizontal");//Get keyboard input
		InputV = Input.GetAxis("Vertical");//Get keyboard input
		Vector3 CamForward = Vector3.Scale(JKCCam.transform.forward, new Vector3(1, 0, 1)).normalized;//Camera's forward direction
		Vector3 MoveDirection = CamForward * InputV + JKCCam.transform.right * InputH;//Get Junkochan's forward direction seen from camera

		if (MoveDirection.magnitude > 0) {//When any WASD key is pushed
			transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(MoveDirection), 0.05f);//Rotate Junkochan to Inputting direction
		}

		if (MoveDirection.magnitude > 0) {//When any WASD key is pushed
			MoveSpeed = 2f;//Set Junkochan's moving speed as walking speed
			JKCAnim.SetBool("Move", true);//Set Junkochan's "Move" parameter in Animator component
		} else {
			JKCAnim.SetBool("Move", false);
		}

		if (Input.GetKey(KeyCode.LeftShift)) {//When shift key is pushed
			MoveSpeed *= 2f;//Set Junkochan's moving speed as Running speed
			JKCAnim.SetBool("Run", true);//Set Junkochan's "Run" parameter in Animator component
		} else {
			JKCAnim.SetBool("Run", false);
		}
		if (Input.GetKey(KeyCode.LeftControl)) {//When Control key is pushed
			MoveDirection *= 0.4f;//Set Junkochan's moving speed as crouching walk speed
			JKCAnim.SetBool("Crouch", true);//Set Junkochan's "Crouch" parameter in Animator component
		} else {
			JKCAnim.SetBool("Crouch", false);
		}

		Height = transform.position.y;//Memory current Junkochan's height

		//JunckoChan Movement
		if(!CheckGrounded())VertSpeed -= 0.2f;//Increase falling speed (worked as gravity acceleration)
		JKCController.Move(MoveDirection*MoveSpeed*Time.deltaTime);//Horizontal movement
		JKCController.Move(Vector3.up*VertSpeed*Time.deltaTime);//Vertical movement

		LastMoveDirection = MoveDirection;

#endregion
	}

	bool CheckGrounded() {//Judge whether Junkochan is on the ground or not
		Ray ray = new Ray(this.transform.position+Vector3.up*0.05f,Vector3.down*1f);//Shoot ray at 0.05f upper from Junkochan's feet position to the ground with its length of 0.1f
		return Physics.Raycast(ray, 0.1f);//If the ray hit the ground, return true
    }
	
	public void ChordStrike()
	{
		ParticleSystem.Play();
        JKCAnim.SetBool("Guard", true); //Play "Sword_Guard" animation in "ActionLayer" in Animator
        Invoke(nameof(SetGuardFalse), 0.1f);
        Invoke(nameof(StopParticles), 0.5f);
    }

	public Vector3 GetLastMoveDirection(){
		return LastMoveDirection;
	}

	void SetGuardFalse()
	{
		JKCAnim.SetBool("Guard", false);
	}

	void StopParticles()
	{
		ParticleSystem.Stop();
	}

	public void Damage()
	{
		Health -= 1;
        JKCAnim.SetBool("IsDamaged", true);
    }
}
