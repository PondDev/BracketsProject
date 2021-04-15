using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tester
{
    public Test test;
    public Transform outZone;
    
    public Tester(Test _test)
    {
        test = _test;
    }

    public Sample TestSample(Sample sample)
    {
        if (sample.products[test] != -1)
        {
            Sample newSample = SampleDatabase.Instance.GetSampleByID(sample.products[test]);
            return newSample;
        }
        else
        {
            return null;
        }
    }
}
