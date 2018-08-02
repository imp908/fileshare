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
            Label twoC = new Label(3, new char[] { 'c', 'd', 'e' });

            List<ILabel> twoTk = new List<ILabel>() { twoA, twoB, twoC };

            Button twoBt = new Button("2");

            Key two = new Key(twoBt, twoTk);

            HashSet<IKey> keys = new HashSet<IKey>() { two };

            KeyPad kp = new KeyPad(keys);

            char[] res0 = kp.command('a');
            char[] res1 = kp.command('b');
            char[] res2 = kp.command(new char[] { 'c', 'd', 'e' });

            string res3 = kp.command("aba");

            CollectionAssert.AreEqual(new char[] { '2' }, res0);
            CollectionAssert.AreEqual(new char[] { '2', '2' }, res1);
            CollectionAssert.AreEqual(new char[] { '2', '2', '2' }, res2);
            Assert.AreEqual("2 22 2", res3);
        }
    }


    [TestClass]
    public class KeyPadTnineTest
    {
        [TestMethod]
        public void KeyPadTnineConvertTest()
        {
            //key 2
            Label twoA = new Label(1, new char[] { 'a' });
            Label twoB = new Label(2, new char[] { 'b' });
            Label twoC = new Label(3, new char[] { 'c' });

            List<ILabel> twoTk = new List<ILabel>() { twoA, twoB, twoC };

            Button twoBt = new Button("2");

            Key two = new Key(twoBt, twoTk);

            //key 3
            Label threeA = new Label(1, new char[] { 'd' });
            Label threeB = new Label(2, new char[] { 'e' });
            Label threeC = new Label(3, new char[] { 'f' });

            List<ILabel> threeTk = new List<ILabel>() { threeA, threeB, threeC };

            Button threeBt = new Button("3");

            Key three = new Key(threeBt, threeTk);

            //key 0
            Label zeroA = new Label(1, new char[] { ' ' });     

            List<ILabel> zeroTk = new List<ILabel>() { zeroA };

            Button zeroBt = new Button("0");

            Key zero = new Key(zeroBt, zeroTk);

            //tnine keypad
            HashSet<IKey> keys = new HashSet<IKey>() { two,three,zero };

            KeyPad kp = new KeyPad(keys);

            string res3 = kp.command("abcdef");
            string res4 = kp.command("a cd f");

            Assert.AreEqual("2 22 2223 33 333", res3);
            Assert.AreEqual("2022230333", res4);
        }
    }

    [TestClass]
    public class FactoriesTest
    {

        [TestMethod]
        public void KeyFactoryTest()
        {
            KeyFactory kf = new KeyFactory();

            Key two=kf.CharKey("2", new List<char[]>() { new char[]{ 'a' }, new char[] { 'b' }, new char[] { 'c' } });
            Key three=kf.CharKey("3", new List<char[]>() { new char[] { 'd' }, new char[] { 'e' }, new char[] { 'f' } });
            Key zero=kf.CharKey("0", new List<char[]>() { new char[] { ' ' }});

            CollectionAssert.AreEqual(two.button.Name, new char[] { '2' });
            CollectionAssert.AreEqual(three.button.Name, new char[] { '3' });
            CollectionAssert.AreEqual(zero.button.Name, new char[] { '0' });


        }

        [TestMethod]
        public void KeypadFactoryTest()
        {
            KeyFactory kf = new KeyFactory();

            Key two = kf.CharKey("2", new List<char[]>() { new char[] { 'a' }, new char[] { 'b' }, new char[] { 'c' } });
            Key three = kf.CharKey("3", new List<char[]>() { new char[] { 'd' }, new char[] { 'e' }, new char[] { 'f' } });
            Key zero = kf.CharKey("0", new List<char[]>() { new char[] { ' ' } });

            KeyPadfactory kpf = new KeyPadfactory();
            HashSet<IKey> hs = new HashSet<IKey>() { two,three,zero};

            KeyPad kp = kpf.KeyPadTNine(hs);


            string res0=kp.command("ab cd ef");
            string res1=kp.command("a b e f");
            string res2=kp.command("abc def");
            string res3=kp.command("abcdef");

            Assert.AreEqual("2 2202223033 333", res0);
            Assert.AreEqual("20220330333", res1);
            Assert.AreEqual("2 22 22203 33 333", res2);
            Assert.AreEqual("2 22 2223 33 333", res3);
        }

    }



    [TestClass]
    public class KeyItemsFactoryTest
    {
        [TestMethod]
        public void KeyItemsFactoryCommandTest()
        {
            KeyItemsChar kc = KeyItemsFactory.keysChar();
                 
            string res= kc.command("a acd").commandToString();

            string res0 = kc.command("ab cd ef").commandToString();
            string res1 = kc.command("a b e f").commandToString();
            string res2 = kc.command("abc def").commandToString();
            string res3 = kc.command("abcdef").commandToString();

            Assert.AreEqual("202 2223",res);
            Assert.AreEqual("2 2202223033 333", res0);
            Assert.AreEqual("20220330333", res1);
            Assert.AreEqual("2 22 22203 33 333", res2);
            Assert.AreEqual("2 22 2223 33 333", res3);
        }
    }
}

