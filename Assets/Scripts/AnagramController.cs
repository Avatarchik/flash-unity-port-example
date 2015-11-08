using UnityEngine;
using System.Collections.Generic;

public class AnagramController : MonoBehaviour {
	private AnagramModel model;

	public void Start() {
		model = new AnagramModel();
		model.Start();
	}

	public void Update() {
		model.Update(Time.deltaTime);
	}
}
