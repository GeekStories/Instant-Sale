using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour {

    public GameManager gm;
    public Image slideshow;
    public Text tutorialText;

    public Sprite[] buying;
    string buying_text = "INVESTING IN PROPERTIES \n \n" +
                         " To purchase a property, simply drag the card into a property slot. For a sale to be successful " +
                         "you need to make sure you have enough money! \n \n" +
                         " Once the property is yours, you will start to earn rent from it! Rent is collected weekly (when you " +
                         "tap \"Next Week..\"). Rent is only added while a card is present in a property slot. Which means " +
                         "properties that are currently being dragging around or properties that are sitting in the sell slot, " +
                         "will not have their rent values added to your weekly rent. \n \n" +
                         " Each property card that sits on the card pile is only available for a certain amount of weeks. If "+
                         "the weeks left date reaches 0, a new card will be generated. Each week "+
                         "you get a starting limit of 5 properties before you need to proceed to the next week to have 5 more property cards "+
                         "generated.";

    public Sprite[] selling;
    string selling_text = "SELLING PROPERTIES \n \n" +
                          "If you can't afford a new property slot, and have no empty slots for better properties, you can drag" +
                          " a property card you no longer want from its respective property slot and drop it in to the sell/pass slot. " +
                          "Once places you will be made an offer. To accept this offer simply click on the Sell button. " +
                          "When you do sell a property, there is a sellbuffer. This starts at 0.25 (25%), and can be increased through" +
                          "prestige. Upgrading the sellbuffer will increase how much money you earn through selling your house!";

    public Sprite[] passing;
    string passing_text = "PASSING PROPERTIES \n \n" +
                          "As with selling a property, sometimes you just can't afford what is on offer. " +
                          "Luickly you can pass on properties by simply clicking on the little X button in the top left of the card pile. " +
                          "This will delete the card and generate a new one. However, a new card will only be generated if there are" +
                          " enough left in the pile. \n \n" +
                          "(see the yellow x/x in the top right of the card pile, the left value is how many cards are left, the" +
                          " right value is how many are generated per week. This is increased by 1 per year passed)";

    public Sprite[] upgrading;
    string upgrade_text = "UPGRADING PROPERTIES \n \n" +
                          "Upgrading a property is easy! Simply click on the \"+\" icon in the property slot. A panel will display two" +
                          " values. One being how much it will cost to upgrade that property, and the other being how much rent" +
                          " will be added due to the upgrade! \n \n" +
                          "Keep in mind however, the rent won't be earned until the property is back"+
                          " in a property slot! So remember to put that property back so you earn money from it!";

    public Sprite[] networth;
    string networth_text = "NETHWORTH \n \n" +
                           "Your networth is calculated is a very simple way. Networth is calculated by the value of each property" +
                           " multiplied by a sellbuffer (starts at 0.25). This sellbuffer can be increased through prestige. \n \n"+
                           "This is important because the goal is to get the highest networth in 50 years! \n \n" +
                           "Yes, the sellbuffer modifier used here, is the same modifier used to calculate your offer when you sell a property!";

    public Sprite[] time;
    string time_text = "WEEKS AND YEARS \n \n" +
                       "As for the \"W1\" and \"Y0\" values, W = Weeks and Y = Years. For every year that passes you gain a %0.001 bonus to your rent!" +
                       "This value is represented in the brackets underneath your weekly rent. \n \n"+
                       "On top of that, you also gain %5 on top of that sellbuffer modifier to increase your sell price! Your sell"+
                       " buffer starts at %25 and increases each year, however this can be increased manually through prestige! \n \n" +
                       "To \"automate\" the next week button, you can increase your weeks per second limit through prestige. This will" +
                       " allow you to use the two buttons to the right of the next week button and increase how many weeks pass" +
                       " per second automatically! This will collect rent for you without having to constantly click!";
     
    int section = -1;

    private void Start(){
        Next();
    }

    public void Next(){
        section++;

        if (section == 6){
            section = 0;
        }

        switch (section){
            case 0:
                tutorialText.text = buying_text;
                break;
            case 1:
                tutorialText.text = selling_text;
                break;
            case 2:
                tutorialText.text = passing_text;
                break;
            case 3:
                tutorialText.text = upgrade_text;
                break;
            case 4:
                tutorialText.text = networth_text;
                break;
            case 5:
                tutorialText.text = time_text;
                break;
            default:
                break;
        }
    }

    public void CloseTutorial(){
        if (gm.sounds)
            gm.ambientSource.PlayOneShot(gm.buttonClick);

        //Close the tutorial screen
        gm.OpenTutorial();

        tutorialText.text = buying_text;
        section = -1;
    }
}
