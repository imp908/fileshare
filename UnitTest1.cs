using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.Collections.Generic;
using System.Text;

using System.Security.Cryptography;
using tNine;
namespace tNineTests_
{
    [TestClass]
    public class TokenTest
    {

        [TestMethod]
        public void TokenArrTest()
        {
            char[] ch = new char['a'];
            int id = 2;
            Label tk = new Label(id, new char['a']);

            Assert.IsTrue(ch.ToString().Equals(tk.arr.ToString()));
            Assert.AreEqual(id, tk.order);
        }


    }

    [TestClass]
    public class Labeltest
    {

        [TestMethod]
        public void LabelCommandTest()
        {
            Label twoA = new Label(1, new char[] { 'a' });
            Label twoB = new Label(2, new char[] { 'b' });


            char[] res0 = twoA.GetCommand();
            char[] res1 = twoB.GetCommand();

            Label twoC = new Label(3, new char[] { 'c', 'd', 'e' });
            char[] res2 = twoC.GetCommand();

            CollectionAssert.AreEqual(new char[] { 'a' }, res0);
            CollectionAssert.AreEqual(new char[] { 'b', 'b' }, res1);
            CollectionAssert.AreEqual(new char[] { 'c', 'd', 'e', 'c', 'd', 'e', 'c', 'd', 'e' }, res2);
        }
    }


    [TestClass]
    public class KeyTest
    {
        [TestMethod]
        public void KeyCommandTest()
        {
           
            HashSet<char[]> hs1 = new HashSet<char[]>() { new char[] { 'a' }, new char[] { 'b' }, new char[] { 'a', 'b' } };
            bool a = hs1.Contains(new char[] { 'b' });

            Label twoA = new Label(1, new char[] { 'a' });
            Label twoB = new Label(1, new char[] { 'b' });

            List<ILabel> twoTk = new List<ILabel>() { twoA, twoB };

            Button twoBt = new Button("2");

            Key two = new Key(twoBt, twoTk);

            char[] cmd = two.GetLabel(new char[] { 'a' }).arr;

            CollectionAssert.AreEqual(cmd, new char[] { 'a' });
        }
    }
    [TestClass]
    public class KeyPadTest
    {
        [TestMethod]
        public void KeyPadCommandTest()
        {
            Label twoA = new Label(1, new char[] { 'a' });
            Label twoB = new Label(2, new char[] { 'b' });
            Label twoC = new Label(3, new char[] { 'c','d','e' });

            List<ILabel> twoTk = new List<ILabel>() { twoA, twoB, twoC };

            Button twoBt = new Button("2");

            Key two = new Key(twoBt, twoTk);

            HashSet<IKey> keys = new HashSet<IKey>() { two };

            KeyPad kp = new KeyPad(keys);

            char[] res0 = kp.command('a');
            char[] res1 = kp.command('b');
            char[] res2 = kp.command(new char[] { 'c','d','e' });

            string res3 = kp.command("aba");

            CollectionAssert.AreEqual(new char[] { '2' }, res0);
            CollectionAssert.AreEqual(new char[] { '2', '2' }, res1);
            CollectionAssert.AreEqual(new char[] { '2','2','2' }, res2);
            Assert.AreEqual("2 22 2", res3);
        }
    }
}

