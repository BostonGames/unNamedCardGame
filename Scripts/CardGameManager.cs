using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardGameManager : MonoBehaviour
{
    public static CardGameManager instance;
    public Player playerScript;
    public Player dealerScript;

    [SerializeField] List<int> suitNumber;

    //for Testing:
    [SerializeField] float rectWidth = 120f;
    [SerializeField] float rectHeight = 40f;
    [SerializeField] GUIStyle guiStyle0;

    public Button dealButton;
    public Button hitButton;
    public Button betButton;
    public Button subtractBetButton;
    public Button standButton;
    public Button startButton;
    public Button doubleButton;
    public Button statsButton;
    public Button closeStatsButton;

    public Text standButtonText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI dealerScoreText;
    public TextMeshProUGUI potAmountText;
    public TextMeshProUGUI pointsText; //cashText
    public TextMeshProUGUI mainText; //cashText

    // In-Game Messages
    public GameObject youWin;
    public GameObject youLose;
    public GameObject doubleBust;
    public GameObject gameOver;
    public GameObject push;
    public GameObject notEnoughChips;
    public GameObject outOfChips;
    public GameObject placeYourBets;
    // Game Panels
    public GameObject gameOverPanel;
    public GameObject statsPanel;
    // card hiding dealers 2nd card
    public GameObject hideCardGameObject;

    // how much is bet
    private int pot = 0;
    public int maxBet = 500;
    public int minBet = 20;

    private int standClicks = 0;
    private int handsPlayed = 0;
    private bool roundOver;


    // ----- CARD COUNTING AND PROBABILITY STATS ----- \\
    public TextMeshProUGUI pDealerBustText;
    public TextMeshProUGUI p21NextHitText;
    public TextMeshProUGUI zenCountText;
    public TextMeshProUGUI zenHintText;
    public TextMeshProUGUI hiOptIICountText;
    public TextMeshProUGUI hiOptIIHintText;

    public int numberOfDraws;
    public int numberOfHearts, numberofClubs, numberOfDiamonds, numberOfSpades;
    public int numberOfCards;

    public int numberOfOnes;
    public int numberOfTwos;
    public int numberOfThrees;
    public int numberOfFours;
    public int numberOfFives;
    public int numberOfSixes;
    public int numberOfSevens;
    public int numberOfEights;
    public int numberOfNines;
    public int numberOfTens;
    public int numberOfKings;
    public int numberOfQueens;
    public int numberOfJacks;
    public int numberOfAces;

    private float pHeart, pClub, pDiamond, pSpade;
    private float p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, pJ, pQ, pK, pA;
    private float pOfWinning;


    public int hiOptIICount = 0;
    public int zenCount = 0;
    public float dealerBustChance = 0.0f;
    public float chanceOfGetting21Next = 0.0f;



    // ---- AUDIO ---- \\
    private AudioSource playerAudio;

    [SerializeField] AudioClip winSound;
    [SerializeField] AudioClip loseRoundSound;
    [SerializeField] AudioClip dealCardSound;
    [SerializeField] AudioClip warningSound;



    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            //since this means there is already a game manager assigned as "instance"
            Destroy(this.gameObject);
        }
    }

    void Start()
    {
        playerAudio = GetComponent<AudioSource>();
        handsPlayed = 0;

        //for testing
        StartNewGame();
        CalculateProbabilities();

        playerScript.ResetChips();
        dealerScript.ResetChips();

        //add listeners to the buttons:
        dealButton.onClick.AddListener(() => DealClicked());
        hitButton.onClick.AddListener(() => HitClicked());
        betButton.onClick.AddListener(() => BetClicked());
        subtractBetButton.onClick.AddListener(() => SubtractBetButtonClicked());
        standButton.onClick.AddListener(() => StandClicked());
        startButton.onClick.AddListener(() => StartButtonClicked());
        doubleButton.onClick.AddListener(() => DoubleButtonClicked());
        statsButton.onClick.AddListener(() => StatsButtonClicked());
        closeStatsButton.onClick.AddListener(() => CloseStatsButtonClicked());

        SetMainText("Press Start to begin");
        pointsText.text = "Points: " + playerScript.GetPoints().ToString();
    }

    public void StartButtonClicked()
    {
        StartNewGame();
        HideAllInGameMessages();

        dealButton.gameObject.SetActive(true);
        subtractBetButton.gameObject.SetActive(true);
        betButton.gameObject.SetActive(true);
        placeYourBets.SetActive(true);

        doubleButton.gameObject.SetActive(false);
        startButton.gameObject.SetActive(false);

        // minimum bet to start game
        playerScript.AddChipToPot();
        dealerScript.AddChipToPot();
        playerScript.AdjustPoints(-minBet);
        playerScript.AdjustPointsThisRound(+minBet);

        pot += minBet * 2;

        potAmountText.text = "Pot: " + pot.ToString();
        pointsText.text = "Points: " + playerScript.GetPoints().ToString();
        mainText.text = "Place your bet and hit deal when you are ready";
    }

    public void StatsButtonClicked()
    {
        statsPanel.SetActive(true);
        statsButton.gameObject.SetActive(false);
    }
    public void CloseStatsButtonClicked()
    {
        statsPanel.SetActive(false);
        statsButton.gameObject.SetActive(true);

    }

    public void DealClicked()
    {
        playerAudio.PlayOneShot(dealCardSound);

        hideCardGameObject.SetActive(true);

        dealerScoreText.gameObject.SetActive(false);
        placeYourBets.SetActive(false);

        Deck.instance.Shuffle();
        playerScript.StartHand();
        dealerScript.StartHand();

        // update scoreboard values
        scoreText.text = "Hand: " + playerScript.handValue.ToString();
        dealerScoreText.text = "Dealer Hand: " + dealerScript.handValue.ToString();

        // adjust button and UI component visibility
        subtractBetButton.gameObject.SetActive(false);
        betButton.gameObject.SetActive(false);
        startButton.gameObject.SetActive(false);
        dealButton.gameObject.SetActive(false);

        doubleButton.gameObject.SetActive(true);
        hitButton.gameObject.SetActive(true);
        standButton.gameObject.SetActive(true);
        standButtonText.text = "Stand";

        HideAllInGameMessages();
        SetMainText("");
    }

    public void HitClicked()
    {
        playerAudio.PlayOneShot(dealCardSound);

        doubleButton.gameObject.SetActive(false);
        HideAllInGameMessages();

        if (playerScript.cardIndex <= 10)
        {
            playerScript.GetCard();
            scoreText.text = "Hand: " + playerScript.handValue.ToString();
            if(playerScript.handValue > 20)
            {
                Invoke("RoundOver", 1.5f);
            }
        }
    }
    public void BetClicked()
    {
        // TODO multiple bet options expand
        //choose amount, if statements?

        int betAmount = minBet;

        if (playerScript.points >= minBet && playerScript.pointsThisRound < playerScript.potChips.Length * minBet)
        {
            playerScript.AdjustPoints(-betAmount);
            playerScript.AdjustPointsThisRound(+betAmount);

            playerScript.AddChipToPot();
            dealerScript.AddChipToPot();

            pot += betAmount * 2;
        }
        else if (playerScript.points < minBet)
        {
            notEnoughChips.SetActive(true);
            playerAudio.PlayOneShot(warningSound);

        }
        else if (playerScript.points <= 0)
        {
            outOfChips.SetActive(true);
            playerAudio.PlayOneShot(warningSound);
        }
        else
        {
            Debug.Log("Unable to process bet, check CardGameManager script");
        }

        if(playerScript.pointsThisRound == playerScript.potChips.Length*minBet)
        {
            SetMainText("You have bet the maximum amount this round");
            playerAudio.PlayOneShot(warningSound);
        }

        pointsText.text = "Points: " + playerScript.GetPoints().ToString();
        potAmountText.text = "Pot: " + pot.ToString();
    }
    public void SubtractBetButtonClicked()
    {
        notEnoughChips.SetActive(false);
        outOfChips.SetActive(false);

        if (playerScript.pointsThisRound >= minBet && pot > minBet*2)
        {
            playerScript.SubtractChipFromPot();
            dealerScript.SubtractChipFromPot();

            playerScript.AdjustPoints(minBet);
            playerScript.AdjustPointsThisRound(-minBet);

            pot -= minBet * 2;

            potAmountText.text = "Pot: " + pot.ToString();
            pointsText.text = "Points: " + playerScript.GetPoints().ToString();

            SetMainText("Place your bet and hit deal when you are ready");
        }
        else
        {
            mainText.text = "Cannot subtract more chips";
            return;
        }

    }

    public void DoubleButtonClicked()
    {
        playerAudio.PlayOneShot(dealCardSound);

        hitButton.gameObject.SetActive(false);
        doubleButton.gameObject.SetActive(false);
        standButton.gameObject.SetActive(false);

        playerScript.GetDoubleCard();
        playerScript.DoubleBets();
        dealerScript.DoubleBets();

        pointsText.text = "Points: " + playerScript.GetPoints().ToString();
        scoreText.text = "Hand: " + playerScript.handValue.ToString();

        HideAllInGameMessages();
        HitDealer();
        Invoke("RoundOver", 1.5f);


    }
    public void AdjustPot(int amount)
    {
        pot += amount;
        potAmountText.text = "Pot: " + pot.ToString();
    }
    public int GetPotAmount()
    {
        return pot;
    }

    public void StandClicked()
    {
        HideAllInGameMessages();
        doubleButton.gameObject.SetActive(false);


        standClicks++;
        if (standClicks >= 1) {
            HitDealer();
            Invoke("RoundOver", 1f);
        }
        else
        {
            HitDealer();
        }
        standButtonText.text = "Call";
    }
    private void HitDealer()
    {
        while(dealerScript.handValue < 16 && dealerScript.cardIndex < 11)
        {
            dealerScript.GetCard();
            dealerScoreText.text = "Dealer Hand: " + dealerScript.handValue.ToString();
            if (dealerScript.handValue > 20) Invoke("RoundOver", 1f);
            
        }
    }

    public void HideAllInGameMessages()
    {
        youLose.SetActive(false);
        youWin.SetActive(false);
        doubleBust.SetActive(false);
        gameOver.SetActive(false);
        push.SetActive(false);
        notEnoughChips.SetActive(false);
        outOfChips.SetActive(false);
        placeYourBets.SetActive(false);

    }
    public void SetMainText(string text)
    {
        mainText.text = text;
    }

    public void RoundOver()
    {
        dealButton.gameObject.SetActive(false);
        doubleButton.gameObject.SetActive(false);
        playerScript.ResetPointsThisRound();

        playerScript.ResetChips();
        dealerScript.ResetChips();

        // game states
        bool playerBust = playerScript.handValue > 21;
        bool dealerBust = dealerScript.handValue > 21;
        bool player21 = playerScript.handValue == 21;
        bool dealer21 = dealerScript.handValue == 21;

        // if stand has been clicked less than twice, no 21s or
        // busts, leave this function becuase games is not over
        if (standClicks < 2 && !playerBust && dealerBust && player21 && dealer21) return;
        bool roundOver = true;

        // double bust
        if (playerBust && dealerBust)
        {
            doubleBust.SetActive(true);
            mainText.text = "All Bust: Bets Returned";
            playerScript.AdjustPoints(pot / 2);
            playerAudio.PlayOneShot(warningSound);

        }
        else if (playerBust || !dealerBust && dealerScript.handValue > playerScript.handValue)
        {
            youLose.SetActive(true);
            mainText.text = "Dealer wins this round";
            playerAudio.PlayOneShot(loseRoundSound);

        }
        else if (playerScript.handValue == dealerScript.handValue)
        {
            push.SetActive(true);
            mainText.text = "Round has resulted in a Push; bets returned";
            playerScript.AdjustPoints(pot / 2);
            playerAudio.PlayOneShot(warningSound);

        }
        else if (dealerBust || playerScript.handValue > dealerScript.handValue)
        {
            youWin.SetActive(true);
            mainText.text = "You have won the round";
            playerScript.AdjustPoints(pot);
            playerAudio.PlayOneShot(winSound);

        }
        else
        {
            roundOver = false;
            Debug.Log("error occured in game manager script");
        }

        // set up UI for next round
        if (roundOver)
        {
            hitButton.gameObject.SetActive(false);
            standButton.gameObject.SetActive(false);
            betButton.gameObject.SetActive(false);
            hideCardGameObject.SetActive(false);
            dealButton.gameObject.SetActive(false);

            startButton.gameObject.SetActive(true);
            dealerScoreText.gameObject.SetActive(true);

            pointsText.text = "Points: " + playerScript.GetPoints().ToString();
            pot = 0;
            standClicks = 0;
        }

        if(playerScript.GetPoints() <= 0)
        {
            GameOver();
            playerAudio.PlayOneShot(loseRoundSound);
        }
    }

    public void GameOver()
    {
        gameOverPanel.SetActive(true);
        mainText.text = "You have run out of points";

        HideAllInGameMessages();
        hitButton.gameObject.SetActive(false);
        standButton.gameObject.SetActive(false);
        betButton.gameObject.SetActive(false);
        hideCardGameObject.SetActive(false);
        dealButton.gameObject.SetActive(false);
        startButton.gameObject.SetActive(false);
        dealerScoreText.gameObject.SetActive(false);
    }

    //for Testing
    private void OnGUI()
    {
        // ----- GAME TITLE ----- \\
        GUI.Box(new Rect((Screen.width / 2) - rectWidth, 10, rectWidth, rectHeight), "unNamed Card Game", guiStyle0);

        // ----- p of X ----- \\
        GUI.Box(new Rect(20, 960, rectWidth, rectHeight), "p of Winning: " + pOfWinning, guiStyle0);

        // ----- CARD METRICS ---- \\
        GUI.Box(new Rect(20, 100, rectWidth, rectHeight), "Number of Draws: " + numberOfDraws, guiStyle0);
        GUI.Box(new Rect(20, 140, rectWidth, rectHeight), "Number of Cards: " + numberOfCards, guiStyle0);

        GUI.Box(new Rect(20, 200, rectWidth, rectHeight), "Number of Spades: " + numberOfSpades, guiStyle0);
        GUI.Box(new Rect(20, 240, rectWidth, rectHeight), "Number of Hearts: " + numberOfHearts, guiStyle0);
        GUI.Box(new Rect(20, 280, rectWidth, rectHeight), "Number of Clubs: " + numberofClubs, guiStyle0);
        GUI.Box(new Rect(20, 320, rectWidth, rectHeight), "Number of Diamonds: " + numberOfDiamonds, guiStyle0);

        GUI.Box(new Rect(20, 380, rectWidth, rectHeight), "1s: " + numberOfOnes, guiStyle0);
        GUI.Box(new Rect(20, 420, rectWidth, rectHeight), "2s: " + numberOfTwos, guiStyle0);
        GUI.Box(new Rect(20, 460, rectWidth, rectHeight), "3s: " + numberOfThrees, guiStyle0);
        GUI.Box(new Rect(20, 500, rectWidth, rectHeight), "4s: " + numberOfFours, guiStyle0);
        GUI.Box(new Rect(20, 540, rectWidth, rectHeight), "5s: " + numberOfFives, guiStyle0);
        GUI.Box(new Rect(20, 580, rectWidth, rectHeight), "6s: " + numberOfSixes, guiStyle0);
        GUI.Box(new Rect(20, 620, rectWidth, rectHeight), "7s: " + numberOfSevens, guiStyle0);
        GUI.Box(new Rect(20, 660, rectWidth, rectHeight), "8s: " + numberOfEights, guiStyle0);
        GUI.Box(new Rect(20, 700, rectWidth, rectHeight), "9s: " + numberOfNines, guiStyle0);
        GUI.Box(new Rect(20, 740, rectWidth, rectHeight), "10s: " + numberOfTens, guiStyle0);
        GUI.Box(new Rect(20, 780, rectWidth, rectHeight), "Kings: " + numberOfKings, guiStyle0);
        GUI.Box(new Rect(20, 820, rectWidth, rectHeight), "Queens: " + numberOfQueens, guiStyle0);
        GUI.Box(new Rect(20, 860, rectWidth, rectHeight), "Jacks: " + numberOfJacks, guiStyle0);
        GUI.Box(new Rect(20, 900, rectWidth, rectHeight), "Aces: " + numberOfAces, guiStyle0);
                         
        GUI.Box(new Rect(420, 200, rectWidth, rectHeight), "p of Spades: " + (100*pSpade).ToString("F4") + "%", guiStyle0);
        GUI.Box(new Rect(420, 240, rectWidth, rectHeight), "p of Hearts: " + pHeart.ToString("F4") + "%", guiStyle0);
        GUI.Box(new Rect(420, 280, rectWidth, rectHeight), "p of Clubs: " + pClub.ToString("F4") + "%", guiStyle0);
        GUI.Box(new Rect(420, 320, rectWidth, rectHeight), "p of Diamonds: " + pDiamond.ToString("F4") + "%", guiStyle0);
                         
        GUI.Box(new Rect(420, 380, rectWidth, rectHeight), "p of 1s: " + (100*p1).ToString("F4") + "%", guiStyle0);
        GUI.Box(new Rect(420, 420, rectWidth, rectHeight), "p of 2s: " + (100*p2).ToString("F4") + "%", guiStyle0);
        GUI.Box(new Rect(420, 460, rectWidth, rectHeight), "p of 3s: " + (100*p3).ToString("F4") + "%", guiStyle0);
        GUI.Box(new Rect(420, 500, rectWidth, rectHeight), "p of 4s: " + (100*p4).ToString("F4") + "%", guiStyle0);
        GUI.Box(new Rect(420, 540, rectWidth, rectHeight), "p of 5s: " + (100*p5).ToString("F4") + "%", guiStyle0);
        GUI.Box(new Rect(420, 580, rectWidth, rectHeight), "p of 6s: " + (100*p6).ToString("F4") + "%", guiStyle0);
        GUI.Box(new Rect(420, 620, rectWidth, rectHeight), "p of 7s: " + (100*p7).ToString("F4") + "%", guiStyle0);
        GUI.Box(new Rect(420, 660, rectWidth, rectHeight), "p of 8s: " + (100*p8).ToString("F4") + "%", guiStyle0);
        GUI.Box(new Rect(420, 700, rectWidth, rectHeight), "p of 9s: " + (100 * p9).ToString("F4") + "%", guiStyle0);
        GUI.Box(new Rect(420, 740, rectWidth, rectHeight), "p of 10s: " + (100 * p10).ToString("F4") + "%", guiStyle0);
        GUI.Box(new Rect(420, 780, rectWidth, rectHeight), "p of Kings: " + (100 * pK).ToString("F4") + "%", guiStyle0);
        GUI.Box(new Rect(420, 820, rectWidth, rectHeight), "p of Queens: " + (100 * pQ).ToString("F4") + "%", guiStyle0);
        GUI.Box(new Rect(420, 860, rectWidth, rectHeight), "p of Jacks: " + (100 * pJ).ToString("F4") + "%", guiStyle0);
        GUI.Box(new Rect(420, 900, rectWidth, rectHeight), "p of Aces: " + (100 * pA).ToString("F4") + "%", guiStyle0);

        GUI.Box(new Rect(420, 100, rectWidth, rectHeight), "Hi Opt II Count: " + hiOptIICount, guiStyle0);
        GUI.Box(new Rect(420, 140, rectWidth, rectHeight), "Zen Count: " + zenCount, guiStyle0);

        GUI.Box(new Rect(820, 100, rectWidth, rectHeight), "Hi Opt II Bet Hint: " + HiOptIIHint(), guiStyle0);
        GUI.Box(new Rect(820, 140, rectWidth, rectHeight), "Zen Bet Hint: " + ZenHint(), guiStyle0);

        GUI.Box(new Rect(820, 200, rectWidth, rectHeight), "Dealer Bust Chance: " + dealerBustChance.ToString("F4") + "%", guiStyle0);
        GUI.Box(new Rect(820, 240, rectWidth, rectHeight), "p of 21 Next Hit: " + (100 * ChanceOfGetting21Next()).ToString("F4") + "%", guiStyle0);
    }

    public void StartNewGame()
    {
        ResetCardCounts();
        playerScript.ResetHand();
        dealerScript.ResetHand();

        CalculateProbabilities();

        hitButton.gameObject.SetActive(false);
        standButton.gameObject.SetActive(false);
        dealButton.gameObject.SetActive(false);
        subtractBetButton.gameObject.SetActive(false);
        betButton.gameObject.SetActive(false);

    }

    public void ResetCardCounts()
    {
        numberOfDraws = 0;
        numberOfCards = 52;

        numberOfSpades = 13;
        numberOfHearts = 13;
        numberofClubs = 13;
        numberOfDiamonds = 13;

        numberOfOnes = 4;
        numberOfTwos = 4;
        numberOfThrees = 4;
        numberOfFours = 4;
        numberOfFives = 4;
        numberOfSixes = 4;
        numberOfSevens = 4;
        numberOfEights = 4;
        numberOfNines = 4;
        numberOfTens = 4;
        numberOfKings = 4;
        numberOfQueens = 4;
        numberOfJacks = 4;
        numberOfAces = 4;

        hiOptIICount = 0;
        zenCount = 0;
        dealerBustChance = 0.0f;
    }

    private string HiOptIIHint()
    {
        string hint = "";

        if (hiOptIICount <= 0)
        {
            hint = "if hand is low bet table minimum";
            return hint;
        }
        else if (hiOptIICount > 0)
        {
            hint = "bet " + hiOptIICount + "x table minimum";
            return hint;
        }
        else { return null; }
    }

    private string ZenHint()
    {
        string hint = "";

        if (zenCount <= 0)
        {
            hint = "if hand is low bet table minimum";
            return hint;
        }
        else if (zenCount > 0)
        {
            hint = "bet " + zenCount + "x table minimum";
            return hint;
        }
        else { return null; }
    }


    string LikelihoodOfWinning()
    {
        string likelihood;


        //probablilty scale

        if (pOfWinning >= 0.8)
        {
            likelihood = "Highly Likely";
        }
        if (pOfWinning >= 0.6 && pOfWinning < 0.8)
        {
            likelihood = "Likely";
        }
        if (pOfWinning >= 0.5 && pOfWinning < 0.6)
        {
            likelihood = "More Likely Than Not";
        }
        if (pOfWinning >= 0.4 && pOfWinning < 0.5)
        {
            likelihood = "Less Likely Than Not";
        }
        if (pOfWinning >= 0.2 && pOfWinning < 0.4)
        {
            likelihood = "Unlikely";
        }
        if (pOfWinning >= 0.0 && pOfWinning < 0.2)
        {
            likelihood = "Highly Unlikely";
        }
        else { likelihood = "Likelihood Calculation Error"; }

        return likelihood;
    }

    public void CalculateProbabilities()
    {
        // convert values to floats so decimals do not come out as 0
        float numberOfDrawsf = (float)numberOfDraws;
        float numberOfCardsf = (float)numberOfCards;

        float numberOfSpadesf = (float)numberOfSpades;
        float numberOfHeartsf = (float)numberOfHearts;
        float numberofClubsf = (float)numberofClubs;
        float numberOfDiamondsf = (float)numberOfDiamonds;

        float numberOfOnesf = (float)numberOfOnes;
        float numberOfTwosf = (float)numberOfTwos;
        float numberOfThreesf = (float)numberOfThrees;
        float numberOfFoursf = (float)numberOfFours;
        float numberOfFivesf = (float)numberOfFives;
        float numberOfSixesf = (float)numberOfSixes;
        float numberOfSevensf = (float)numberOfSevens;
        float numberOfEightsf = (float)numberOfEights;
        float numberOfNinesf = (float)numberOfNines;
        float numberOfTensf = (float)numberOfTens;
        float numberOfKingsf = (float)numberOfKings;
        float numberOfQueensf = (float)numberOfQueens;
        float numberOfJacksf = (float)numberOfJacks;
        float numberOfAcesf = (float)numberOfAces;


        //probablility of drawing a 1
        p1 = numberOfOnesf / numberOfCardsf;
        //probability of drawing a 2
        p2 = numberOfTwosf / numberOfCardsf;
        //probability of drawing a 3
        p3 = numberOfThreesf / numberOfCardsf;
        //probability of drawing a 4
        p4 = numberOfFoursf / numberOfCardsf;
        //probability of drawing a 5
        p5 = numberOfFivesf / numberOfCardsf;
        //probability of drawing a 6
        p6 = numberOfSixesf / numberOfCardsf;
        //probability of drawing a 7
        p7 = numberOfSevensf / numberOfCardsf;
        //probability of drawing an 8
        p8 = numberOfEightsf / numberOfCardsf;
        //probability of drawing a 9
        p9 = numberOfNinesf / numberOfCardsf;
        //probability of drawing a 10
        p10 = numberOfTensf / numberOfCardsf;
        //probability of drawing a Jack
        pJ = numberOfJacksf / numberOfCardsf;
        //probability of drawing a Queen
        pQ = numberOfQueensf / numberOfCardsf;
        //probability of drawing a King
        pK = numberOfKingsf / numberOfCardsf;
        //probability of drawing an Ace
        pA = numberOfAcesf / numberOfCardsf;

        //probablility of drawing a club
        pClub = numberofClubsf / numberOfCardsf;
        //probablility of drawing a diamond
        pDiamond = numberOfDiamondsf / numberOfCardsf;
        //probablility of drawing a heart
        pHeart = numberOfHeartsf / numberOfCardsf;
        //probablility of drawing a spade
        pSpade = numberOfSpadesf / numberOfCardsf;

        chanceOfGetting21Next = ChanceOfGetting21Next();
        UpdateCardCountingBoard();
    }

    public void UpdateCardCountingBoard()
    {
        pDealerBustText.text = "Dealer Bust - " + dealerBustChance.ToString("F2") + "%";
        p21NextHitText.text = "21 Next Hit - " + (100 * ChanceOfGetting21Next()).ToString("F2") + "%";
        zenCountText.text = "Zen Count - " + zenCount;
        zenHintText.text = ZenHint();
        hiOptIICountText.text = "Hi Opt II Count - " + hiOptIICount;
        hiOptIIHintText.text = HiOptIIHint();
    }

    public void CalculateDealersChanceOfBusting(int cardNumber)
    {
        if(cardNumber == 2) { dealerBustChance = 35.30f; }
        if(cardNumber == 3) { dealerBustChance = 37.56f; }
        if(cardNumber == 4) { dealerBustChance = 40.28f; }
        if(cardNumber == 5) { dealerBustChance = 42.89f; }
        if(cardNumber == 6) { dealerBustChance = 42.08f; }
        if(cardNumber == 7) { dealerBustChance = 25.99f; }
        if(cardNumber == 8) { dealerBustChance = 23.86f; }
        if(cardNumber == 9) { dealerBustChance = 23.34f; }
        if (cardNumber == 10) { dealerBustChance = 21.43f; }
        // ace
        if (cardNumber == 11) { dealerBustChance = 11.65f; }
    }

    public float ChanceOfGetting21Next()
    {
        int neededDrawValue;
        float probability;

        neededDrawValue = 21 - playerScript.handValue;
        if(neededDrawValue == 1)
        {
            probability = pA;
            return probability;
        }
        if (neededDrawValue == 2)
        {
            probability = p2;
            return probability;
        }
        if (neededDrawValue == 3)
        {
            probability = p3;
            return probability;
        }
        if (neededDrawValue == 4)
        {
            probability = p4;
            return probability;
        }
        if (neededDrawValue == 5)
        {
            probability = p5;
            return probability;
        }
        if (neededDrawValue == 6)
        {
            probability = p6;
            return probability;
        }
        if (neededDrawValue == 7)
        {
            probability = p7;
            return probability;
        }
        if (neededDrawValue == 8)
        {
            probability = p8;
            return probability;
        }
        if (neededDrawValue == 9)
        {
            probability = p9;
            return probability;
        }
        if (neededDrawValue == 10)
        {
            probability = p10 + pK + pQ + pJ;
            return probability;
        }
        if (neededDrawValue == 11)
        {
            probability = pA;
            return probability;
        }
        else {
            Debug.Log("Could not determine likelihood of next draw = 21");
            probability = 0;
            return probability; }

    }
}
