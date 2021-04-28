using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour
{
	public static Deck instance;
	public Sprite[] cardSprites;
	int[] cardValues = new int[53];
	int[] cardIndexValues = new int[52];
	private int currentIndex = 0;

	void Awake()
    {
		instance = this;
    }

    private void Start()
    {
		GetCardValues();
		GetIndexCardValues();
	}

	void GetCardValues()
	{
		int num = 0;
		// Loop to assign values to the cards
		for (int i = 0; i < cardSprites.Length; i++)
		{
			// so value isn't -1 what it's supposed to be
			num = i + 1;
			// Count up to the amout of cards, 52
			// then divide by 13 to keep values of each suit correct
			num %= 13;
			// if there is a remainder after x/13, then remainder
			// is used as the value, unless over 10, then use 10
			if (num > 10 || num == 0)
			{
				num = 10;
			}
			cardValues[i] = num++;
		}
	}
	void GetIndexCardValues()
	{
		int num = 0;
		// Loop to assign values to the cards
		for (int i = 0; i < cardSprites.Length; i++)
		{
			// so value isn't -1 what it's supposed to be
			num = i;

			cardIndexValues[i] = num++;
		}
	}


	public void Shuffle()
    {
		// std array swapping
		for(int i = 0; i < cardSprites.Length; i++)
        {
			int j = Mathf.FloorToInt(Random.Range(0.0f, 1.0f) * cardSprites.Length);
			Sprite face = cardSprites[i];
			cardSprites[i] = cardSprites[j];
			cardSprites[j] = face;

			int value = cardValues[i];
			cardValues[i] = cardValues[j];
			cardValues[j] = value;

			int indexValue = cardIndexValues[i];
			cardIndexValues[i] = cardIndexValues[j];
			cardIndexValues[j] = indexValue;
		}
		currentIndex = 1;

		Debug.Log("Deck has been shuffled");
	}

	/**
	 * Adds the given card from the deck.
	 * @return void
	 */
	public int DealCard(Card cardScript)
	{
		cardScript.SetSprite(cardSprites[currentIndex]);
		//increase value of index by one after getting the value
		cardScript.SetValue(cardValues[currentIndex]);
		cardScript.SetIndexValue(cardIndexValues[currentIndex]);
		cardScript.SetCardOrderDrawn(CardGameManager.instance.numberOfDraws);

		currentIndex++;
		CardGameManager.instance.numberOfCards--;
		CardGameManager.instance.numberOfDraws++;
		CardGameManager.instance.CalculateProbabilities();

		return cardScript.GetValueOfCard();
	}
	
	public Sprite GetCardBack()
	{
		return cardSprites[0];
	}

	/**
	 * Returns the number of cards in the deck.
	 * @return int
	 */
	public int size()
	{
		int size;

		size = CardGameManager.instance.numberOfCards;

		return size;
	}


	//TODO put this functionality in:
	/**
	 * "Peeks" the top card in the deck without removing the card from the deck.
	 * @return Card
	 
	public Card peekCard()
	{

	}*/

	/**
	 * Returns the cards in the deck.
	 * @return Stack<Card>
	
	public Stack<Card> getCards()
	{

	} */
}
