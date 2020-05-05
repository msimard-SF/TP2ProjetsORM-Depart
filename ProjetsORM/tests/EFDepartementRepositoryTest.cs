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
    public class EFDepartementRepositoryTest
    {
        private EFDepartementRepository repository;
        private Departement departement1;
        private Departement departement2;
        private Employe employe1;
        private Employe employe2;

        private void SetUp()
        {
            // Initialiser les objets nécessaires aux tests
            var builder = new DbContextOptionsBuilder<ProjetsORMContexte>();
            builder.UseInMemoryDatabase(databaseName: "dept_db");   // Database en mémoire
            var contexte = new ProjetsORMContexte(builder.Options);
            repository = new EFDepartementRepository(contexte);

            //Initialiser données
            departement1 = new Departement()
            {
                NomDepartement = "Info",
                NomComplet = "Informatique"
            };

            departement2 = new Departement()
            {
                NomDepartement = "RH",
                NomComplet = "Ress.Humaines"
            };

            employe1 = new Employe()
            {
                Nas = 123456789,
                Nom = "Lacasse",
                Prenom = "Bob",
                Sexe = 'M',
                DateEmbauche = Convert.ToDateTime("2009-09-09"),
                Fonction = "analyste",
                NomDepartement = "Info"
            };

            contexte.Employes.Add(employe1);

            employe2 = new Employe()
            {
                Nas = 123456788,
                Nom = "Labrosse",
                Prenom = "Bob",
                Sexe = 'M',
                DateEmbauche = Convert.ToDateTime("2011-09-09"),
                Fonction = "analyste",
                NomDepartement = "RH"
            };

            contexte.Employes.Add(employe2);
        }

        [Fact]
        public void AjouterDepartement()
        {
            // Arrange
            SetUp();
            
            // Act
            repository.AjouterDepartement(departement1);

            // Assert
            ICollection<Departement> result = repository.RechercherTousDepartements();
            Assert.Single(result);
            Assert.True(result.Contains(departement1));
        }

        [Fact]
        public void ModifierDepartement()
        {
            // Arrange
            SetUp();
            repository.AjouterDepartement(departement1);
            departement1.NomComplet = "TI";

            // Act
            repository.ModifierDepartement(departement1);

            // Assert
            ICollection<Departement> result = repository.RechercherTousDepartements();
            Assert.Equal(expected: departement1.NomComplet, actual: result.ElementAt(0).NomComplet);
        }

        [Fact]
        public void SupprimerDepartement()
        {
            // Arrange
            SetUp();
            repository.AjouterDepartement(departement1);

            // Act
            repository.SupprimerDepartement(departement1);

            // Assert
            ICollection<Departement> result = repository.RechercherTousDepartements();
            Assert.Empty(result);
        }

        [Fact]
        public void RechercherTousDepartements()
        {
            SetUp();
            repository.AjouterDepartement(departement1);
            repository.AjouterDepartement(departement2);

            // Act
            ICollection<Departement> result = repository.RechercherTousDepartements();

            // Assert
            Assert.Equal(expected: 2, actual: result.Count());
            Assert.True(result.Contains(departement1));
            Assert.True(result.Contains(departement2));

        }

        [Fact]
        public void RechercherNombreEmployesParDepartement()
        {
            SetUp();
            repository.AjouterDepartement(departement1);
            repository.AjouterDepartement(departement2);

            // Act
            IDictionary<string, int> result = repository.RechercherNombreEmployesParDepartement();

            // Assert
            Assert.True(result.ContainsKey(departement1.NomComplet));
            Assert.True(result.ContainsKey(departement2.NomComplet));
            Assert.Equal(expected: 1, actual: result[departement1.NomComplet]);
            Assert.Equal(expected: 1, actual: result[departement2.NomComplet]);
        }

        [Fact]
        public void RechercherNombreEmployesPourUnDepartement()
        {
            SetUp();
            repository.AjouterDepartement(departement1);

            // Act
            ICollection<Employe> result = repository.RechercheEmployesParDepartement(departement1);

            // Assert
            Assert.Single(result);
            Assert.True(result.Contains(employe1));
        }
    }
}
