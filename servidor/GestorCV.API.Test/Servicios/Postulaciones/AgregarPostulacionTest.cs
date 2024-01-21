using GestorCV.API.Controllers.Servicios;
using GestorCV.API.Controllers.Servicios.Postulaciones;
using GestorCV.API.Infraestructura;
using GestorCV.API.Repositorios;
using Microsoft.Extensions.Configuration;
using Moq;

namespace GestorCV.API.Test.Servicios.Postulaciones
{
    [TestClass]
    public class AgregarPostulacionTest
    {
        private readonly EjecutorPeticiones ejecutorPeticiones;
        private const string EmpleoPrueba = "Desarrollador Fullstack Senior";
        private const string UsuarioPrueba = "Lautaro Avecilla";

        private Mock<IRepositorioPostulaciones> repositorioPostulacionesMock = new Mock<IRepositorioPostulaciones>(MockBehavior.Strict);

        public AgregarPostulacionTest()
        {
            ejecutorPeticiones = new EjecutorPeticiones();

            //Preparo variables globales
            //Arrange
            var memoriaConfiguracion = new Dictionary<string, string> {
                {"CorreoSMTP:Host", "sandbox.smtp.mailtrap.io"},
                {"CorreoSMTP:Puerto", "2525"},
                {"CorreoSMTP:Usuario", "59aa86b6e194ea"},
                {"CorreoSMTP:Password", "203cdd5a1d9da9"},
                {"CorreoSMTP:CorreoOrigen", "correo@origen.com"},
            };

            IConfiguration configuracionGlobal = new ConfigurationBuilder()
                .AddInMemoryCollection(memoriaConfiguracion)
                .Build();

            new AppConfiguration(configuracionGlobal);
        }

        [TestMethod]
        public void Agregar_Satisfactorio()
        {
            // Preparo parámetros
            var idUsuario = 1;
            var idEmpleo = 2;
            var idNuevaPostulacion = 3;

            var parametros = new PeticionAgregar.Parametros(idEmpleo, idUsuario, "lautaro.avecilla@uai.edu.ar");

            // Preparo respuesta de repositorio y métodos de transacciones
            repositorioPostulacionesMock.Setup(r => r.Agregar(It.IsAny<Models.Dtos.Postulacion>()))
                .Returns(new RepositorioPostulaciones.RespuestaAgregarPostulacion(idNuevaPostulacion, EmpleoPrueba, UsuarioPrueba));
            repositorioPostulacionesMock.Setup(r => r.IniciarTransaccion());
            repositorioPostulacionesMock.Setup(r => r.ConfirmarTransaccion());

            var peticion = new PeticionAgregar(parametros, repositorioPostulacionesMock.Object);

            // Ejecuto la petición
            var resultado = ejecutorPeticiones.Ejecutar(peticion);

            // Valido la respuestas
            Assert.IsNotNull(resultado);

            // Verifico que devuelva en nuevo ID
            var resultadoAgregarPostulacion = (PeticionAgregar.Resultado)resultado;
            Assert.AreEqual(idNuevaPostulacion, resultadoAgregarPostulacion.Id);

            // Verifico que el repositorio sea llamado al menos una vez
            repositorioPostulacionesMock.Verify(r => r.Agregar(It.IsAny<Models.Dtos.Postulacion>()), Times.Once);

            // Verifico que el repositorio inició y confirmó la transacción
            repositorioPostulacionesMock.Verify(r => r.IniciarTransaccion(), Times.Once);
            repositorioPostulacionesMock.Verify(r => r.ConfirmarTransaccion(), Times.Once);
            repositorioPostulacionesMock.Verify(r => r.AnularTransaccion(), Times.Never);
        }

        [TestMethod]
        public void Agregar_UsuarioNoValido()
        {
            // Preparo parámetros
            // Envío usuario no valido
            var idUsuario = 0;
            var idEmpleo = 2;

            var parametros = new PeticionAgregar.Parametros(idEmpleo, idUsuario, "lautaro.avecilla@uai.edu.ar" );

            // Preparo repositorio para verificar
            repositorioPostulacionesMock.Setup(r => r.Agregar(It.IsAny<Models.Dtos.Postulacion>()));

            var peticion = new PeticionAgregar(parametros, repositorioPostulacionesMock.Object);

            // Ejecuto la petición y verifico que lance una excepción de validación
            Assert.ThrowsException<ValidacionException>(() => { ejecutorPeticiones.Ejecutar(peticion); });

            // Verifico que el repositorio no sea llamado
            repositorioPostulacionesMock.Verify(r => r.Agregar(It.IsAny<Models.Dtos.Postulacion>()), Times.Never);

            // Verifico que el repositorio no haya iniciado transacciones
            repositorioPostulacionesMock.Verify(r => r.IniciarTransaccion(), Times.Never);
            repositorioPostulacionesMock.Verify(r => r.ConfirmarTransaccion(), Times.Never);
            repositorioPostulacionesMock.Verify(r => r.AnularTransaccion(), Times.Never);
        }

        [TestMethod]
        public void Agregar_EmpleoNoValido()
        {
            // Preparo parámetros
            var idUsuario = 1;
            // Envío empleo no valido
            var idEmpleo = 0;

            var parametros = new PeticionAgregar.Parametros(idEmpleo, idUsuario, "lautaro.avecilla@uai.edu.ar");

            // Preparo repositorio para verificar
            repositorioPostulacionesMock.Setup(r => r.Agregar(It.IsAny<Models.Dtos.Postulacion>()));

            var peticion = new PeticionAgregar(parametros, repositorioPostulacionesMock.Object);

            // Ejecuto la petición y verifico que lance una excepción de validación
            Assert.ThrowsException<ValidacionException>(() => { ejecutorPeticiones.Ejecutar(peticion); });

            // Verifico que el repositorio no sea llamado
            repositorioPostulacionesMock.Verify(r => r.Agregar(It.IsAny<Models.Dtos.Postulacion>()), Times.Never);

            // Verifico que el repositorio no haya iniciado transacciones
            repositorioPostulacionesMock.Verify(r => r.IniciarTransaccion(), Times.Never);
            repositorioPostulacionesMock.Verify(r => r.ConfirmarTransaccion(), Times.Never);
            repositorioPostulacionesMock.Verify(r => r.AnularTransaccion(), Times.Never);
        }
    }
}