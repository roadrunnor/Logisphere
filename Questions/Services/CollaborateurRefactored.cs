namespace Questions.Services
{
    /**
     * Collaborateur.cs :
     * La classe Collaborateur est une classe statique avec une méthode AjouterContenuBD
     * qui communique avec une base de données. La classe ne peut pas être injectée directement
     * en tant que dépendance en raison de son caractère statique.
     * 
     */
    public class Collaborateur : ICollaborateur
    {
        public void AjouterContenuBD(string contenu)
        {
        }
    }
}
