
using System;
using Librairie.Entities;
using Librairie.Services.Interfaces;

namespace Librairie.Services
{
    public class ClientService : IServiceClient
    {
        private readonly IServiceBD _serviceBD;

        public ClientService(IServiceBD serviceBD)
        {
            _serviceBD = serviceBD;
        }

        public Client CreerClient(string nomClient)
        {
            if (string.IsNullOrWhiteSpace(nomClient))
            {
                throw new ArgumentException("Le nom d'utilisateur doit être renseigné.");
            }

            if (_serviceBD.ObtenirClient(nomClient) != null)
            {
                throw new ArgumentException("Le nom d'utilisateur est déjà utilisé par un autre client.");
            }

            var nouveauClient = new Client
            {
                Id = Guid.NewGuid(),
                NomUtilisateur = nomClient
            };

            _serviceBD.AjouterClient(nouveauClient);
            return nouveauClient;
        }

        public void RenommerClient(Guid clientId, string nouveauNomClient)
        {
            if (string.IsNullOrWhiteSpace(nouveauNomClient))
            {
                throw new ArgumentException("Le nouveau nom d'utilisateur doit être renseigné.");
            }

            var client = _serviceBD.ObtenirClient(clientId);
            if (client == null)
            {
                throw new InvalidOperationException("Le client n'existe pas.");
            }

            if (client.NomUtilisateur == nouveauNomClient)
            {
                throw new ArgumentException("Le nouveau nom d'utilisateur doit être différent de l'ancien.");
            }

            if (_serviceBD.ObtenirClient(nouveauNomClient) != null)
            {
                throw new ArgumentException("Le nom d'utilisateur est déjà utilisé par un autre client.");
            }

            client.NomUtilisateur = nouveauNomClient;
            _serviceBD.ModifierClient(client);
        }
    }
}
