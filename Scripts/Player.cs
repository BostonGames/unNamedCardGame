using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // -- this script is for BOTH Player and Dealer --- \\

    // get other scripts
    public Card cardScript;
    public Deck deckScript;


    // Total value of this Players hand
    public int handValue = 0;

    public GameObject[] hand;
    public GameObject doubleCard;
    public GameObject[] potChips;

    public int cardIndex = 0;
    public int chipIndex = 0;


    //tracking aces of 1 or 11 conversions
    List<Card> aceList = new List<Card>();

    public int points = 100;
    public int pointsThisRound = 0;


    // Start is called before the first frame update
    public void StartHand()
    {
        GetCard();
        GetCard();
    }


    public void ChooseName(string name)
    {
        //Create a player given a name
       // *@param name
    }

    /** GETCARD
     * Adds a card to the players hand.
     * @param card
    */
    public int GetCard()
    {

        // Get a card, use deal card to assign sprite and value to card on table
        int cardValue = deckScript.DealCard(hand[cardIndex].GetComponent<Card>());
        // Show card on game screen
        hand[cardIndex].GetComponent<Renderer>().enabled = true;
        // Add card value to running total of the hand
        handValue += cardValue;
        // If value is 1, it is an ace
        if (cardValue == 1)
        {
            aceList.Add(hand[cardIndex].GetComponent<Card>());

            // this corrects Aces being counted twice
            CardGameManager.instance.numberOfAces++;

        }
        // Check if we should use an 11 instead of a 1
        AceCheck();

        CardGameManager.instance.numberOfDraws = CardGameManager.instance.numberOfDraws++;

        cardIndex++;
        return handValue;

    }

    // if player hits Double
    public int GetDoubleCard()
    {
        // Get a card, use deal card to assign sprite and value to card on table
        int cardValue = deckScript.DealCard(doubleCard.GetComponent<Card>());
        // Show card on game screen
        doubleCard.GetComponent<Renderer>().enabled = true;
        // Add card value to running total of the hand
        handValue += cardValue;
        // If value is 1, it is an ace
        if (cardValue == 1)
        {
            aceList.Add(hand[cardIndex].GetComponent<Card>());

            // this corrects Aces being counted twice
            CardGameManager.instance.numberOfAces++;

        }
        // Check if we should use an 11 instead of a 1
        AceCheck();

        CardGameManager.instance.numberOfDraws = CardGameManager.instance.numberOfDraws++;

        cardIndex++;
        return handValue;

    }
    public void DoubleBets()
    {
        int i;

        for (i = 0; i < chipIndex; i++)
        {
            if(points >= 20)
            {
                potChips[chipIndex].GetComponent<Renderer>().enabled = true;
                CardGameManager.instance.AdjustPot(CardGameManager.instance.minBet);
                AdjustPoints(-CardGameManager.instance.minBet);

            }
            else
            {
                CardGameManager.instance.mainText.text = "You have gone all in for this Double resulting in less than 2x your original bets.";
            }
        }


    }

    public void AddChipToPot()
    {
        potChips[chipIndex].GetComponent<Renderer>().enabled = true;
        chipIndex++;
    }
    public void SubtractChipFromPot()
    {
        potChips[chipIndex - 1].GetComponent<Renderer>().enabled = false;
        chipIndex--;
    }


    // calculate if Aces should be counted as 1s or 11
    public void AceCheck()
    {
        // for each ace in the list, check these metrics:
        foreach(Card ace in aceList)
        {
            // will we bust 21 if ace is counted as an 11?
            if(handValue + 10 < 22 && ace.GetValueOfCard() == 1)
            {
                ace.SetValue(11);
                handValue += 10;
            }else if (handValue > 21 && ace.GetValueOfCard() == 11)
            {
                ace.SetValue(1);
                handValue -= 10;
            }
        }
    }

    // add or subtract from points for bets
    public void AdjustPoints(int amount)
    {
        points += amount;
    }
    public void AdjustPointsThisRound(int amount)
    {
        pointsThisRound += amount;
    }
    public void ResetPointsThisRound()
    {
        pointsThisRound = 0;
    }

    // get players current point amount
    public int GetPoints()
    {
        return points;
    }


    public void ResetHand()
    {
        // loop through cards on table and reset to default state
        for(int i = 0; i < hand.Length; i++)
        {
            hand[i].GetComponent<Card>().ResetCard();
            hand[i].GetComponent<Renderer>().enabled = false;
        }


        cardIndex = 0;
        handValue = 0;
        aceList = new List<Card>();

        doubleCard.GetComponent<Renderer>().enabled = false;
        CardGameManager.instance.hideCardGameObject.SetActive(false);
    }
    public void ResetChips()
    {
        for (int j = 0; j < potChips.Length; j++)
        {
            potChips[j].GetComponent<Renderer>().enabled = false;
        }
        chipIndex = 0;
    }

}
