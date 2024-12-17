using System.Collections.Generic;
using UnityEngine;


public class ChordKnowledge : MonoBehaviour
{
    // All equivalents have been squashed
    public enum Chord
    {
        A, B, C, D, E, F, G, Am, Bm, Cm, Dm, Em, Fm, Gm,
        Af, Bf, Ef, Afm, Bfm, Efm,
        Cs, Fs, Csm, Fsm
    };


    // These are all possible note names. 
    public enum Note
    {
        A, B, C, D, E, F, G,
        Af, Bf, Cf, Df, Ef, Ff, Gf,
        As, Bs, Cs, Ds, Es, Fs, Gs
    }
    // Dictionary mapping chords to notes
    public Dictionary<Chord, List<Note>> chordToNotes = new Dictionary<Chord, List<Note>>
    {
        { Chord.A, new List<Note> { Note.A, Note.Cs, Note.E } },
        { Chord.Am, new List<Note> { Note.A, Note.C, Note.E } },
        { Chord.B, new List<Note> { Note.B, Note.Ds, Note.Fs } },
        { Chord.Bm, new List<Note> { Note.B, Note.D, Note.Fs } },
        { Chord.C, new List<Note> { Note.C, Note.E, Note.G } },
        { Chord.Cm, new List<Note> { Note.C, Note.Ef, Note.G } },
        { Chord.D, new List<Note> { Note.D, Note.Fs, Note.A } },
        { Chord.Dm, new List<Note> { Note.D, Note.F, Note.A } },
        { Chord.E, new List<Note> { Note.E, Note.Gs, Note.B } },
        { Chord.Em, new List<Note> { Note.E, Note.G, Note.B } },
        { Chord.F, new List<Note> { Note.F, Note.A, Note.C } },
        { Chord.Fm, new List<Note> { Note.F, Note.Af, Note.C } },
        { Chord.G, new List<Note> { Note.G, Note.B, Note.D } },
        { Chord.Gm, new List<Note> { Note.G, Note.Bf, Note.D } },
        { Chord.Cs, new List<Note> { Note.Cs, Note.Es, Note.Gs } },
        { Chord.Csm, new List<Note> { Note.Cs, Note.E, Note.Gs } },
        { Chord.Af, new List<Note> { Note.Af, Note.C, Note.Ef } },
        { Chord.Afm, new List<Note> { Note.Af, Note.Cf, Note.Ef } },
        { Chord.Bf, new List<Note> { Note.Bf, Note.D, Note.F } },
        { Chord.Bfm, new List<Note> { Note.Bf, Note.Df, Note.F } },
        { Chord.Ef, new List<Note> { Note.Ef, Note.G, Note.Bf } },
        { Chord.Efm, new List<Note> { Note.Ef, Note.Gf, Note.Bf } },
        { Chord.Fs, new List<Note> { Note.Fs, Note.As, Note.Cs } },
        { Chord.Fsm, new List<Note> { Note.Fs, Note.A, Note.Cs } },
    };
}