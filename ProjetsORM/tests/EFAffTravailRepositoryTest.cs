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
    public class EFAffTravailRepositoryTest
    {
        private EFAffTravailRepository repository;
        private Employe employe1;
        private Employe employe2;
        private Projet projet1;
        private Client client1;
        private Projet projet2;

        private void SetUp()
        {
            // Initialiser les objets nécessaires aux tests
            var builder = new DbContextOptionsBuilder<ProjetsORMContexte>();
            builder.UseInMemoryDatabase(databaseName: "affTrav_db");   // Database en mémoire
            var contexte = new ProjetsORMContexte(builder.Options);
            repository = new EFAffTravailRepository(contexte);

            //Initialiser données
            client1 = new Client()
            {
                NomClient = "ABC",
                NoEnregistrement = 1111,
                Ville = "Québec",
                CodePostal = "G3G1G1"
            };

            contexte.Clients.Add(client1);

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

            employe2 = new Employe()
            {
                Nas = 123456788,
                Nom = "Labrosse",
                Prenom = "Bob",
                Sexe = 'M',
                DateEmbauche = Convert.ToDateTime("2011-09-09"),
                Fonction = "analyste"
            };

            contexte.Employes.Add(employe2);

            projet1 = new Projet()
            {
                NomClient = client1.NomClient,
                NomProjet = "TelIP",
                Budget = 10000,
                NoGestionnaire = employe1.NoEmploye
            };

            contexte.Projets.Add(projet1);

            projet2 = new Projet()
            {
                NomClient = client1.NomClient,
                NomProjet = "Wifi",
                Budget = 15000,
                NoGestionnaire = employe1.NoEmploye
            };

            contexte.Projets.Add(projet2);

        }

        [Fact]
        public void AjouterAffTravail()
        {
            // Arrange
            SetUp();
            AffectationTravail affectation = new AffectationTravail()
            {
                NoEmploye = employe1.NoEmploye,
                NomProjet = projet1.NomProjet,
                NomClient = projet1.NomClient,
                DateAffectation = Convert.ToDateTime("2020-01-02")
            };

            // Act
            repository.AjouterAffectation(affectation);

            // Assert
            ICollection<AffectationTravail> result = repository.RechercherAffectationEmploye(employe1);
            Assert.True(result.Contains(affectation));
        }

        [Fact]
        public void ModifierAffTravail()
        {
            // Arrange
            SetUp();
            AffectationTravail affectation = new AffectationTravail()
            {
                NoEmploye = employe1.NoEmploye,
                NomProjet = projet1.NomProjet,
                NomClient = projet1.NomClient,
                DateAffectation = Convert.ToDateTime("2020-01-02")
            };

            repository.AjouterAffectation(affectation);

            // Act
            affectation.DateAffectation = Convert.ToDateTime("2020-02-01");
            repository.ModifierAffectation(affectation);

            // Assert
            ICollection<AffectationTravail> result = repository.RechercherAffectationEmploye(employe1);
            Assert.Equal(expected : affectation.DateAffectation, actual : result.ElementAt(0).DateAffectation);
        }

        [Fact]
        public void SupprimerAffTravail()
        {
            // Arrange
            SetUp();
            AffectationTravail affectation = new AffectationTravail()
            {
                NoEmploye = employe1.NoEmploye,
                NomProjet = projet1.NomProjet,
                NomClient = projet1.NomClient,
                DateAffectation = Convert.ToDateTime("2020-01-02")
            };

            repository.AjouterAffectation(affectation);

            // Act
            repository.SupprimerAffectation(affectation);

            // Assert
            ICollection<AffectationTravail> result = repository.RechercherAffectationEmploye(employe1);
            Assert.Empty(result);
        }

        [Fact]
        public void RechercherAffTravailParProjet()
        {
            SetUp();
            AffectationTravail affectation = new AffectationTravail()
            {
                NoEmploye = employe1.NoEmploye,
                NomProjet = projet1.NomProjet,
                NomClient = projet1.NomClient,
                DateAffectation = Convert.ToDateTime("2020-01-02")
            };

            AffectationTravail affectation2 = new AffectationTravail()
            {
                NoEmploye = employe1.NoEmploye,
                NomProjet = projet2.NomProjet,
                NomClient = projet2.NomClient,
                DateAffectation = Convert.ToDateTime("2020-01-02")
            };

            repository.AjouterAffectation(affectation);
            repository.AjouterAffectation(affectation2);

            // Act
            ICollection<AffectationTravail> result = repository.RechercherAffectationProjet(projet1);

            // Assert
            Assert.True(result.Contains(affectation));
            Assert.False(result.Contains(affectation2));
        }

        [Fact]
        public void RechercherAffTravailParEmploye()
        {
            SetUp();
            AffectationTravail affectation = new AffectationTravail()
            {
                NoEmploye = employe1.NoEmploye,
                NomProjet = projet1.NomProjet,
                NomClient = projet1.NomClient,
                DateAffectation = Convert.ToDateTime("2020-01-02")
            };

            AffectationTravail affectation2 = new AffectationTravail()
            {
                NoEmploye = employe2.NoEmploye,
                NomProjet = projet2.NomProjet,
                NomClient = projet2.NomClient,
                DateAffectation = Convert.ToDateTime("2020-01-02")
            };

            repository.AjouterAffectation(affectation);
            repository.AjouterAffectation(affectation2);

            // Act
            ICollection<AffectationTravail> result = repository.RechercherAffectationEmploye(employe1);

            // Assert
            Assert.True(result.Contains(affectation));
            Assert.False(result.Contains(affectation2));

        }
    }
}
