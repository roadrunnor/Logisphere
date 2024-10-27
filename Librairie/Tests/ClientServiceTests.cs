
using Xunit;
using Moq;
using Librairie.Entities;
using Librairie.Services;
using Librairie.Services.Interfaces;
using System;

namespace Librairie.Tests
{
    public class ClientServiceTests
    {
        private readonly Mock<IServiceBD> _mockServiceBD;
        private readonly ClientService _clientService;

        public ClientServiceTests()
        {
            _mockServiceBD = new Mock<IServiceBD>();
            _clientService = new ClientService(_mockServiceBD.Object);
        }

        [Fact]
        public void CreerClient_NomUtilisateurRenseigne_CreationReussie()
        {
            // Arrange
            var nomClient = "ClientUnique";
            _mockServiceBD.Setup(s => s.ObtenirClient(nomClient)).Returns((Client)null);

            // Act
            var clientCree = _clientService.CreerClient(nomClient);

            // Assert
            Assert.NotNull(clientCree);
            Assert.Equal(nomClient, clientCree.NomUtilisateur);
            _mockServiceBD.Verify(s => s.AjouterClient(It.IsAny<Client>()), Times.Once);
        }

        [Fact]
        public void CreerClient_NomUtilisateurDejaUtilise_Echoue()
        {
            // Arrange
            var nomClient = "ClientExistant";
            _mockServiceBD.Setup(s => s.ObtenirClient(nomClient)).Returns(new Client { NomUtilisateur = nomClient });

            // Act & Assert
            Assert.Throws<ArgumentException>(() => _clientService.CreerClient(nomClient));
        }

        [Fact]
        public void RenommerClient_ClientExisteEtNomChange_Reussi()
        {
            // Arrange
            var clientId = Guid.NewGuid();
            var nouveauNom = "NomNouveau";
            var clientExistant = new Client { Id = clientId, NomUtilisateur = "AncienNom" };
            _mockServiceBD.Setup(s => s.ObtenirClient(clientId)).Returns(clientExistant);
            _mockServiceBD.Setup(s => s.ObtenirClient(nouveauNom)).Returns((Client)null);

            // Act
            _clientService.RenommerClient(clientId, nouveauNom);

            // Assert
            Assert.Equal(nouveauNom, clientExistant.NomUtilisateur);
            _mockServiceBD.Verify(s => s.ModifierClient(clientExistant), Times.Once);
        }

        [Fact]
        public void RenommerClient_NomNonRenseigne_Echoue()
        {
            // Arrange
            var clientId = Guid.NewGuid();

            // Act & Assert
            Assert.Throws<ArgumentException>(() => _clientService.RenommerClient(clientId, ""));
        }

        [Fact]
        public void RenommerClient_ClientInexistant_Echoue()
        {
            // Arrange
            var clientId = Guid.NewGuid();
            var nouveauNom = "NomInconnu";
            _mockServiceBD.Setup(s => s.ObtenirClient(clientId)).Returns((Client)null);

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => _clientService.RenommerClient(clientId, nouveauNom));
        }
    }
}
