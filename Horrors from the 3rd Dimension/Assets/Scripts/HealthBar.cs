using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour {
    public RectTransform dimension;
    public float maxLength;
    // Use this for initialization
    void Start () {
        dimension = (RectTransform)GetComponent<RectTransform>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void UpdateHealth()
    {
        float maxHealth = GetComponentInParent<PlayerController>().maxHealth;
        float health = GetComponentInParent<PlayerController>().health;
        if (health < 0)
        {
            health = 0;
        }
        dimension.localScale = new Vector3(maxLength * health / maxHealth, dimension.localScale.y, dimension.localScale.z);
        //dimension.localPosition = new Vector3(-50 * dimension.localScale.x, dimension.localPosition.y, dimension.localPosition.z);
    }
}

/**
 * Once upon a time there lived a pinapple named Georgia, King of Potatoland. Georgia was a creul and greetiy king, terrifying and terrified of his subjects Georgia was mad with power, and dreamed of becoming immortal via the rite of the eleven elven pillars of wise wisdom of grapefruit mango tea. He heasrd of this ritual from his monkey firend Simbaku. Simbaku did not approve of Georgia's evil ways, nor of his cocaine habit, but still he supported him in his quest for everlasting life. The question was, how were they to find the pillars of elvish wize grapefruit wisdom? Why, by consulting the wisest Wizards in this whistful land, of course!. But who was the wisest? Georgia, as it turned out! But wait, how could Georgia ask himself of things he did not know? The solution was simple: Build an elaborate contraption that would allow himn to speak to himself. THe contraption involved a Golden Cornecopia, a time-traveling minx and the blood of ten-thousand darkly, dankly, seadly, blackly, diseasedly, terrifyingly dead souly souls. When the ritual was completed and the contraption built, Georgia spoke, and asked himself a question: "Oh, great and wonderful me, where are the pillars of the red velvet rose elves for the vivification of grapefruit lemons that will allow me to attain my ascention? Good question. I don't know. I only know what you know. Are you sure? Yes. Are you really sure? Yes. Are you really, really sure? No, actually. I donkn't knpow whether or not I doin't know it. I may in fact know. You know who knows if I know? Who? You! Me? Yes, Me! Do I know the answer? Yes, of course I do! You shall not be dissapointed! The pillars can be found in the darkest shadow of the night, at the end of the circular road, in the middle of the enless lake! Thank you, Georgia, you have breewn very helpful. I am eternally greatful!. Good, good, Good luck!" And so Georgia and his friend set off to find the impossible placer, and quite im possibly they found it within two days. Yhe only problem was that it wasn't real. None of them were real. Once this was learned, they knew eternity would be wewternally short. Oh, well. Thatsessssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssslife!
 */
