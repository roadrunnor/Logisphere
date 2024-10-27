# RÉPONSES ET BONUS
# Définition du Ré-Usinage
Le ré-usinage est le processus de restructuration du code source d'une application sans modifier son comportement externe. Son objectif est d'améliorer la lisibilité, la maintenabilité et la performance du code sans introduire de nouveaux bugs. Ce processus consiste souvent à simplifier les structures de code, réduire la redondance, améliorer les noms de variables et appliquer des principes de conception pour une meilleure modularité.

# Objectifs du Ré-Usinage
- Améliorer la Lisibilité : Rendre le code plus compréhensible pour les développeurs, ce qui facilite la maintenance et de futures modifications.
- Réduire la Redondance : Supprimer les duplications de code pour minimiser les risques d’erreurs et faciliter les mises à jour.
- Optimiser la Performance : Identifier les goulots d’étranglement et restructurer le code pour une meilleure efficacité.
- Suivre les Bonnes Pratiques : Appliquer les principes SOLID, SRP, et autres bonnes pratiques pour assurer la qualité du code.

# Réponse à la Question Bonus
Pour garantir l'intégrité des comptes de quantités lors des ajustements pour les fonctions AcheterLivre et RembourserLivre, une transaction atomique devrait être utilisée. En cas de problème technique, les deux ajustements (réduction ou augmentation de la quantité de livres disponibles et d'exemplaires pour le client) seraient exécutés comme une seule unité de travail. Une transaction atomique suit un concept de base de données et de programmation qui garantit qu'un ensemble d'opérations est traité comme une unité indivisible. Cela signifie que toutes les opérations incluses dans la transaction réussissent ou échouent ensemble. Si une erreur survient au cours d'une transaction, toutes les opérations sont annulées, et l'état initial est restauré, assurant ainsi l'intégrité des données. Autrement dit, toutes les opérations doivent réussir pour que la transaction soit validée, les opérations en cours sont isolées des autres transactions, évitant les interférences et garantissant que le résultat final est indépendant des transactions simultanées. Une fois la transaction validée les changements sont permanents, même en cas de panne, autrement on parlera de "rollback".

# Exemple appliqué à la Partie Bonus
Dans le contexte de l'achat et du remboursement d'un livre, une transaction atomique garantirait que :

- Lors de l'achat d'un livre : La réduction de la quantité de livres disponibles à la vente et l'ajout de l'exemplaire au compte du client sont traités comme une seule opération. Si l'une de ces étapes échoue, les deux actions vont être annulées.

- Lors du remboursement : L'ajustement de la quantité de livres disponibles et la suppression de l'exemplaire vendu du compte client se produisent ensemble. En cas de problème, aucun changement n'est appliqué. Cela permet d'éviter des incohérences de données (par exemple, des exemplaires qui manquent en inventaire sans qu'ils aient été attribués à un client).

- Cela pourrait être réalisé avec :
Une transaction de base de données (si les opérations sont stockées en base de données).
Un bloc transactionnel dans le code pour s'assurer que les ajustements se font ensemble ou pas du tout, réduisant ainsi le risque d'incohérences.
