using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scanner : MonoBehaviour
{
    [SerializeField]
    bool triggerpress;
    [SerializeField]
    bool triggerheldonenter;
    public List<string> journal = new List<string> ();
    [SerializeField]
    private MonoBehaviour outline;
    [SerializeField]
    float triggerTimer = 0f;
    [SerializeField]
    private GameObject itemObject;
    [SerializeField]
    private GameObject journalObject;
    [SerializeField]
    private GameObject scannerObject;
    [SerializeField]
    private GameObject itemContent;
    [SerializeField]
    private Button journalEntry;
    [SerializeField]
    private GameObject journalScreen;
    [SerializeField]
    private GameObject scanHighlight;
    [SerializeField]
    private GameObject scanInfo;
    [SerializeField]
    private GameObject scanStart;
    [SerializeField]
    private GameObject scanComplete;
    [SerializeField]
    private GameObject smartDevice;
    [SerializeField]
    private Collider parentCollider;
    private bool scanCompleted;
    private bool multiScan;
    private int scanVariant;
    [SerializeField]
    private int numScans = 0;
    
    void OnEnable()
    {
        /*reference all objects needed from the start for enabling/disabling
        we also need to disable menu elements we want to start as inactive manually
        otherwise unity will be unable to store their references*/
        smartDevice = GameObject.Find("SmartDevice(Clone)");
        parentCollider = smartDevice.GetComponent<Collider>();
        /*I commented the below section out because disabling objects later in the code caused
          a null reference upon trying to enable them again. Instead I explicitly set the references
          using the inspector.*/
        /*
        itemObject = GameObject.Find("ItemsList");
        journalObject = GameObject.Find("JournalList");
        scannerObject = GameObject.Find("Scanner");
        journalContent = GameObject.Find("JournalContent");
        scanHighlight = GameObject.Find("Highlight");
        scanInfo = GameObject.Find("ScanInfo");
        scanStart = GameObject.Find("ScanStart");
        scanComplete = GameObject.Find("ScanComplete");
        */

        itemObject.SetActive(false);
        journalObject.SetActive(false);
        journalScreen.SetActive(false);
        journalEntry.gameObject.SetActive(false);
        scannerObject.SetActive(false);
        scanStart.SetActive(false);
        scanComplete.SetActive(false);
        
    }

    void Update()
    {

        //Get input from trigger press
        triggerpress = Input.GetKey("joystick button 15");
        if (scannerObject.activeInHierarchy == false)
        {
            parentCollider.enabled = false;
        }
        if (scannerObject.activeInHierarchy == true)
        {
            parentCollider.enabled = true;
        }


    }

    void OnTriggerEnter(Collider other)
    {
        //if object is scannable, enable it's outline, reset scan flag, and make sure trigger isn't already pressed
        if (other.tag == "Scannable")
        {
            outline = other.GetComponent("MeshOutline") as MonoBehaviour;
            outline.enabled = true;
            scanCompleted = false;
            if (triggerpress == true)
            {
                triggerheldonenter = true;
            }
        }
    }

    void OnTriggerStay(Collider other)
    {
        /*if object is scannable: turn outline on, and if trigger isn't already pressed, holding trigger for 3 seconds 
         * will activate different texts on smart device, and scan object to journal if it has not been scanned previously.
         * Once scan completes, set scan flag to true so journal entries don't get repeatedly added*/
        if (other.tag == "Scannable" && triggerheldonenter == false)
        {
            
            if (triggerpress == true && triggerTimer < 3f)
            {
                triggerTimer += Time.deltaTime;
                scanHighlight.SetActive(false);
                scanInfo.SetActive(false);
                scanStart.SetActive(true);
            }

            if (triggerpress ==  false)
            {
                triggerheldonenter = false;
                triggerTimer = 0f;
                scanHighlight.SetActive(true);
                scanInfo.SetActive(true);
                scanStart.SetActive(false);
                scanComplete.SetActive(false);
            }
           
            if (triggerTimer >= 3f && scanCompleted == false)
            {
                if (journal.Contains(other.name) == false)
                {
                    Scan(other.name);
                }
                scanHighlight.SetActive(false);
                scanInfo.SetActive(false);
                scanStart.SetActive(false);
                scanComplete.SetActive(true);
            }

        }
    }

    void OnTriggerExit(Collider other)
    {
        //reset flag for trigger being held on exit and disable it's outline
        triggerheldonenter = false;
        outline.enabled = false;
        scanHighlight.SetActive(true);
        scanInfo.SetActive(true);
        scanStart.SetActive(false);
        scanComplete.SetActive(false);
    }

    private void Scan(string scanName)
    {
        /*This function looks big and scary, but it's not. Here's what it does:
         * Add object name to journal list, then reference journal entry original object to grab it's position
         * Create an offset for the next instance of the entry by multiplying it by the number of scans
         * Instantiate both the new entry, and journal screen and set their parents, position only needed for entry
         * Create uniquely individual names for these clones after instantiation. This is just in case their reference passed to ButtonPath below becomes null
         * Assign new names to the newly instantiated objects
         * Create temp variables for the text and entry name, then assign their values in the switch statement, based on the name of the object OnTriggerEnter references
         * Assign varied texts to the components of the new instances, which will show up in the visual UI elements
         * Add the listener for OnClick for the new entry, calling ButtonPath. Note: if references passed to ButtonPath become null, I'll instead opt not to pass those references and search using transform.find
         * Then, based on the number of lines, set the size of the text element to be just enough to scroll through all the text
         * Check to see if the size set based on lines is less than the available screen space (forcing text to display center screen) and if so, add extra length to be exactly equal to the screen space
         * Finally, increment the number of scans for the next scan to reference*/
        Rescan:
        journal.Add(scanName);
        Vector3 entryOffset = new Vector3(0, -90 * numScans, 0);
        Button entryClone = Instantiate<Button>(journalEntry, journalEntry.transform.parent, false);
        entryClone.transform.localPosition += entryOffset;
        if (numScans > 4)
        {
            float listDeltaY = journalObject.transform.Find("EntryViewport/JournalContent").GetComponent<RectTransform>().sizeDelta.y;
            journalObject.transform.Find("EntryViewport/JournalContent").GetComponent<RectTransform>().sizeDelta = new Vector2(320, listDeltaY + 90);
        }
        GameObject screenClone = Instantiate<GameObject>(journalScreen, journalScreen.transform.parent);
        string entryCloneName = "JournalEntry" + numScans.ToString();
        string screenCloneName = "JournalScreen" + numScans.ToString();
        entryClone.name = entryCloneName;
        entryClone.gameObject.SetActive(true);
        screenClone.name = screenCloneName;
        string entryName;
        string text;
        switch (scanName)
        {
            case "clock_note":
                text = "'Laura, Clock's broken. Don't call to have it fixed; we shouldn't spend that kind of money right now. Half the time, it's just a great big cabinet anyway. I'll buy you a watch when I come home. Call it an early Christmas present. Love, Arthur." + Environment.NewLine;
                entryName = "Clock Note";
                scanCompleted = true;
                break;
            case "george_birthday_letter":
                text = "George, I know it must be difficult for you to have a birthday without your father there to congratulate you. I confess, as important as it is that I stay in London for a few more days, I've half a mind to abandon my work and hop the first train back home. But there's a war on, and now that you're ten years old, I can trust you to be the man of the house while I'm away. It won't be an easy task, taking care of Mum and keeping the Germans away all on your own. I've asked Mr.Churchhill to personally oversee a small gift to help you in your duty. He's also told me to remind you that no civilized activity can take place without a proper cup of tea. Once I return, I'll bring you a cake so great in size you'll explode if you eat it all at once. That's a promise, man-to-man, and a Walker always keeps his promises. Happy birthday. All my love to your and your Mum. Dad" + Environment.NewLine;
                entryName = "George's Birthday Letter";
                scanCompleted = true;
                break;
            case "george_journal":
                text = "Note: This seems to be dated 7/4/32 \r\nToday I am seven years old. Father says that a Walker should write down all his great deeds for the next genurashon. Starting tomorrow I will do something great every day and write it here. \r\n8 April — Got top marks on the quiz today. \r\n9 April — Caught a frog in the creek with my bare hands. He seemed more scared of me than I was of him so I let him run free. \r\n10 April — Made it up to the first branch on the big oak tree on the hill across the creek. \r\n11 April - Left Mum a pretty yellow flower I found on the way home from school." + Environment.NewLine;
                entryName = "Journal Of Great Deeds";
                scanCompleted = true;
                break;
            case "secret_wall_light_lever":
                text = "Seems Like it can be pushed. What is it for?" + Environment.NewLine;
                entryName = "Wall Lever";
                scanCompleted = true;
                break;
            case "Lock_Body":
                text = "It's a simple combination lock. Maybe I can find the combo somewhere here?" + Environment.NewLine;
                entryName = "Combination Lock";
                scanCompleted = true;
                break;
            case "Grandfather_Clock_Head":
                text = "It's a Grandfather Clock, but time isn't ticking. The hands seem moveable..." + Environment.NewLine;
                entryName = "Grandfather Clock";
                scanCompleted = true;
                break;
            case "Photo_Frame":
                text = "It's a picture depicting Winston Churchhill. Doesn't seem secured very well." + Environment.NewLine;
                entryName = "Picture Frame";
                scanCompleted = true;
                break;
            case "Blueprint":
                text = "It's a blueprint that appears to be for this house. It seems to show that there's a basement, which is a little unusual for a house in these parts." + Environment.NewLine;
                entryName = "Blueprint";
                scanCompleted = true;
                break;
            case "basement_note":
                text = "I know I locked the chest with my name, like I was taught. But I can't remember my name. I thought I was Laura, but that wasn't right. Hart is too short, Walker is too long. Maybe I have been a Walker too long myself. I have been trapped in this room for a little more than eleven minutes. In another eighty seconds, the last of the air will be gone. I will die here, alone. \r\n I love you, George.";
                entryName = "Basement Letter";
                scanCompleted = true;
                break;
            case "ComputerEvans":
                text = "From: George Walker george.walker.mp@parliament.uk \r\nSent: Monday, 1 Jun 2020 10:41 \r\nTo: Bill Evans wevans@localoffice.gwalker.uk \r\nSubject: Re: Re: November Royal Legion event  \r\nFor God's sake, man! I can't be certain of my schedule for an event six months from now! Those pricks at the Legion already have my money; what do they care if I actually show up? Tell them I can't commit to an appearance, but I might pop in if I'm so inclined.  Regarding the new security measures, tell Gallagher: 1. Use today's date for your security PIN number 2. I don't want to hear any more of this techno-gobbledygook from him, ever" + Environment.NewLine;
                entryName = "Email: Walker to Evans";
                scanCompleted = true;
                multiScan = true;
                scanVariant = 1;
                break;
            case "ComputerEvans1":
                text = "From: Sara Hunt shunt@localoffice.gwalker.uk \r\nSent: Friday, 5 Jun 2020 13:10 \r\nTo: William Evans wevans @localoffice.gwalker.uk  \r\nSubject: Re: More damn security nonsense  \r\nBill,  Don't go over Walker's head on this, or on anything really. You weren't there when the phone girl who used to sit in the spot next to you got caught following Tom's guidelines that contradicted the old man's. When he stopped ranting to take a breath, the poor silly nit asked him if he kissed his mother with that mouth; he fired her on the spot and her computer still hasn't been reassigned.  Not even Grey is immune to his whims. If Walker says a computer code needs to be 123456, then the code is 123456 until he changes his mind for no reason. I certainly hope that's not yours.  \r\n- Sara" + Environment.NewLine;
                entryName = "Email: Hunt to Evans";
                scanCompleted = true;
                multiScan = true;
                scanVariant = 2;
                break;
            case "ComputerEvans2":
                text = "From: Tom Gallagher tgallagher@oceantek.com \r\nSent: Monday, 1 Jun 2020 09:27 \r\nTo: George Walker Local Constituency Office, All Users \r\nSubject: ATTN: Your passwords  \r\nIt has been all of one week since the local quarantines ended and Oceantek resumed operations. In that time, I have received over five hundred password - related requests from users who did not read the previous system-wide notice.I am sending this form email to every user at every office I manage in the hopes that it will clarify the process. Your password has been reset to the six-digit birthday you provided during registration, in day / month / year order.If you have administrator privileges, you can still log in with your existing credentials. When you next log in, the system will prompt you to generate a new password.  The users with administrator privileges at your site are: George Walker, Richard Grey, Sara Hunt. Thank you for your patience.    \r\nTom Gallagher  \r\nInformation Security Administrator  \r\nOceantek Solutions" + Environment.NewLine;
                entryName = "Email: Gallagher to Office";
                scanCompleted = true;
                multiScan = true;
                scanVariant = 3;
                break;
            case "ComputerEvans3":
                text = "From: George Walker george.walker.mp@parliament.uk \r\nSent: Wednesday, 8 Mar 2020 09:31 \r\nTo: George Walker Local Constituency Office, All Users \r\nSubject: Happy birthday, Ms. Hunt!  \r\nPlease wish a happy 28th birthday to Sara Hunt, our brilliant Office Manager!Here's hoping that she doesn't end up a cantankerous old-timer when she's my age, six decades from now!" + Environment.NewLine;
                entryName = "Email: Walker to Office";
                scanCompleted = true;
                multiScan = false;
                break;
            case "ComputerWalker":
                text = "From: Tom Gallagher tgallagher@oceantek.com \r\nSent: Friday, 5 Jun 2020 16:06 \r\nTo: George Walker george.walker.mp @parliament.uk  \r\nSubject: Security code  \r\nMr Walker:  As promised, I have turned the shade of yellow you sent me into the security code DBC484. I've already been to the office and programmed this code into your private safe.  Please delete this email immediately once you have committed the password to memory.  \r\nTom Gallagher \r\nInformation Security Administrator \r\nOceantek Solutions" + Environment.NewLine;
                entryName = "Email: Gallagher to Walker";
                scanCompleted = true;
                multiScan = true;
                scanVariant = 4;
                break;
            case "ComputerWalker1":
                text = "From: Richard Grey rgsec@anonymail.net \r\nSent: Monday, 8 Jun 2020 16:15 \r\nTo: George Walker george.walker.mp @parliament.uk  \r\nSubject: The investigation \r\nCity has contracted a private detective without police affiliation.Data is sparse, but their case history suggests tenacity and eye for detail.Entirely possible that office security will be breached. Orders?  \r\nFrom: George Walker george.walker.mp @parliament.uk \r\nSent: Friday, 5 Jun 2020 15:22 \r\nTo: R Grey rgsec @anonymail.net \r\nSubject: Re: The house  \r\nNot the news I wanted to start my weekend, Mr Grey. Might as well make the building inspector disappear entirely; the city will likely send investigators next.Monitor from a distance; if the police do make a connection, I will handle it on my end. Do not engage." + Environment.NewLine;
                entryName = "Email: Grey to Walker";
                scanCompleted = true;
                multiScan = false;
                break;
            case "ComputerGrey":
                text = "From: Tom Gallagher tgallagher@oceantek.com \r\nSent: Friday, 5 Jun 2020 16:08 \r\nTo: Richard Grey grey @rgsec.net  \r\nSubject: Re: Personal code  \r\nMr Grey:  It's a reference to the novel Fahrenheit 451, named for the temperature at which paper burns. While I can't change that secure PIN again without changing all the others, the Fahrenheit system is admittedly ridiculous so I compromised on the new safe code. Yours is 0233, the approximate equivalent in degrees Celsius. Remember that Mr Walker must enter his own password as well, and one of you must use a full-access ID card.  Also, there's no call for rudeness. First off, I'm a Scot who works in IT; I am immune to third-form playground insults. Second, I've had many users frustrated that they have to alter their workflow to meet legally required security standards; their tears taste like the finest Macallan 25. Lastly, you work in security too, mate. You know better than me what's required to ensure all the government secrets you work with stay secret.  \r\nTom Gallagher \r\nInformation Security Administrator \r\nOceantek Solutions" + Environment.NewLine;
                entryName = "Email: Gallagher to Grey";
                scanCompleted = true;
                break;
            case "ComputerHunt":
                text = "From: William Evans wevans@localoffice.gwalker.uk \r\nSent: Friday, 5 Jun 2020 11:27 \r\nTo: Sara Hunt shunt @localoffice.gwalker.uk  \r\nSubject: More damn security nonsense  \r\nYou got your secure PIN set up as your birth date, right? I get enough abuse from Walker as is, I don't need more from the IT guy. Can you help me change mine so I don't have to interact with either of those twits ?  \r\n-Bill" + Environment.NewLine;
                entryName = "Email: Evans to Hunt";
                scanCompleted = true;
                multiScan = true;
                scanVariant = 5;
                break;
            case "ComputerHunt1":
                text = "From: Tom Gallagher tgallagher@oceantek.com \r\nSent: Wednesday, 3 Jun 2020 10:27 \r\nTo: Sara Hunt shunt @localoffice.gwalker.uk  \r\nSubject: Password resets  \r\nSara,  Why can't all my users be like you? Courteous; respectful of my time; saying \"please\" and \"thank you\" even with the boss breathing down your neck. I don't know how you do it. Tell him you caught me at a good time and I got the job done in twenty minutes. Isn't it nice having end-to-end encryption so only you and I can read these emails?  The password to Mr Walker's workstation has been reset to default; remember that he needs to enter the day and month as \"0704\" and not the other way around. Mr Grey would not tell me his birth date, so his password is considerably less secure. Please remind him to change it when he next logs in; I cannot force him to do so without locking his account again for at least 24 hours, and he's already less than pleased with me.  By the by, Colin has been pestering me to invite you on a double date; he claims to be worried that you'll have another romantic disaster. I think he just wants to criticize your next would-be suitor, but you do have a rather troubling tendency to attract \"hot messes\". Let me know.  \r\nTom Gallagher \r\nInformation Security Administrator \r\nOceantek Solutions  \r\nFrom: Sara Hunt shunt @localoffice.gwalker.uk  \r\nSent: Wednesday, 3 Jun 2020 10:20 \r\nTo: Tom Gallagher tgallagher @oceantek.com  \r\nSubject: Password resets  \r\nGood morning Tom, I'm so sorry to trouble you again. You guessed it, forgotten password. This time it's two-for-one; both Mr Grey and the boss himself are unable to log in and he's demanding it be taken care of immediately. After you read this message, would you please set Mr Walker's password back to the default ? It's not terribly urgent, since I told him you likely had a backlog of at least an hour. He'll be over the moon if it takes you half that time.Thank you. Sara Hunt PS: Probably won't surprise you that last night was not exactly a great success. I try to give everyone a fair chance on the first date, but I've never been so glad to tell someone I had to work early.Trust me, you don't want the details." + Environment.NewLine;
                entryName = "Email: Gallagher to Hunt";
                scanCompleted = true;
                multiScan = false;
                break;
            default:
                text = "You haven't assigned text for this object. Check your code, quick!" + Environment.NewLine;
                entryName = "Uh-oh, Spaghetti-O";
                scanCompleted = true;
                break;
            
        }
        entryClone.GetComponentInChildren<Text>().text = entryName;
        entryClone.onClick.AddListener(() => ButtonPath(entryClone, screenClone));
        screenClone.transform.Find("Title").GetComponent<Text>().text = entryName;
        screenClone.transform.Find("JournalViewport/JournalContent").GetComponent<Text>().text = text;
        
        float textDeltaY = screenClone.transform.Find("JournalViewport/JournalContent").GetComponent<RectTransform>().sizeDelta.y;
        if (textDeltaY < 460 == true)
        {
            screenClone.transform.Find("JournalViewport/JournalContent").GetComponent<RectTransform>().sizeDelta = new Vector2(320, textDeltaY + (460 - textDeltaY));
        }
        numScans++;
        if (multiScan == true)
        {
            switch (scanVariant)
            {
                case 1:
                    scanName = "ComputerEvans1";
                    break;
                case 2:
                    scanName = "ComputerEvans2";
                    break;
                case 3:
                    scanName = "ComputerEvans3";
                    break;
                case 4:
                    scanName = "ComputerWalker1";
                    break;
                case 5:
                    scanName = "ComputerHunt1";
                    break;
                default:
                    Debug.Log("You didn't set the scan variant for a multi scan. Check your code.");
                    break;

            }
            goto Rescan;
        }
    }

    private void ButtonPath(Button entryButton, GameObject screenObject)
    {
        entryButton.transform.parent.parent.parent.gameObject.SetActive(false);
        screenObject.SetActive(true);
        Canvas.ForceUpdateCanvases();
        int numLines = screenObject.transform.Find("JournalViewport/JournalContent").GetComponent<Text>().cachedTextGenerator.lines.Count;
        Debug.Log("Number of lines: " + numLines.ToString());
        screenObject.transform.Find("JournalViewport/JournalContent").GetComponent<RectTransform>().sizeDelta = new Vector2(320, 46 * numLines);
    }
}
