using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

using tNine;
namespace TnineTests
{
    
    [TestClass]
    public class TokenTest
    {

        [TestMethod]
        public void TokenArrTest()
        {
            char[] ch = new char['a'];
            int id = 2;
            Token tk = new Token(id, new char['a']);
            
            Assert.IsTrue(ch.ToString().Equals(tk.arr.ToString()));
            Assert.AreEqual(id, tk.order);
        }

       
    }

    [TestClass]
    public class KeyTest
    {
        [TestMethod]
        public void KeyCommandTest() {
            Token twoA = new Token(1, new char['a']);
            Token twoB = new Token(1, new char['b']);

            Button twoBt = new Button("2");

            List<IToken> twoTk = new List<IToken>() { twoA, twoB };

            Key two = new Key(twoBt, twoTk);

            char[] cmd =two.command("b");

            Assert.Fail();
        }
    }
    [TestClass]
    public class KeyPadTest
    {

        [TestMethod]
        public void KeyPadCommandTest()
        {
            Token twoA = new Token(1, new char['a']);
            Token twoB = new Token(1, new char['b']);

            Button twoBt = new Button("2");

            List<IToken> twoTk = new List<IToken>() { twoA, twoB };

            Key two = new Key(twoBt, twoTk);

            List<IKey> keys = new List<IKey>() { two };

            KeyPad kp = new KeyPad(keys);

            Assert.Fail();
        }
    }
}
