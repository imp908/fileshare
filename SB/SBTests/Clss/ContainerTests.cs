using Microsoft.VisualStudio.TestTools.UnitTesting;
using SB_;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SB_.Tests
{
    [TestClass()]
    public class ContainerTests
    {
        [TestMethod()]
        public void ContainerTest()
        {
            Container container_ = new Container();
            int _expected = 7;
            int _actual = 0;
            try
            {
                _actual = (from s in container_.items select s).Count() + (from s in container_.operations select s).Count();
            }
            catch(Exception e)
            {
                System.Diagnostics.Trace.WriteLine(e.Message);
            }
            Assert.AreEqual<int>(_expected, _actual);
        }

        [TestMethod()]
        public void EquationReadTest()
        {
            
            Container container_ = new Container();
            List<ItemsStatus> ds = new List<ItemsStatus>();
            int expected_ = 0;
            int actual_ = 0;

            ds.Add(new ItemsStatus() { Equation= @"1+1", Items = 3});
            ds.Add(new ItemsStatus() { Equation = @"1+1=2", Items = 5});
            ds.Add(new ItemsStatus() { Equation = @"a=b", Items = 3 });
            ds.Add(new ItemsStatus() { Equation = @"a*2=b+1", Items = 7 });
            ds.Add(new ItemsStatus() { Equation = @"0.1*a*2+c^3=b+1", Items = 13 });

            foreach (ItemsStatus ds_ in ds)
            {
                container_.ParsedInit();
                container_.StringToItemParse(ds_.Equation);
                try
                {                   
                    ds_.Compare(container_.parsed.Count());
                }
                catch(Exception e)
                {
                    System.Diagnostics.Trace.WriteLine(e.Message);
                }
            }            
         
            Assert.IsFalse((from s in ds where s.Status == false select s).Any());

        }        

        [TestMethod()]
        public void OperationsCountTest()
        {
            Container container_ = new Container();
            int _expected = 4;
            int _actual = 0;
            
            try
            {
                _actual = (from s in container_.operations where s is IOpeartion_ select s).Count();
            }
            catch (NullReferenceException e)
            {
                System.Diagnostics.Trace.Write(e.Message);
            }
            catch (Exception e)
            {
                System.Diagnostics.Trace.Write(e.Message);
            }

            //Assert.Fail();
            //Assert.Inconclusive(@"Not all operations initialized");
            Assert.AreEqual<int>(_expected, _actual);

        }

        [TestMethod()]
        public void ItemCountTest()
        {
            Container container_ = new Container();
            int _expected = 3;
            int _actual = 0;

            try
            {
                _actual = (from s in container_.items select s).Count();
            }
            catch (NullReferenceException e)
            {
                System.Diagnostics.Trace.Write(e.Message);
            }
            catch (Exception e)
            {
                System.Diagnostics.Trace.Write(e.Message);
            }

            //Assert.Fail();
            //Assert.Inconclusive(@"Not all operations initialized");
            Assert.AreEqual<int>(_expected, _actual);
        }

        [TestMethod()]
        public void DuplicateDotsTest()
        {
            System.Exception actual_ = new Exception();
            DoubleDotsException expected_ = new DoubleDotsException();

            Container container_ = new Container();
            List<DotsStatus> checkStrings = new List<DotsStatus>()
            {
               new DotsStatus() { Equation = @"0..1", Status = false }
               ,new DotsStatus() { Equation = @"..1", Status = false }
               ,new DotsStatus() { Equation = @"0..", Status = false }
               ,new DotsStatus() { Equation = @"..", Status = false }
            };

            foreach (DotsStatus str_ in checkStrings)
            {
                try
                {
                    container_.StringToItemParse(str_.Equation);
                }
                catch (Exception e)
                {
                    if (e is DoubleDotsException)
                    {
                        str_.Status = true;
                    }
                }
            }

            if (!(from s in checkStrings where s.Status == false select s).Any())
            {
                actual_ = new DoubleDotsException();
            }

            Assert.AreEqual(actual_.GetType(), expected_.GetType());
        }      

    }
}