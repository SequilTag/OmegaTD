using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Loader<SoundManager>
{
    [SerializeField] AudioClip shot1;
    [SerializeField] AudioClip shot2;
    [SerializeField] AudioClip background;
    [SerializeField] AudioClip click;

    public AudioClip Shot1
    {
        get
        {
            return shot1;
        }
    }
    public AudioClip Shot2
    {
        get
        {
            return shot2;
        }
    }
    public AudioClip Background
    {
        get
        {
            return background;
        }
    }

    public AudioClip Click
    {
        get
        {
            return click;
        }
    }

}
