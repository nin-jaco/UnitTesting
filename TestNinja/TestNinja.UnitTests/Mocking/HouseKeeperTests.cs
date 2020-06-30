using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using NUnit.Framework;
using TestNinja.Mocking;

namespace TestNinja.UnitTests.Mocking
{
    [TestFixture]
    public class HouseKeeperTests
    {
        private HousekeeperService _service;
        private Mock<IStatementGenerator> _statementGeneratopr;
        private Mock<IEmailSender> _emailSender;
        private Mock<HousekeeperService.IXtraMessageBox> _messageBox;
        private readonly DateTime _statementDate = new DateTime(2017,1,1);
        private HousekeeperService.Housekeeper _housekeeper;
        private string _filename;

        [SetUp]
        public void SetUp()
        {
            _housekeeper = new HousekeeperService.Housekeeper
            {
                Email = "a",
                FullName = "b",
                Oid = 1, 
                StatementEmailBody = "c"
            };
            var unitOfWork = new Mock<IUnitOfWork>();
            unitOfWork.Setup(uow => uow.Query<HousekeeperService.Housekeeper>()).Returns(
                new List<HousekeeperService.Housekeeper>
                {
                    _housekeeper
                }.AsQueryable());
            
            _statementGeneratopr = new Mock<IStatementGenerator>();
            _statementGeneratopr.Setup(sg => sg.SaveStatement(_housekeeper.Oid, _housekeeper.FullName, _statementDate)).Returns(() => _filename);
            
            _emailSender = new Mock<IEmailSender>();
            _messageBox = new Mock<HousekeeperService.IXtraMessageBox>();
            
            _service = new HousekeeperService(unitOfWork.Object, _statementGeneratopr.Object, _emailSender.Object, _messageBox.Object);
        }
        
        [Test]
        public void SendStatementEmails_WhenCalled_GenerateStatements()
        {
            
            _service.SendStatementEmails(_statementDate);
            _statementGeneratopr.Verify(sg => sg.SaveStatement(_housekeeper.Oid, _housekeeper.FullName, _statementDate));
        }

        [Test] 
        public void SendStatementEmails_HousekeepersEmailIsNull_ShouldNotGenerateStatements()
        {
            _housekeeper.Email = null;
            _service.SendStatementEmails(_statementDate);
            //_statementGeneratopr.Verify(sg => sg.SaveStatement(_housekeeper.Oid, _housekeeper.FullName, _statementDate), Times.Never);
            VerifyEmailSent();
        }
        
        [Test] 
        public void SendStatementEmails_HousekeepersEmailIsWhiteSpace_ShouldNotGenerateStatements()
        {
            _housekeeper.Email = " ";
            _service.SendStatementEmails(_statementDate);
            //_statementGeneratopr.Verify(sg => sg.SaveStatement(_housekeeper.Oid, _housekeeper.FullName, _statementDate), Times.Never);
            VerifyEmailSent();
        }
        
        [Test] 
        public void SendStatementEmails_HousekeepersEmailIsEmpty_ShouldNotGenerateStatements()
        {
            _housekeeper.Email = "";
            _service.SendStatementEmails(_statementDate);
            VerifyEmailSent();
        }

        private void VerifyEmailSent()
        {
            _statementGeneratopr.Verify(sg => sg.SaveStatement(_housekeeper.Oid, _housekeeper.FullName, _statementDate),
                Times.Never);
        }

        [Test] 
        public void SendStatementEmails_IfStatementIsGenerated_ShouldEmailStatements()
        {
            
            _statementGeneratopr.Setup(sg => sg.SaveStatement(_housekeeper.Oid, _housekeeper.FullName, _statementDate)).Returns(_filename);
            _service.SendStatementEmails(_statementDate);
            _emailSender.Verify(es => es.EmailFile(_housekeeper.Email, _housekeeper.StatementEmailBody, _filename, It.IsAny<string>()));
        }
        
        [Test] 
        public void SendStatementEmails_IfFilenameIsNull_ShouldNotEmailStatement()
        {
            _filename = null;
            
            _service.SendStatementEmails(_statementDate);
            VerifyEmailNotSent();
        }

        private void VerifyEmailNotSent()
        {
            _emailSender.Verify(
                es => es.EmailFile(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()),
                Times.Never);
        }

        [Test] 
        public void SendStatementEmails_IfFilenameIsEmptyString_ShouldNotEmailStatement()
        {
            _filename = "";
            //_statementGeneratopr.Setup(sg => sg.SaveStatement(_housekeeper.Oid, _housekeeper.FullName, _statementDate)).Returns("");
            _service.SendStatementEmails(_statementDate);
            VerifyEmailNotSent();
        }
        
        [Test] 
        public void SendStatementEmails_IfFilenameIsWhiteSpace_ShouldNotEmailStatement()
        {
            _filename = " ";
            //_statementGeneratopr.Setup(sg => sg.SaveStatement(_housekeeper.Oid, _housekeeper.FullName, _statementDate)).Returns(" ");
            _service.SendStatementEmails(_statementDate);
            //_emailSender.Verify(es => es.EmailFile(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
            VerifyEmailNotSent();
        }
        
        [Test] 
        public void SendStatementEmails_EmailSendingFails_DisplayMessageBox()
        {
            _emailSender.Setup(es => es.EmailFile(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>()
                )).Throws<Exception>();
            _service.SendStatementEmails(_statementDate);

            _messageBox.Verify(mb => mb.Show(It.IsAny<string>(), It.IsAny<string>(), HousekeeperService.MessageBoxButtons.OK));
        }
    }
}