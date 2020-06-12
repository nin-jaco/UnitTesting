using System;
using NUnit.Framework;
using TestNinja.Fundamentals;

namespace TestNinja.UnitTests
{
    [TestFixture]
    public class ReservationTests
    {
        [Test]
        public void CanBeCancelledBy_UserIsAdmin_ReturnsTrue()
        {
            //arrange
            var reservation = new Reservation();

            //act
            var result = reservation.CanBeCancelledBy(new User {IsAdmin = true});

            //assert
            Assert.That(result, Is.True);
        }

        [Test]
        public void CanBeCancelledBy_UserIsCreator_ReturnsTrue()
        {
            //arrange
            var user = new User();
            var reservation = new Reservation{MadeBy = user };
            
            //act
            var result = reservation.CanBeCancelledBy(user);
            //assert
            Assert.IsTrue(result);
        }

        [Test]
        public void CanBeCalledBy_AnotherUserCancellingReservation_ReturnsFalse()
        {
            var reservation = new Reservation{MadeBy = new User()};
            var result = reservation.CanBeCancelledBy(new User());
            Assert.IsFalse(result);

        }
    }
}
