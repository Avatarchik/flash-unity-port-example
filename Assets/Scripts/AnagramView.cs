using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using com.finegamedesign.anagram;

public class AnagramView {
	private AudioSource audio;
	private Model model;
	private Main main;

	public AnagramView(Model theModel, Main theMainScene) {
		model = theModel;
		main = theMainScene;
		audio = main.gameObject.GetComponent<AudioSource>();
	}

	private string letterMouseDown;

	/**
	 * Remember which letter was just clicked on this update.
	 *
	 * http://answers.unity3d.com/questions/20328/onmousedown-to-return-object-name.html
	 */
	private void updateMouseDown()
	{
		letterMouseDown = null;
		if (Input.GetMouseButtonDown(0)) {
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit)) {
				letterMouseDown = hit.transform.Find("text3d").GetComponent<TextMesh>().text.ToLower();
				// Debug.Log("View.updateMouseDown: " + letterMouseDown);
			}
		}
	}

	private bool IsLetterMouseDown(string letter)
	{
		return letter == letterMouseDown;
	}

	public bool IsLetterKeyDown(string letter)
	{
		return Input.GetKeyDown(letter.ToLower())
			|| IsLetterMouseDown(letter.ToLower());
	}

	public void update() {
		updateCheat();
		updateBackspace();
		updateMouseDown();
		List<string> presses = model.getPresses(IsLetterKeyDown);
		updateSelect(model.press(presses), true);
		updateSubmit();
		updatePosition();
		updateLetters(main.transform.Find("word/state").gameObject, model.word, "bone_{0}/letter");
		updateLetters(main.transform.Find("input/state").gameObject, model.inputs, "Letter_{0}");
		updateLetters(main.transform.Find("input/output").gameObject, model.outputs, "Letter_{0}");
		updateHud();
	}

	/**
	 * http://answers.unity3d.com/questions/762073/c-list-of-string-name-for-inputgetkeystring-name.html
	 */
	private void updateCheat()
	{
		if (Input.GetKeyDown("page up"))
		{
			model.cheatLevelUp(1);
			audio.PlayOneShot(main.selectSound);
		}
		else if (Input.GetKeyDown("page down"))
		{
			model.cheatLevelUp(-1);
			audio.PlayOneShot(main.selectSound);
		}
	}

        /**
         * Delete or backspace:  Remove last letter.
         */
        private void updateBackspace()
        {
            if (Input.GetKeyDown("delete")
            || Input.GetKeyDown("backspace")
	    || "delete" == letterMouseDown)
            {
                updateSelect(model.backspace(), false);
		letterMouseDown = null;
            }
        }

        /**
         * Each selected letter in word plays animation "selected".
Select, submit: Anders sees reticle and sword. Test case:  2015-04-18 Anders sees word is a weapon.
         */
        private void updateSelect(List<int> selects, bool selected)
        {
            GameObject parent = main.transform.Find("word/state").gameObject;
	string state = selected ? "selected" : "none";
            for (int s = 0; s < selects.Count; s++)
            {
                int index = (int) selects[s];
                string name = "bone_" + index + "/letter";
		ViewUtil.SetState(parent.transform.Find(name).gameObject, state);
                audio.PlayOneShot(main.selectSound);
            }
        }

	private void updateHud()
	{
		Text help = main.transform.Find("hud/help").GetComponent<Text>();
		help.text = model.help.ToString();
		Text score = main.transform.Find("hud/score").GetComponent<Text>();
		score.text = model.score.ToString();
		Text level = main.transform.Find("hud/level").GetComponent<Text>();
		level.text = model.levels.current().ToString();
		Text levelMax = main.transform.Find("hud/levelMax").GetComponent<Text>();
		levelMax.text = model.levels.count().ToString();
	}


        /**
         * Press space or enter.  Input word.
         * Word robot approaches.
         *     restructure synchronized animations:
         *         word
         *             complete
         *             state
         *         input
         *             output
         *             state
         */
        private void updateSubmit()
        {
            if (Input.GetKeyDown("space")
            || Input.GetKeyDown("return")
	    || "submit" == letterMouseDown)
            {
	    	letterMouseDown = null;
                string state = model.submit();
                if (null != state) 
                {
                    // TODO main.word.gotoAndPlay(state);
		    ViewUtil.SetState(main.transform.Find("input").gameObject, state);
		    audio.PlayOneShot(main.shootSound);
                }
                resetSelect();
            }
        }

        private void resetSelect()
        {
            GameObject parent = main.transform.Find("word").gameObject.transform.Find("state").gameObject;
            int max = model.letterMax;
            for (int index = 0; index < max; index++)
            {
                string name = "bone_" + index + "/letter";
		ViewUtil.SetState(parent.transform.Find(name).gameObject, "none");
            }
        }

	/**
	 * Unity prohibits editing a property of position.
	 */
	private void setPositionY(Transform transform, float y)
	{
		Vector3 position = transform.position;
		position.y = y;
		transform.position = position;
	}

	/**
	 * Unity prohibits editing a property of position.
	 */
	private void updatePosition()
	{
		if (model.mayKnockback())
		{
			updateOutputHitsWord();
		}
		setPositionY(main.transform.Find("word"), model.wordPositionScaled);
	}

	private void updateOutputHitsWord()
	{
		GameObject target = main.transform.Find("word/state").gameObject;
                if (model.onOutputHitsWord())
		{
			ViewUtil.SetState(target, "hit");

			audio.PlayOneShot(main.explosionBigSound);
		}
	}

	/**
	 * Find could be cached.
	 * http://gamedev.stackexchange.com/questions/15601/find-all-game-objects-with-an-input-string-name-not-tag/15617#15617
	 */
        private void updateLetters(GameObject parent, List<string> letters, string namePattern) {
		int max = model.letterMax;
		for (int i = 0; i < max; i++)
		{
			string name = namePattern.Replace("{0}", i.ToString());
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
