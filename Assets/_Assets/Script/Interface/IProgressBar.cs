using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IProgressBar 
{
    public event EventHandler<ProgressBarEvent> ProgressBar;
    public class ProgressBarEvent : EventArgs{
        public float  progressNomalize;
    }
}
