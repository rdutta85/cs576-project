using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Guitar : ChordKnowledge
{

    /*
    NOTE TO GROUP:
    The following audio clips play guitar chords I sampled from MuseScore4. 

    Each audio file is named as follows:
    First letter is the note name from A to G.
    Letters after the note represent modifiers to the original chord.
    "f" means that a note is flat, meaning the pitch has been decreased by a half step (notated in music as "#").
    "s" means that a note is sharp, meaning the pitch has been raised by a half step (notated in music as "b").

    Chords are constructed by taking the root note and adding specific notes that are above that note. 

    To successfully construct a chord, use the following algorithm: 
    1. Select a root note. 

    Note names must be 2 away at each note starting at the root:
    Example: C, E, G.

    2. Add the remaining two notes in the chord.

    3. Select the correct modifiers. (See below)
    Major Chords: root, root + 4, root + 7
    Minor Chords: root, root + 3, root + 7

    Notice that the only difference between the major and minor chords is the middle note.        

    Modifiers:
    All pitches in order and their note names:
    0  1  2  3  4  5  6  7  8  9  10 11 12
    C     D     E  F     G     A     B  C

    Modifiers increase or decrease the pitch by 1. If C = 0, then C# = 1. If B is 11, then Bb is 10. 

    Notice: Notes have different number of steps away from each other. Specifically, B and C, and E and F are each
    1 step away, whereas every other pair of consecutive notes are 2 steps away.

    This means that for every pair of consecutive notes X and Y that are 2 steps away, X# = Yb. For example, C# = Db.

    This also means that for every pair of consecutive notes P and Q that are 1 step away, P = Qb, and P# = Q. 
    For example, E = Fb, and E# = F. 
    Other instance, B = Cb, and B# = C
    The modified note is never used as a chord root for this project.

    There are several chords that sound the same but are notated differently. For this project, we will only use the
    simplest name for each, except for one note that we will use is Ab instead of G#. This implementation detail is more suited to 
    band instruments rather than orchestra instruments, however, I messed up and I don't want to change it anymore
    because it is a mere note name. More details will be in Enemy.cs

    Example of the algorithm:
    1. fs (F#)

    2. Letter note names are as follows: F, A, C (after G is A, the notes wrap around like a ring)

    3. Select the correct modifiers.
        i. F is F# (derived from the root)
        ii. F# = 6 (F = 5), therefore A must be A# (6+4)
        iii. F# = 6, therefore C must be C# (6+7 % 11)

    Another Example of the algorithm:
    1. efm (Eb minor)

    2. Letter note names are: E, G, B

    3. Modifiers:
        i.   E -> Eb (3)
        ii.  G -> Gb (3+3=6)
        iii. B -> Bb (3+7=10)

    Instead of using a bunch of switch statements for the construction of a chord's notes, construct a dictionary
    with indicies representing note names and apply addition and modulus to get the note. 
 */

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
    public List<Button> aToGButtons; // Buttons 'A' - 'G'
    public Button flatButton;        // Button 'b'
    public Button sharpButton;       // Button 's'
    public Button minorButton;       // Button 'm'

    private Button selectedNoteButton; // Tracks the note button being played
    private bool isFlat = false;       // Tracks if the flat modifier is active
    private bool isSharp = false;      // Tracks if the sharp modifier is active
    private bool isMinor = false;      // Tracks if the minor modifier is active

    private string chord; //last played chord
    private TextMeshProUGUI chordnotestextUI;
    void Start()
    {
        chordnotestextUI = GameObject.Find("ChordNotes").GetComponent<TextMeshProUGUI>();

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
        DisplayChordNotes();
        CheckForNearbyEnemies();
    }
    //

    void DisplayChordNotes()
    {
        string firstLetter = chord[0].ToString().ToUpper();
        string rest = chord.Substring(1);
        string chordstr = firstLetter + rest;

        List<Note> player_notes = chordToNotes[(Chord)System.Enum.Parse(typeof(Chord), chordstr)];

        string noteStr = "Notes: ";
        foreach (Note note in player_notes)
        {
            noteStr += note.ToString() + " ";
        }

        StartCoroutine(DisplayChordNotesRoutine(noteStr));

    }

    IEnumerator DisplayChordNotesRoutine(string noteStr)
    {
        chordnotestextUI.text = noteStr;
        chordnotestextUI.color = Color.white;
        yield return new WaitForSeconds(1.5f);
        chordnotestextUI.text = "";
        chordnotestextUI.color = Color.clear;
    }

    void CheckForNearbyEnemies()
    {
        // Get all colliders within a sphere centered on the player's position
        Collider[] hitColliders = Physics.OverlapSphere(junko.transform.position, junko.AttackRange);

        foreach (var hitCollider in hitColliders)
        {
            // Check if the collider is an enemy
            if (hitCollider.gameObject.CompareTag("Enemy"))
            {
                Enemy hitObject = hitCollider.gameObject.GetComponent<Enemy>();
                hitObject.Attacked(chord, Random.Range(junko.AttackDamage[0], junko.AttackDamage[1]));
            }
        }
    }

    // Toggles the flat modifier
    void ToggleFlat()
    {
        isFlat = !isFlat;

        // NOTE TO GROUP: Music notes cannot be both flat and sharp. They are like negative/positive modifiers to the note. 
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

    // Toggles a button's state
    void ToggleButton(Button button, bool isActive)
    {
        button.GetComponent<Image>().color = isActive ? Color.yellow : Color.white;
    }

    // Plays the appropriate audio clip based on the selected note and modifiers
    void PlayNoteAudio()
    {
        if (selectedNoteButton == null)
        {
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
                        chord = "bfm";
                        clipToPlay = bfm;
                        break;
                    case 1: // B#m = Cm
                        chord = "cm";
                        clipToPlay = cm;
                        break;
                    case 2: // C#m
                        chord = "csm";
                        clipToPlay = csm;
                        break;
                    case 3: // D#m = Efm
                        chord = "efm";
                        clipToPlay = efm;
                        break;
                    case 4: // E#m = Fm
                        chord = "fm";
                        clipToPlay = fm;
                        break;
                    case 5: // F#m
                        chord = "fsm";
                        clipToPlay = fsm;
                        break;
                    case 6: // G#m = Afm
                        chord = "afm";
                        clipToPlay = afm;
                        break;
                }
            }
            else
            {
                switch (index)
                {
                    case 0: // A# = Bf
                        chord = "bf";
                        clipToPlay = bf;
                        break;
                    case 1: // B# = C
                        chord = "c";
                        clipToPlay = c;
                        break;
                    case 2: // C#
                        chord = "cs";
                        clipToPlay = cs;
                        break;
                    case 3: // D# = Ef
                        chord = "ef";
                        clipToPlay = ef;
                        break;
                    case 4: // E# = F
                        chord = "f";
                        clipToPlay = f;
                        break;
                    case 5: // F#
                        chord = "fs";
                        clipToPlay = fs;
                        break;
                    case 6: // G# = Af
                        chord = "af";
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
                        chord = "afm";
                        clipToPlay = afm;
                        break;
                    case 1: // Bfm
                        chord = "bfm";
                        clipToPlay = bfm;
                        break;
                    case 2: // Cfm = Bm
                        chord = "bm";
                        clipToPlay = bm;
                        break;
                    case 3: // Dfm = Csm
                        chord = "csm";
                        clipToPlay = csm;
                        break;
                    case 4: // Efm
                        chord = "efm";
                        clipToPlay = efm;
                        break;
                    case 5: // Ffm = Em
                        chord = "em";
                        clipToPlay = em;
                        break;
                    case 6: // Gfm = Fsm
                        chord = "fsm";
                        clipToPlay = fsm;
                        break;
                }
            }
            else
            {
                switch (index)
                {
                    case 0: // Af
                        chord = "af";
                        clipToPlay = af;
                        break;
                    case 1: // Bf
                        chord = "bf";
                        clipToPlay = bf;
                        break;
                    case 2: // Cf = B
                        chord = "b";
                        clipToPlay = b;
                        break;
                    case 3: // Df = Cs
                        chord = "cs";
                        clipToPlay = cs;
                        break;
                    case 4: // Ef
                        chord = "ef";
                        clipToPlay = ef;
                        break;
                    case 5: // Ff = E
                        chord = "e";
                        clipToPlay = e;
                        break;
                    case 6: // Gf = Fs
                        chord = "fs";
                        clipToPlay = fs;
                        break;
                }
            }
        }
        else
        {
            if (isMinor)
            {
                switch (index)
                {
                    case 0: // Am
                        chord = "am";
                        clipToPlay = am;
                        break;
                    case 1: // Bm
                        chord = "bm";
                        clipToPlay = bm;
                        break;
                    case 2: // Cm
                        chord = "cm";
                        clipToPlay = cm;
                        break;
                    case 3: // Dm
                        chord = "dm";
                        clipToPlay = dm;
                        break;
                    case 4: // Em
                        chord = "em";
                        clipToPlay = em;
                        break;
                    case 5: // Fm
                        chord = "fm";
                        clipToPlay = fm;
                        break;
                    case 6: // Gm
                        chord = "gm";
                        clipToPlay = gm;
                        break;
                }
            }
            else
            {
                switch (index)
                {
                    case 0: // A
                        chord = "a";
                        clipToPlay = a;
                        break;
                    case 1: // B
                        chord = "b";
                        clipToPlay = b;
                        break;
                    case 2: // C
                        chord = "c";
                        clipToPlay = c;
                        break;
                    case 3: // D
                        chord = "d";
                        clipToPlay = d;
                        break;
                    case 4: // E
                        chord = "e";
                        clipToPlay = e;
                        break;
                    case 5: // F
                        chord = "f";
                        clipToPlay = f;
                        break;
                    case 6: // G
                        chord = "g";
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