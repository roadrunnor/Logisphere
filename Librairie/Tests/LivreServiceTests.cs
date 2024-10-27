
using Xunit;
using Moq;
using Librairie.Entities;
using Librairie.Services;
using Librairie.Services.Interfaces;
using System;

namespace Librairie.Tests
{
    public class LivreServiceTests
    {
        private readonly Mock<IServiceBD> _mockServiceBD;
        private readonly LivreService _livreService;

        public LivreServiceTests()
        {
            _mockServiceBD = new Mock<IServiceBD>();
            _livreService = new LivreService(_mockServiceBD.Object);
        }

        [Fact]
        public void AcheterLivre_ClientExiste_LivreAchete()
        {
            // Arrange
            var clientId = Guid.NewGuid();
            var livreId = Guid.NewGuid();
            var montant = 20m;
            var client = new Client { Id = clientId };
            var livre = new Livre { Id = livreId, Quantite = 1, Prix = 20m };

            _mockServiceBD.Setup(s => s.ObtenirClient(clientId)).Returns(client);
            _mockServiceBD.Setup(s => s.ObtenirLivre(livreId)).Returns(livre);

            // Act
            var monnaieRendue = _livreService.AcheterLivre(clientId, livreId, montant);

            // Assert
            Assert.Equal(0m, monnaieRendue);
            Assert.Equal(0, livre.Quantite);
            Assert.Contains(livreId, client.ListeLivreAchete.Keys);
            _mockServiceBD.Verify(s => s.ModifierLivre(livre), Times.Once);
        }

        [Fact]
        public void AcheterLivre_ClientInexistant_Echoue()
        {
            // Arrange
            var clientId = Guid.NewGuid();
            var livreId = Guid.NewGuid();
            var montant = 20m;

            _mockServiceBD.Setup(s => s.ObtenirClient(clientId)).Returns((Client)null);

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => _livreService.AcheterLivre(clientId, livreId, montant));
        }

        [Fact]
        public void AcheterLivre_QuantiteInsuffisante_Echoue()
        {
            // Arrange
            var clientId = Guid.NewGuid();
            var livreId = Guid.NewGuid();
            var montant = 20m;
            var client = new Client { Id = clientId };
            var livre = new Livre { Id = livreId, Quantite = 0, Prix = 20m };

            _mockServiceBD.Setup(s => s.ObtenirClient(clientId)).Returns(client);
            _mockServiceBD.Setup(s => s.ObtenirLivre(livreId)).Returns(livre);

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => _livreService.AcheterLivre(clientId, livreId, montant));
        }

        [Fact]
        public void RembourserLivre_ClientEtLivreExistants_LivreRembourse()
        {
            // Arrange
            var clientId = Guid.NewGuid();
            var livreId = Guid.NewGuid();
            var client = new Client { Id = clientId };
            client.ListeLivreAchete[livreId] = 1;
            var livre = new Livre { Id = livreId, Quantite = 0, Prix = 20m };

            _mockServiceBD.Setup(s => s.ObtenirClient(clientId)).Returns(client);
            _mockServiceBD.Setup(s => s.ObtenirLivre(livreId)).Returns(livre);

            // Act
            var montantRembourse = _livreService.RembourserLivre(clientId, livreId);

            // Assert
            Assert.Equal(20m, montantRembourse);
            Assert.Equal(1, livre.Quantite);
            Assert.False(client.ListeLivreAchete.ContainsKey(livreId));
            _mockServiceBD.Verify(s => s.ModifierLivre(livre), Times.Once);
        }

        [Fact]
        public void RembourserLivre_ClientInexistant_Echoue()
        {
            // Arrange
            var clientId = Guid.NewGuid();
            var livreId = Guid.NewGuid();

            _mockServiceBD.Setup(s => s.ObtenirClient(clientId)).Returns((Client)null);

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => _livreService.RembourserLivre(clientId, livreId));
        }
    }
}
