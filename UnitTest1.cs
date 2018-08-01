using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

using System.Security.Cryptography;

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
            CollectionAssert.AreEqual(new char[] { 'b','b' }, res1);
            CollectionAssert.AreEqual(new char[] { 'c', 'd','e', 'c', 'd', 'e', 'c', 'd', 'e' }, res2);
        }
    }


    [TestClass]
    public class KeyTest
    {
        [TestMethod]
        public void KeyCommandTest() {

            char[] ch1 = new char[] { 'a' };
            char[] ch2 = new char[] { 'a' };
            string s1 = string.Join("", ch1);
            string s2 = string.Join("", ch2);
            int h1 = ch1.GetHashCode();
            int h2 = ch2.GetHashCode();
            int h21 = s1.GetHashCode();
            int h22 = s2.GetHashCode();

            byte[] bt1 = Encoding.UTF8.GetBytes(ch1);
            byte[] bt2 = Encoding.UTF8.GetBytes(ch2);
            int h31 = bt1.GetHashCode();
            int h32 = bt2.GetHashCode();

            using(MD5 m = MD5.Create())
            {
                byte[] h41 = m.ComputeHash(bt1);
                byte[] h42 = m.ComputeHash(bt2);
                
            }

            HashSet<char[]> hs1 = new HashSet<char[]>() { new char[] { 'a' }, new char[] { 'b' } , new char[] { 'a','b' } };
            bool a=hs1.Contains(new char[] { 'b' });

            Label twoA = new Label(1, new char[] { 'a' });
            Label twoB = new Label(1, new char[] { 'b' });

            List<ILabel> twoTk = new List<ILabel>() { twoA, twoB };

            Button twoBt = new Button("2");

            Key two = new Key(twoBt, twoTk);

            char[] cmd=two.GetLabel(new char[] { 'a' }).arr;

            Assert.IsNotNull(cmd);
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
           
            List<ILabel> twoTk = new List<ILabel>() { twoA, twoB };

            Button twoBt = new Button("2");

            Key two = new Key(twoBt, twoTk);

            List<IKey> keys = new List<IKey>() { two };

            KeyPad kp = new KeyPad(keys);

            char[] res0=kp.command('a');
            char[] res1=kp.command("b");

            CollectionAssert.AreEqual(new char[] { 'a' }, res0);
            CollectionAssert.AreEqual(new char[] { 'b','b' }, res1);    
        }
    }
}
