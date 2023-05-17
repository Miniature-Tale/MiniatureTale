using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class AdderTest
{
    // A Test behaves as an ordinary method
    [Test]
    public void AdderTestSimplePasses()
    {
        var myGameObject = new GameObject();
        myGameObject.AddComponent<CustomAdder>();
        var mycustomAdder = myGameObject.GetComponent<CustomAdder>();

        int result = mycustomAdder.MyAdderMethod(1,2);

        Assert.AreEqual(3, result);
    }
}
