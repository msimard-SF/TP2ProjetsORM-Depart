using Microsoft.EntityFrameworkCore;
using ProjetsORM.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ProjetsORM.AccesDonnees
{
    public class EFProjetRepositoryTest
    {
        private EFProjetRepository repository;
        private Projet projet1;
        private Projet projet2;
        private Projet projet3;
        private Client client1;
        private Client client2;
        private Employe employe1;

        private void SetUp()
        {
            // Initialiser les objets nécessaires aux tests
            var builder = new DbContextOptionsBuilder<ProjetsORMContexte>();
            builder.UseInMemoryDatabase(databaseName: "projet_db");   // Database en mémoire
            var contexte = new ProjetsORMContexte(builder.Options);
            repository = new EFProjetRepository(contexte);

            //Initialiser données
            client1 = new Client()
            {
                NomClient = "ABC",
                NoEnregistrement = 1111,
                Ville = "Québec",
                CodePostal = "G3G1G1"
            };

            client2 = new Client()
            {
                NomClient = "XYZ",
                NoEnregistrement = 1100,
                Ville = "Québec",
                CodePostal = "G3G1G3"
            };

            contexte.Clients.Add(client1);
            contexte.Clients.Add(client2);

            employe1 = new Employe()
            {
                Nas = 123456789,
                Nom = "Lacasse",
                Prenom = "Bob",
                Sexe = 'M',
                DateEmbauche = Convert.ToDateTime("2009-09-09"),
                Fonction = "analyste"
            };

            contexte.Employes.Add(employe1);

            projet1 = new Projet()
            {
                NomClient = client1.NomClient,
                NomProjet = "TelIP",
                Budget = 10000,
                NoGestionnaire = employe1.NoEmploye
            };

            projet2 = new Projet()
            {
                NomClient = client1.NomClient,
                NomProjet = "Wifi",
                Budget = 15000,
                NoGestionnaire = employe1.NoEmploye
            };

            projet3 = new Projet()
            {
                NomClient = client2.NomClient,
                NomProjet = "Test",
                Budget = 50000,
                NoGestionnaire = employe1.NoEmploye
            };
        }

        [Fact]
        public void AjouterProjet()
        {
            // Arrange
            SetUp();
            
            // Act
            repository.AjouterProjet(projet1);

            // Assert
            Projet result = repository.RechercherProjet(projet1.NomProjet, projet1.NomClient);
            Assert.Same(expected: projet1, actual: result);
        }

        [Fact]
        public void ModifierProjet()
        {
            // Arrange
            SetUp();
            repository.AjouterProjet(projet1);
            projet1.Budget = 100000;

            // Act
            repository.ModifierProjet(projet1);

            // Assert
            Projet result = repository.RechercherProjet(projet1.NomProjet, projet1.NomClient);
            Assert.Equal(expected: projet1.Budget, actual: result.Budget);
        }

        [Fact]
        public void SupprimerProjet()
        {
            // Arrange
            SetUp();
            repository.AjouterProjet(projet1);

            // Act
            repository.SupprimerProjet(projet1);

            // Assert
            Projet result = repository.RechercherProjet(projet1.NomProjet, projet1.NomClient);
            Assert.Null(result);
        }

        [Fact]
        public void RechercherProjet()
        {
            SetUp();
            repository.AjouterProjet(projet1);
            repository.AjouterProjet(projet2);

            // Act
            Projet result = repository.RechercherProjet(projet2.NomProjet, projet2.NomClient);

            // Assert
            Assert.Same(expected: projet2, actual: result);

        }

        [Fact]
        public void RechercherProjetParClient()
        {
            SetUp();
            repository.AjouterProjet(projet1);
            repository.AjouterProjet(projet2);
            repository.AjouterProjet(projet3);

            // Act
            ICollection<Projet> result = repository.RechercherProjetsParClient(client1);

            // Assert
            Assert.Equal(expected: 2, actual: result.Count());
            Assert.True(result.Contains(projet1) && result.Contains(projet2));
        }

        [Fact]
        public void RechercherTotalBudgetParClient()
        {
            SetUp();
            repository.AjouterProjet(projet1);
            repository.AjouterProjet(projet2);
            repository.AjouterProjet(projet3);

            // Act
            decimal? result = repository.RechercherTotalBudgetParClient(client1);

            // Assert
            Assert.Equal(expected: projet1.Budget + projet2.Budget, actual: result);
        }

        [Fact]
        public void RechercherMoyenneBudgetParClient()
        {
            SetUp();
            repository.AjouterProjet(projet1);
            repository.AjouterProjet(projet2);
            repository.AjouterProjet(projet3);

            // Act
            decimal? result = repository.RechercherMoyenneBudgetParClient(client1);

            // Assert
            Assert.Equal(expected: (projet1.Budget + projet2.Budget) / 2, actual: result);
        }
    }
}
