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
    public class EFClientRepositoryTest
    {
        private EFClientRepository repository;
        private Client client1;
        private Client client2;

        private void SetUp()
        {
            // Initialiser les objets nécessaires aux tests
            var builder = new DbContextOptionsBuilder<ProjetsORMContexte>();
            builder.UseInMemoryDatabase(databaseName: "client_db");   // Database en mémoire
            var contexte = new ProjetsORMContexte(builder.Options);
            repository = new EFClientRepository(contexte);

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
                Ville = "Matane",
                CodePostal = "G5L1G1"
            };
            
        }

        [Fact]
        public void AjouterClient()
        {
            // Arrange
            SetUp();
            
            // Act
            repository.AjouterClient(client1);

            // Assert
            Client result = repository.RechercherClientNom(client1.NomClient);
            Assert.Equal(expected : client1, actual : result);
        }

        [Fact]
        public void ModifierClient()
        {
            // Arrange
            SetUp();
            repository.AjouterClient(client1);
            client1.Ville = "Montréal";

            // Act
            repository.ModifierClient(client1);

            // Assert
            Client result = repository.RechercherClientNom(client1.NomClient);
            Assert.Equal(expected: client1, actual: result);
        }

        [Fact]
        public void SupprimerClient()
        {
            // Arrange
            SetUp();
            repository.AjouterClient(client1);

            // Act
            repository.SupprimerClient(client1);

            // Assert
            Client result = repository.RechercherClientNom(client1.NomClient);
            Assert.Null(result);
        }

        [Fact]
        public void RechercherClientParNom()
        {
            SetUp();
            repository.AjouterClient(client1);
            repository.AjouterClient(client2);

            // Act
            Client result = repository.RechercherClientNom(client2.NomClient);

            // Assert
            Assert.Equal(expected: client2, actual: result);
        }

        [Fact]
        public void RechercherClientParVille()
        {
            SetUp();
            repository.AjouterClient(client1);
            repository.AjouterClient(client2);

            // Act
            ICollection<Client> result = repository.RechercherClientVille(client2.Ville);

            // Assert
            Assert.True(result.Contains(client2));
            Assert.False(result.Contains(client1));
        }
    }
}
