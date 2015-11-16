using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Main : MonoBehaviour {
	public AudioClip selectSound;
	public AudioClip shootSound;

	private Model model;
	private View view;

	public void Start() {
		model = new Model();
		view = new View(model, this);
	}

	public void Update() {
		model.Update(Time.deltaTime);
		view.Update();
	}
}
