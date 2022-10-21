using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Win32;
using System;
using System.IO;

namespace GreenHorn.UnitTests
{
    [TestClass]
    public class ValidationsTest
    {
        [TestMethod]
        public void IsNameValid_IsMatch_ReturnTrue() // method_scenario_expectedBehavior()
        {
            //arrange
            string test = "John Smith";
            string error = "error";
            //act
            var result = Validations.IsNameValid(test, out  error);
            //assert
            Assert.IsTrue(result);
        }
        [TestMethod]
        public void IsNameValid_NotMatch_ReturnFalse()
        {
            string test = "`**_Reaper5000_**#78GyJ(22@)`";
            string error = "";

            var result = Validations.IsNameValid(test, out error);
            Assert.IsFalse(result);
        }
        [TestMethod]
        public void IsYearValid_Below1900_ReturnFalse()
        {
            DateTime date = new DateTime(); //set to 01/01/0001
            string error = "";

            var result = Validations.IsYearValid(date, out error);
            Assert.IsFalse(result);
        }
        [TestMethod]
        public void IsYearValid_InRange_ReturnTrue()
        {
            DateTime date = new DateTime(2022, DateTime.Now.Month, 1);
            string error = "";

            var result = Validations.IsYearValid(date, out error);
            Assert.IsTrue(result);
        }
        [TestMethod]
        public void IsYearValid_Over2100_ReturnFalse()
        {
            DateTime date = new DateTime(2101, DateTime.Now.Month, 1);
            string error = "";

            var result = Validations.IsYearValid(date, out error);
            Assert.IsFalse(result);
        }
        [TestMethod]
        public void IsEmailValid_BadStructure_ReturnFalse()
        {
            string email = "anything@anything_anything";
            string error = "";

            var result = Validations.IsEmailValid(email, out error);
            Assert.IsFalse(result);
        }
        [TestMethod]
        public void IsPhoneValid_BadStructure_ReturnFalse()
        {
            string phone = "514 521 r 52145";
            string error = "";

            var result = Validations.IsPhoneValid(phone, out error);
            Assert.IsFalse(result);
        }
        [TestMethod]
        public void IsPhoneValid_GoodStructure_ReturnTrue()
        {
            string phone = "(514) 521-5214";
            string error = "";

            var result = Validations.IsPhoneValid(phone, out error);
            Assert.IsTrue(result);
        }
        [TestMethod]
        public void IsAddressValid_TooShort_ReturnFalse()
        {
            string address = "Yup";
            string error = "";

            var result = Validations.IsAddressValid(address, out error);
            Assert.IsFalse(result);
        }
        [TestMethod]
        public void IsPositionNameValid_Nope_ReturnFalse()
        {
            string name = "#678 987 6554";
            string error = "";

            var result = Validations.IsPositionNameValid(name, out error);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsPositionNameValid_TooManyCharacters_ReturnFalse()
        {
            string name = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit.Aenean commodo ligula eget dolor. Aenean massa. Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus.";
            string error = "";

            var result = Validations.IsPositionNameValid(name, out error);
            Assert.IsFalse(result);
        }
    }
}
