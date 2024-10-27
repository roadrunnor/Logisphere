using System;
using System.Collections.Generic;
using System.Linq;

namespace Questions
{
    // Améliorations apportées :

    /**
     * Question.cs : 
     * La classe Question utilise la classe Collaborateur pour ajouter du contenu à une base de données.
     * La méthode Traiter a une logique complexe avec plusieurs variables locales pour valider et ajouter le contenu.
     * La validation utilise des messages d'erreur et vérifie la longueur du contenu.
     * La classe pourrait être améliorée pour faciliter la lecture, réduire la complexité et introduire l'injection 
     * de dépendance pour Collaborateur.
     * 
     * Ré-usinage proposé :
     * Transformation de Collaborateur pour l'Injection de Dépendance :
     * Convertir Collaborateur en une classe non statique et créer une interface ICollaborateur pour 
     * permettre l’injection de dépendance, ce qui facilite la testabilité de Question.
     * 
     * Amélioration de Question : 
     * Injecter ICollaborateur dans la classe Question.
     * Simplifier la méthode Traiter en réduisant la complexité et en optimisant 
     * la gestion des variables locales.
     * Améliorer la méthode Valider en la rendant indépendante des effets de bord (éviter 
     * de modifier message dans Valider).
     * 
     */
    public class Question(ICollaborateur collaborateur)
    {
        // Injection de dépendance pour ICollaborateur pour rendre la classe testable et découplée
        private readonly ICollaborateur _collaborateur = collaborateur;

        public void Traiter(List<string> listeContenu)
        {
            // Validation de la liste pour s'assurer qu'elle n'est ni null ni vide
            if (listeContenu == null || listeContenu.Count == 0)
                throw new ArgumentException("La liste de contenu ne peut être vide.");

            var listeContenuValide = new List<string>();

            // Boucle pour valider et traiter chaque élément de la liste
            foreach (var contenu in listeContenu)
            {
                // Utilisation de la méthode Valider qui retourne un bool et un message d'erreur via un paramètre de sortie
                if (Valider(contenu, out string message))
                {
                    // Limite le contenu à une longueur maximale de 10 caractères
                    var contenuCourt = contenu[..Math.Min(10, contenu.Length)];

                    // Ajout du contenu validé à la liste des contenus valides s'il n'est pas déjà présent
                    if (!listeContenuValide.Contains(contenuCourt))
                    {
                        listeContenuValide.Add(contenuCourt);
                    }
                }
                else
                {
                    // Lance une exception avec le message d'erreur si la validation échoue
                    throw new Exception(message);
                }
            }

            // Envoie chaque contenu validé à la méthode AjouterContenuBD de ICollaborateur
            listeContenuValide.ForEach(x => _collaborateur.AjouterContenuBD(x));
        }

        // Méthode de validation améliorée avec un paramètre de sortie pour le message d'erreur
        private static bool Valider(string contenu, out string message)
        {
            message = string.Empty;

            // Validation si le contenu est vide ou null
            if (string.IsNullOrWhiteSpace(contenu))
            {
                message = "Le contenu ne peut être vide";
                return false;
            }

            // Validation si le contenu dépasse 10 caractères
            if (contenu.Length > 10)
            {
                message = "Le contenu est trop long";
                return false;
            }

            // Retourne true si toutes les validations sont réussies
            return true;
        }
    }
}
