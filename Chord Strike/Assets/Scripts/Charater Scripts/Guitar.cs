using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Guitar : MonoBehaviour
{
    private AudioSource audioSource;
    private JunkochanControl junko;
    [Header("Audio Clips")]
    public AudioClip af;
    public AudioClip a;
    public AudioClip bf;
    public AudioClip b;
    public AudioClip c;
    public AudioClip cs;
    public AudioClip d;
    public AudioClip ef;
    public AudioClip e;
    public AudioClip f;
    public AudioClip fs;
    public AudioClip g;
    public AudioClip afm;
    public AudioClip am;
    public AudioClip bfm;
    public AudioClip bm;
    public AudioClip cm;
    public AudioClip csm;
    public AudioClip dm;
    public AudioClip efm;
    public AudioClip em;
    public AudioClip fm;
    public AudioClip fsm;
    public AudioClip gm;

    [Header("UI Buttons")]
    public List<Button> aToGButtons; // A-G buttons
    public Button flatButton;        // Button 'b'
    public Button sharpButton;       // Button 's'
    public Button minorButton;       // Button 'm'

    private Button selectedNoteButton; // Tracks the currently selected note button (A-G)
    private bool isFlat = false;       // Tracks if the flat modifier is active
    private bool isSharp = false;      // Tracks if the sharp modifier is active
    private bool isMinor = false;      // Tracks if the minor modifier is active

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        junko = GameObject.Find("JunkoChan").GetComponent<JunkochanControl>();
        // Assign listeners to A-G buttons
        foreach (Button btn in aToGButtons)
        {
            btn.onClick.AddListener(() => OnNoteButtonClicked(btn));
        }

        // Assign listeners to modifier buttons
        flatButton.onClick.AddListener(ToggleFlat);
        sharpButton.onClick.AddListener(ToggleSharp);
        minorButton.onClick.AddListener(ToggleMinor);
    }

    // Handles A-G note button clicks
    void OnNoteButtonClicked(Button clickedButton)
    {
        // Select the new button
        selectedNoteButton = clickedButton;

        // Play the corresponding audio
        PlayNoteAudio();
    }

    // Toggles the flat modifier
    void ToggleFlat()
    {
        isFlat = !isFlat;

        if (isFlat && isSharp)
        {
            isSharp = false;
        }
        ToggleButton(flatButton, isFlat);
        ToggleButton(sharpButton, isSharp);
    }

    // Toggles the sharp modifier
    void ToggleSharp()
    {
        isSharp = !isSharp;

        if (isSharp && isFlat)
        {
            isFlat = false;
        }
        ToggleButton(flatButton, isFlat);
        ToggleButton(sharpButton, isSharp);
    }

    // Toggles the minor modifier
    void ToggleMinor()
    {
        isMinor = !isMinor;
        ToggleButton(minorButton, isMinor);
    }

    // Selects a button and highlights it
    void SelectButton(Button button)
        {
            button.GetComponent<Image>().color = Color.yellow; // Indicate selection
        }

        // Deselects a button and resets its highlight
        void DeselectButton(Button button)
        {
            button.GetComponent<Image>().color = Color.white; // Reset to default
        }

    // Toggles a button's state
    void ToggleButton(Button button, bool isActive)
    {
        button.GetComponent<Image>().color = isActive ? Color.yellow : Color.white;
    }

    // Plays the appropriate audio clip based on the selected note and modifiers
    void PlayNoteAudio()
        {
            if (selectedNoteButton == null) {
                return;
            }

            int index = aToGButtons.IndexOf(selectedNoteButton);
            AudioClip clipToPlay = null;

            if (isSharp)
            {
                if (isMinor)
                {
                    switch (index)
                    {
                        case 0: // A#m = Bfm
                            clipToPlay = bfm;
                            break;
                        case 1: // B#m = Cm
                            clipToPlay = cm;
                            break;
                        case 2: // C#m
                            clipToPlay = csm;
                            break;
                        case 3: // D#m = Efm
                            clipToPlay = efm;
                            break;
                        case 4: // E#m = Fm
                            clipToPlay = fm;
                            break;
                        case 5: // F#m
                            clipToPlay = fsm;
                            break;
                        case 6: // G#m = Afm
                            clipToPlay = afm;
                            break;
                    }
                } 
                else
                {
                    switch (index)
                    {
                        case 0: // A# = Bf
                            clipToPlay = bf;
                            break;
                        case 1: // B# = C
                            clipToPlay = c;
                            break;
                        case 2: // C#
                            clipToPlay = cs;
                            break;
                        case 3: // D# = Ef
                            clipToPlay = ef;
                            break;
                        case 4: // E# = F
                            clipToPlay = f;
                            break;
                        case 5: // F#
                            clipToPlay = fs;
                            break;
                        case 6: // G# = Af
                            clipToPlay = af;
                            break;
                    }
                }
            }
            else if (isFlat)
            {
                if (isMinor)
                {
                    switch (index)
                    {
                        case 0: // Afm
                            clipToPlay = afm;
                            break;
                        case 1: // Bfm
                            clipToPlay = bfm;
                            break;
                        case 2: // Cfm = Bm
                            clipToPlay = bm;
                            break;
                        case 3: // Dfm = Csm
                            clipToPlay = csm;
                            break;
                        case 4: // Efm
                            clipToPlay = efm;
                            break;
                        case 5: // Ffm = Em
                            clipToPlay = em;
                            break;
                        case 6: // Gfm = Fsm
                            clipToPlay = fsm;
                            break;
                    }
                }
                else
                {
                    switch (index)
                    {
                        case 0: // Af
                            clipToPlay = af;
                            break;
                        case 1: // Bf
                            clipToPlay = bf;
                            break;
                        case 2: // Cf = B
                            clipToPlay = b;
                            break;
                        case 3: // Df = Cs
                            clipToPlay = cs;
                            break;
                        case 4: // Ef
                            clipToPlay = ef;
                            break;
                        case 5: // Ff = E
                            clipToPlay = e;
                            break;
                        case 6: // Gf = Fs
                            clipToPlay = fs;
                            break;
                    }
                }
            }
            else {
                if (isMinor)
                {
                    switch (index)
                    {
                        case 0: // Am
                            clipToPlay = am;
                            break;
                        case 1: // Bm
                            clipToPlay = bm;
                            break;
                        case 2: // Cm
                            clipToPlay = cm;
                            break;
                        case 3: // Dm
                            clipToPlay = dm;
                            break;
                        case 4: // Em
                            clipToPlay = em;
                            break;
                        case 5: // Fm
                            clipToPlay = fm;
                            break;
                        case 6: // Gm
                            clipToPlay = gm;
                            break;
                    }
                }
                else
                {
                    switch (index)
                    {
                        case 0: // A
                            clipToPlay = a;
                            break;
                        case 1: // B
                            clipToPlay = b;
                            break;
                        case 2: // C
                            clipToPlay = c;
                            break;
                        case 3: // D
                            clipToPlay = d;
                            break;
                        case 4: // E
                            clipToPlay = e;
                            break;
                        case 5: // F
                            clipToPlay = f;
                            break;
                        case 6: // G
                            clipToPlay = g;
                            break;
                    }
                }
            }
        


        if (clipToPlay != null)
        {
            junko.ChordStrike();
            audioSource.PlayOneShot(clipToPlay);
        }
    }
}