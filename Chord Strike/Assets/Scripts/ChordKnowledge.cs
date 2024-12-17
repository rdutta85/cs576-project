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
    { Chord.A, new List<Note> { Note.A, Note.Cs, Note.E, Note.Df, Note.Ff } },
    { Chord.Am, new List<Note> { Note.A, Note.C, Note.E, Note.Bs, Note.Ff } },
    { Chord.B, new List<Note> { Note.B, Note.Ds, Note.Fs, Note.Cf, Note.Ef, Note.Gf } },
    { Chord.Bm, new List<Note> { Note.B, Note.D, Note.Fs, Note.Cf, Note.Gf } },
    { Chord.C, new List<Note> { Note.C, Note.E, Note.G, Note.Bs, Note.Ff } },
    { Chord.Cm, new List<Note> { Note.C, Note.Ef, Note.G, Note.Bs, Note.Ds } },
    { Chord.D, new List<Note> { Note.D, Note.Fs, Note.A, Note.Gf } },
    { Chord.Dm, new List<Note> { Note.D, Note.F, Note.A, Note.Es } },
    { Chord.E, new List<Note> { Note.E, Note.Gs, Note.B, Note.Ff, Note.Af } },
    { Chord.Em, new List<Note> { Note.E, Note.G, Note.B, Note.Ff } },
    { Chord.F, new List<Note> { Note.F, Note.A, Note.C, Note.Es, Note.Bs } },
    { Chord.Fm, new List<Note> { Note.F, Note.Af, Note.C, Note.Es, Note.Gs, Note.Bs } },
    { Chord.G, new List<Note> { Note.G, Note.B, Note.D, Note.Cf } },
    { Chord.Gm, new List<Note> { Note.G, Note.Bf, Note.D, Note.Af } },
    { Chord.Cs, new List<Note> { Note.Cs, Note.Es, Note.Gs, Note.Df, Note.F, Note.Af } },
    { Chord.Csm, new List<Note> { Note.Cs, Note.E, Note.Gs, Note.Df, Note.Ff, Note.Af } },
    { Chord.Af, new List<Note> { Note.Af, Note.C, Note.Ef, Note.Gs, Note.Bs, Note.Ds } },
    { Chord.Afm, new List<Note> { Note.Af, Note.Cf, Note.Ef, Note.Gs, Note.B, Note.Ds } },
    { Chord.Bf, new List<Note> { Note.Bf, Note.D, Note.F, Note.Af, Note.Es } },
    { Chord.Bfm, new List<Note> { Note.Bf, Note.Df, Note.F, Note.Af, Note.Cs, Note.Es } },
    { Chord.Ef, new List<Note> { Note.Ef, Note.G, Note.Bf, Note.Ds, Note.Af } },
    { Chord.Efm, new List<Note> { Note.Ef, Note.Gf, Note.Bf, Note.Ds, Note.Fs, Note.Af } },
    { Chord.Fs, new List<Note> { Note.Fs, Note.Af, Note.Cs, Note.Gf, Note.Bf, Note.Df } },
    { Chord.Fsm, new List<Note> { Note.Fs, Note.A, Note.Cs, Note.Gf, Note.Df } }
};
}