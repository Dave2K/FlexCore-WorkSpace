using Xunit;
using Moq;
using FlexCore.Database.Interfaces;

namespace FlexCore.Database.Core.Tests
{
    /// <summary>
    /// Test per verificare il corretto funzionamento della classe UnitOfWorkBase.
    /// </summary>
    public class UnitOfWorkBaseTests
    {
        /// <summary>
        /// Verifica che il metodo Dispose() elimini correttamente il contesto dati.
        /// </summary>
        [Fact]
        public void Dispose_ShouldDisposeDataContext()
        {
            // Arrange: Crea un mock del contesto dati e un'istanza di UnitOfWork
            var mockDataContext = new Mock<IDataContext>();
            var unitOfWork = new ConcreteUnitOfWork(mockDataContext.Object);

            // Act: Chiama Dispose() sull'UnitOfWork
            unitOfWork.Dispose();

            // Assert: Verifica che Dispose() del contesto dati sia stato chiamato una volta
            mockDataContext.Verify(c => c.Dispose(), Times.Once);
        }

        /// <summary>
        /// Verifica che CommitTransaction() deleghi correttamente l'operazione al contesto dati.
        /// </summary>
        [Fact]
        public void CommitTransaction_ShouldCallDataContext()
        {
            // Arrange: Crea un mock del contesto dati e un'istanza di UnitOfWork
            var mockDataContext = new Mock<IDataContext>();
            var unitOfWork = new ConcreteUnitOfWork(mockDataContext.Object);

            // Act: Esegui CommitTransaction()
            unitOfWork.CommitTransaction();

            // Assert: Verifica che CommitTransaction() del contesto dati sia stato chiamato
            mockDataContext.Verify(c => c.CommitTransaction(), Times.Once);
        }

        // Implementazione concreta di UnitOfWorkBase per i test
        private class ConcreteUnitOfWork : UnitOfWorkBase
        {
            public ConcreteUnitOfWork(IDataContext dataContext) : base(dataContext) { }
        }
    }
}