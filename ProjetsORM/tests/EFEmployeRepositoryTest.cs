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
    public class EFEmployeRepositoryTest
    {
        private EFEmployeRepository repository;
        private Employe employe1;
        private Employe employe2;
        private Employe superviseur1;
        private Employe superviseur2;
        private ProjetsORMContexte ctx;

        private void SetUp()
        {
            // Initialiser les objets nécessaires aux tests
            var builder = new DbContextOptionsBuilder<ProjetsORMContexte>();
            builder.UseInMemoryDatabase(databaseName: "employe_db");   // Database en mémoire
            var contexte = new ProjetsORMContexte(builder.Options);
            repository = new EFEmployeRepository(contexte);
            ctx = contexte;

            //Initialiser données
            superviseur1 = new Employe()
            {
                Nas = 123456777,
                Nom = "Labrecque",
                Prenom = "Julie",
                Sexe = 'F',
                DateEmbauche = Convert.ToDateTime("2007-09-09"),
                Fonction = "directeur"
            };

            superviseur2 = new Employe()
            {
                Nas = 123456799,
                Nom = "Lachance",
                Prenom = "Pierre",
                Sexe = 'M',
                DateEmbauche = Convert.ToDateTime("2000-09-09"),
                Fonction = "directeur"
            };

        }

        [Fact]
        public void AjouterEmploye()
        {
            // Arrange
            SetUp();
            
            // Act
            repository.AjouterEmploye(superviseur1);

            // Assert
            Employe result = repository.RechercherEmployeParID(superviseur1.NoEmploye);
            Assert.Same(expected: superviseur1, actual: result);
        }

        [Fact]
        public void ModifierEmploye()
        {
            // Arrange
            SetUp();
            repository.AjouterEmploye(superviseur1);
            superviseur1.Salaire = 125000;

            // Act
            repository.ModifierEmploye(superviseur1);

            // Assert
            Employe result = repository.RechercherEmployeParID(superviseur1.NoEmploye);
            Assert.Equal(expected: superviseur1.Salaire, actual: result.Salaire);
        }

        [Fact]
        public void SupprimerEmploye()
        {
            // Arrange
            SetUp();
            repository.AjouterEmploye(superviseur1);

            // Act
            repository.SupprimerEmploye(superviseur1);

            // Assert
            Employe result = repository.RechercherEmployeParID(superviseur1.NoEmploye);
            Assert.Null(result);
        }

        [Fact]
        public void RechercherTousEmployes()
        {
            SetUp();
            repository.AjouterEmploye(superviseur1);
            repository.AjouterEmploye(superviseur2);

            // Act
            ICollection<Employe> result = repository.RechercherTousEmployes();

            // Assert
            Assert.Equal(expected: 2, actual: result.Count());
            Assert.True(result.Contains(superviseur1));
            Assert.True(result.Contains(superviseur2));

        }

        [Fact]
        public void RechercherEmployeParID()
        {
            SetUp();
            repository.AjouterEmploye(superviseur1);
            repository.AjouterEmploye(superviseur2);

            // Act
            Employe result = repository.RechercherEmployeParID(superviseur2.NoEmploye);

            // Assert
            Assert.Same(expected: superviseur2, actual: result);
        }

        [Fact]
        public void RechercherEmployeParNom()
        {
            SetUp();
            repository.AjouterEmploye(superviseur1);
            repository.AjouterEmploye(superviseur2);

            // Act
            ICollection<Employe> result = repository.RechercherEmployeParNom(superviseur2.Nom, superviseur2.Prenom);

            // Assert
            Assert.True(result.Contains(superviseur2));
            Assert.Single(result);
        }

        [Fact]
        public void RechercherSuperviseurs()
        {
            SetUp();
            repository.AjouterEmploye(superviseur1);
            repository.AjouterEmploye(superviseur2);

            employe1 = new Employe()
            {
                Nas = 123456789,
                Nom = "Lacasse",
                Prenom = "Bob",
                Sexe = 'M',
                DateEmbauche = Convert.ToDateTime("2009-09-09"),
                Fonction = "analyste",
                NoSuperviseur = superviseur1.NoEmploye
            };

            employe2 = new Employe()
            {
                Nas = 123456788,
                Nom = "Labrosse",
                Prenom = "Bob",
                Sexe = 'M',
                DateEmbauche = Convert.ToDateTime("2011-09-09"),
                Fonction = "analyste",
                NoSuperviseur = superviseur2.NoEmploye
            };

            repository.AjouterEmploye(employe1);
            repository.AjouterEmploye(employe2);

            // Act
            ICollection<Employe> result = repository.RechercherTousSuperviseurs();

            // Assert
            Assert.Equal(expected: 2, actual: result.Count());
            Assert.True(result.Contains(superviseur1) && result.Contains(superviseur2));
        }

        [Fact]
        public void RechercherEmployesSupervises()
        {
            SetUp();
            repository.AjouterEmploye(superviseur1);
            repository.AjouterEmploye(superviseur2);

            employe1 = new Employe()
            {
                Nas = 123456789,
                Nom = "Lacasse",
                Prenom = "Bob",
                Sexe = 'M',
                DateEmbauche = Convert.ToDateTime("2009-09-09"),
                Fonction = "analyste",
                NoSuperviseur = superviseur1.NoEmploye
            };

            employe2 = new Employe()
            {
                Nas = 123456788,
                Nom = "Labrosse",
                Prenom = "Bob",
                Sexe = 'M',
                DateEmbauche = Convert.ToDateTime("2011-09-09"),
                Fonction = "analyste",
                NoSuperviseur = superviseur2.NoEmploye
            };

            repository.AjouterEmploye(employe1);
            repository.AjouterEmploye(employe2);

            // Act
            ICollection<Employe> result = repository.RechercherEmployesSupervises(superviseur1);

            // Assert
            Assert.Single(result);
            Assert.True(result.Contains(employe1));
        }
    }
}
