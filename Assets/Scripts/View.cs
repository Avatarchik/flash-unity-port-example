using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class View {
	private Model model;
	private GameObject main;

	public View(Model theModel, GameObject theMainScene) {
		model = theModel;
		main = theMainScene;
	}

	public bool IsLetterKeyDown(string letter)
	{
		return Input.GetKeyDown(letter.ToLower());
	}

	public void Update() {
		ArrayList presses = model.getPresses(IsLetterKeyDown);
		model.press(presses);
		updateLetters(main.transform.Find("word").gameObject.transform.Find("state").gameObject, model.word);
		updateLetters(main.transform.Find("input").gameObject.transform.Find("state").gameObject, model.inputs);
	}

	/**
	 * Find could be cached.
	 * http://gamedev.stackexchange.com/questions/15601/find-all-game-objects-with-an-input-string-name-not-tag/15617#15617
	 */
        private void updateLetters(GameObject parent, ArrayList letters) {
		int max = model.letterMax;
		for (int i = 0; i < max; i++)
		{
			string name = "Letter_" + i;
			GameObject letter = parent.transform.Find(name).gameObject;
			if (null != letter)
			{
				bool visible = i < letters.Count;
				letter.SetActive(visible);
				if (visible) {
					GameObject text3d = letter.transform.Find("text3d").gameObject;
					TextMesh mesh = text3d.GetComponent<TextMesh>();
					mesh.text = (string) letters[i];
				}

			}
		}
	}
}
