
using System;
using Librairie.Entities;
using Librairie.Services.Interfaces;

namespace Librairie.Services
{
    public class LivreService : IServiceLivre
    {
        private readonly IServiceBD _serviceBD;

        public LivreService(IServiceBD serviceBD)
        {
            _serviceBD = serviceBD;
        }

        public decimal AcheterLivre(Guid IdClient, Guid IdLivre, decimal montant)
        {
            var client = _serviceBD.ObtenirClient(IdClient);
            if (client == null)
                throw new InvalidOperationException("Le client n'existe pas.");

            var livre = _serviceBD.ObtenirLivre(IdLivre);
            if (livre == null)
                throw new InvalidOperationException("Le livre n'existe pas.");

            if (montant <= 0)
                throw new ArgumentException("Le montant doit être supérieur à 0.");

            if (livre.Quantite <= 0)
                throw new InvalidOperationException("Le livre est en rupture de stock.");

            if (montant < livre.Prix)
                throw new ArgumentException("Le montant payé est insuffisant pour acheter ce livre.");

            livre.Quantite -= 1;
            _serviceBD.ModifierLivre(livre);

            if (!client.ListeLivreAchete.ContainsKey(IdLivre))
                client.ListeLivreAchete[IdLivre] = 0;
            client.ListeLivreAchete[IdLivre] += 1;

            return montant - livre.Prix;
        }

        public decimal RembourserLivre(Guid IdClient, Guid idLivre)
        {
            var client = _serviceBD.ObtenirClient(IdClient);
            if (client == null)
                throw new InvalidOperationException("Le client n'existe pas.");

            var livre = _serviceBD.ObtenirLivre(idLivre);
            if (livre == null)
                throw new InvalidOperationException("Le livre n'existe pas.");

            if (!client.ListeLivreAchete.ContainsKey(idLivre) || client.ListeLivreAchete[idLivre] <= 0)
                throw new InvalidOperationException("Le client n'a pas acheté ce livre.");

            client.ListeLivreAchete[idLivre] -= 1;
            if (client.ListeLivreAchete[idLivre] == 0)
                client.ListeLivreAchete.Remove(idLivre);

            livre.Quantite += 1;
            _serviceBD.ModifierLivre(livre);

            return livre.Prix;
        }
    }
}
