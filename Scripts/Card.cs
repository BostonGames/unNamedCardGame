using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
	[SerializeField] int value = 0;
	[SerializeField] int indexValue = 0;
	[SerializeField] int totalCardCount;

	private int cardOrderDrawn;

	public int GetValueOfCard()
	{
		Debug.Log(getDisplayText());
		return value;
 	}

	public void SetValue(int newValue)
	{
		value = newValue;
	}
	public void SetIndexValue(int newValue)
	{
		indexValue = newValue;
	}
	public void SetCardOrderDrawn(int newValue)
	{
		cardOrderDrawn = newValue;
	}



	public void SetSprite(Sprite newSprite)
	{
		gameObject.GetComponent<SpriteRenderer>().sprite = newSprite;
	}

	//for dealing a new hand
	public void ResetCard()
	{
		// flip card ocver
		Sprite back = Deck.instance.GetCardBack();
		gameObject.GetComponent<SpriteRenderer>().sprite = back;
		// set value to zero
		value = 0;
	}


	/**
	 * Returns the suit of the card
	 *
	 * Ex: Hearts
	 *
	 * @return String
	 */
	public string GetSuit()
	{
		string suitValue;

		if(indexValue < 13)
        {
			suitValue = "Clubs";

			CardGameManager.instance.numberofClubs--;

			return suitValue;

		}
		else if (indexValue >= 13 && indexValue < 26)
		{
			suitValue = "Diamonds";

			CardGameManager.instance.numberOfDiamonds--;


			return suitValue;

		}
		else if (indexValue >= 26 && indexValue < 39)
		{
			suitValue = "Hearts";
			CardGameManager.instance.numberOfHearts--;


			return suitValue;

		}
		else if (indexValue >= 39 && indexValue <= 52)
		{
			suitValue = "Spades";
			CardGameManager.instance.numberOfSpades--;


			return suitValue;

		}
        else
		{
			Debug.Log("suit unable to be determined, possible corrupt index value of card");
			return null;
		}


	}

	/**
	 * Returns the face value of the card
	 *
	 * Ex: King
	 * @return String
	 */
	public string getFaceValue()
	{
		string faceValue;

		if(indexValue == 0 || indexValue == 13 || indexValue == 26 || indexValue == 39)
        {
			faceValue = "Ace";
			CardGameManager.instance.numberOfAces--;
			CardGameManager.instance.numberOfOnes--;


			CardGameManager.instance.zenCount--;

			if (cardOrderDrawn == 2)
			{
				int cardNumber;

				cardNumber = 11;
				CardGameManager.instance.CalculateDealersChanceOfBusting(cardNumber);
			}


			return faceValue;

		}
		else if (indexValue == 1 || indexValue == 14 || indexValue == 27 || indexValue == 40)
		{
			faceValue = "2";
			CardGameManager.instance.numberOfTwos--;

			CardGameManager.instance.hiOptIICount++;

			CardGameManager.instance.zenCount++;

			if (cardOrderDrawn == 2)
			{
				int cardNumber;

				cardNumber = 2;
				CardGameManager.instance.CalculateDealersChanceOfBusting(cardNumber);
			}


			return faceValue;
		}
		else if (indexValue == 2 || indexValue == 15 || indexValue == 28 || indexValue == 41)
		{
			faceValue = "3";
			CardGameManager.instance.numberOfThrees--;

			CardGameManager.instance.hiOptIICount++;

			CardGameManager.instance.zenCount++;

			if (cardOrderDrawn == 2)
			{
				int cardNumber;

				cardNumber = 3;
				CardGameManager.instance.CalculateDealersChanceOfBusting(cardNumber);
			}



			return faceValue;
		}
		else if (indexValue == 3 || indexValue == 16 || indexValue == 29 || indexValue == 42)
		{
			faceValue = "4";
			CardGameManager.instance.numberOfFours--;

			CardGameManager.instance.hiOptIICount++;
			CardGameManager.instance.hiOptIICount++;

			CardGameManager.instance.zenCount++;
			CardGameManager.instance.zenCount++;

			if (cardOrderDrawn == 2)
			{
				int cardNumber;

				cardNumber = 4;
				CardGameManager.instance.CalculateDealersChanceOfBusting(cardNumber);
			}


			return faceValue;
		}
		else if (indexValue == 4 || indexValue == 17 || indexValue == 30 || indexValue == 43)
		{
			faceValue = "5";
			CardGameManager.instance.numberOfFives--;

			CardGameManager.instance.hiOptIICount++;
			CardGameManager.instance.hiOptIICount++;

			CardGameManager.instance.zenCount++;
			CardGameManager.instance.zenCount++;

			if (cardOrderDrawn == 2)
			{
				int cardNumber;

				cardNumber = 5;
				CardGameManager.instance.CalculateDealersChanceOfBusting(cardNumber);
			}


			return faceValue;
		}
		else if (indexValue == 5 || indexValue == 18 || indexValue == 31 || indexValue == 44)
		{
			faceValue = "6";
			CardGameManager.instance.numberOfSixes--;

			CardGameManager.instance.hiOptIICount++;

			CardGameManager.instance.zenCount++;
			CardGameManager.instance.zenCount++;

			if (cardOrderDrawn == 2)
			{
				int cardNumber;

				cardNumber = 6;
				CardGameManager.instance.CalculateDealersChanceOfBusting(cardNumber);
			}


			return faceValue;
		}
		else if (indexValue == 6 || indexValue == 19 || indexValue == 32 || indexValue == 45)
		{
			faceValue = "7";
			CardGameManager.instance.numberOfSevens--;

			CardGameManager.instance.hiOptIICount++;

			CardGameManager.instance.zenCount++;

			if (cardOrderDrawn == 2)
			{
				int cardNumber;

				cardNumber = 7;
				CardGameManager.instance.CalculateDealersChanceOfBusting(cardNumber);
			}


			return faceValue;
		}
		else if (indexValue == 7 || indexValue == 20 || indexValue == 33 || indexValue == 46)
		{
			faceValue = "8";
			CardGameManager.instance.numberOfEights--;

			if (cardOrderDrawn == 2)
			{
				int cardNumber;

				cardNumber = 8;
				CardGameManager.instance.CalculateDealersChanceOfBusting(cardNumber);
			}


			return faceValue;
		}
		else if (indexValue == 8 || indexValue == 21 || indexValue == 34 || indexValue == 47)
		{
			faceValue = "9";
			CardGameManager.instance.numberOfNines--;

			if (cardOrderDrawn == 2)
			{
				int cardNumber;

				cardNumber = 9;
				CardGameManager.instance.CalculateDealersChanceOfBusting(cardNumber);
			}


			return faceValue;
		}
		else if (indexValue == 9 || indexValue == 22 || indexValue == 35 || indexValue == 48)
		{
			faceValue = "10";
			CardGameManager.instance.numberOfTens--;

			CardGameManager.instance.hiOptIICount--;
			CardGameManager.instance.hiOptIICount--;

			CardGameManager.instance.zenCount--;
			CardGameManager.instance.zenCount--;

			if (cardOrderDrawn == 2)
			{
				int cardNumber;

				cardNumber = 10;
				CardGameManager.instance.CalculateDealersChanceOfBusting(cardNumber);
			}


			return faceValue;
		}
		else if (indexValue == 10 || indexValue == 23 || indexValue == 36 || indexValue == 49)
		{
			faceValue = "Jack";
			CardGameManager.instance.numberOfJacks--;

			CardGameManager.instance.hiOptIICount--;
			CardGameManager.instance.hiOptIICount--;

			CardGameManager.instance.zenCount--;
			CardGameManager.instance.zenCount--;

			if (cardOrderDrawn == 2)
			{
				int cardNumber;

				cardNumber = 10;
				CardGameManager.instance.CalculateDealersChanceOfBusting(cardNumber);
			}


			return faceValue;
		}
		else if (indexValue == 11 || indexValue == 24 || indexValue == 37 || indexValue == 50)
		{
			faceValue = "Queen";
			CardGameManager.instance.numberOfQueens--;

			CardGameManager.instance.hiOptIICount--;
			CardGameManager.instance.hiOptIICount--;

			CardGameManager.instance.zenCount--;
			CardGameManager.instance.zenCount--;

			if (cardOrderDrawn == 2)
			{
				int cardNumber;

				cardNumber = 10;
				CardGameManager.instance.CalculateDealersChanceOfBusting(cardNumber);
			}


			return faceValue;
		}
		else if (indexValue == 12 || indexValue == 25 || indexValue == 38 || indexValue == 51)
		{
			faceValue = "King";
			CardGameManager.instance.numberOfKings--;

			CardGameManager.instance.hiOptIICount--;
			CardGameManager.instance.hiOptIICount--;

			CardGameManager.instance.zenCount--;
			CardGameManager.instance.zenCount--;

			if (cardOrderDrawn == 2)
			{
				int cardNumber;

				cardNumber = 10;
				CardGameManager.instance.CalculateDealersChanceOfBusting(cardNumber);
			}


			return faceValue;
		}
		else
		{
			Debug.Log("faceValue unable to be determined, possible corrupt index value of card");
			return null;
		}


	}


	/**
	 * Returns the human readable string representation of the card.
	 *
	 * Ex: King of Hearts
	 *
	 * @return String
	 */
	public string getDisplayText()
	{
		string cardValue;

		cardValue = getFaceValue() + " of " + GetSuit() + " , card index value: " + indexValue + ", card order value: " + cardOrderDrawn;

		return cardValue;
	}

}
