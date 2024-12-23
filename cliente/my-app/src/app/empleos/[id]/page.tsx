"use client";
import { useRouter } from "next/navigation";
import { useEffect, useState } from "react";
import Layout from "../../layoutUser";
import { mensajeErrorGeneral } from "@/constants";
import { fetchPrivado } from "@/componentes/compartido";
import Modal from "@/componentes/compartido/modal";
import Spinner from "@/componentes/compartido/spinner";

export default function Empleo({ params }: { params: { id: string } }) {
  const { push } = useRouter();

  const [mensajeModal, setMensajeModal] = useState<string>();
  const [tituloModal, setTituloModal] = useState<string>("");
  const [empleo, setEmpleo] = useState<{
    id: string;
    titulo: string;
    descripcion: string;
    fechaPublicacion: string;
    ubicacion: string;
    remuneracion: number;
    modalidadTrabajo: string;
    horariosLaborales: string;
    tipoTrabajo: string;
    perfiles: Array<{ id: number; nombre: string }>;
    etiquetas: Array<{ id: number; nombre: string }>;
  }>();
  const [cursos, setCursos] =
    useState<{ id: number; titulo: string; contenido: string }[]>();
  const [indiceCursoActivo, setIndiceCursoActivo] = useState<number | null>();

  const cambiarAcordeon = (index: number) => {
    setIndiceCursoActivo(indiceCursoActivo === index ? null : index);
  };

  useEffect(() => {
    const fetchData = async () => {
      await fetchPrivado(`http://localhost:4000/empleos/${params.id}`, "GET")
        .then(async (data) => {
          if (data.ok) {
            const respuesta = await data.json();
            cargarDatosIniciales(respuesta);
            return;
          }

          setTituloModal("Error");
          setMensajeModal(mensajeErrorGeneral);
        })
        .catch(() => {
          setTituloModal("Error");
          setMensajeModal(mensajeErrorGeneral);
        });
    };

    fetchData();
  }, [params.id]);

  const cargarDatosIniciales = (datos: any) => {
    const empleoActual: {
      id: string;
      titulo: string;
      descripcion: string;
      fechaPublicacion: string;
      ubicacion: string;
      remuneracion: number;
      modalidadTrabajo: string;
      horariosLaborales: string;
      tipoTrabajo: string;
      perfiles: Array<{ id: number; nombre: string }>;
      etiquetas: Array<{ id: number; nombre: string }>;
    } = {
      id: datos.empleo.id,
      titulo: datos.empleo.titulo,
      descripcion: datos.empleo.mensaje,
      ubicacion: datos.empleo.ubicacion,
      modalidadTrabajo: datos.empleo.modalidadTrabajo,
      tipoTrabajo: datos.empleo.tipoTrabajo,
      fechaPublicacion: datos.empleo.fechaPublicacion,
      horariosLaborales: datos.empleo.horarioLaboral,
      remuneracion: datos.empleo.remuneracion,
      etiquetas: datos.etiquetas.filter((x: { nombre: string; id: string }) =>
        datos.empleo.etiquetas.includes(parseInt(x.id))
      ),
      perfiles: datos.perfiles.filter((x: { nombre: string; id: string }) =>
        datos.empleo.perfiles.includes(parseInt(x.id))
      ),
    };

    setEmpleo(empleoActual);

    var cursosRecomendados = datos.cursos.map(
      (x: { id: number; titulo: string; mensaje: string }) => ({
        id: x.id,
        titulo: x.titulo,
        contenido: x.mensaje,
      })
    );

    setCursos(cursosRecomendados);
  };

  const postulacionClick = async () => {
    await fetchPrivado(
      `http://localhost:4000/postulaciones/${params.id}`,
      "POST"
    )
      .then(async (data) => {
        if (data.ok) {
          setTituloModal("Postulación");
          setMensajeModal(
            "Se ha postulado correctamente a la posición. Se ha enviado un correo de confirmación.\nEn breve recibirá novedades respecto al progreso de su postulación."
          );

          return;
        } else if (data.status === 400) {
          const respuesta = await data.json();
          const validaciones = respuesta.Validaciones.map(
            (validacion: { Mensaje: string }) => validacion.Mensaje
          ).join("\r\n");

          setTituloModal("Error");
          setMensajeModal(validaciones);

          return;
        }

        setTituloModal("Error");
        setMensajeModal(mensajeErrorGeneral);
      })
      .catch((error) => {
        setTituloModal("Error");
        setMensajeModal(error);
      });
  };

  const backButtonClick = async () => {
    push("/empleos");
  };

  const limpiarModal = () => setMensajeModal(undefined);

  return (
    <Layout>
      <div className="cuerpo">
        {empleo && (
          <>
            <h1 className="Titulo2">
              <strong>{empleo.titulo}</strong>
            </h1>

            <div className="etiquetas">
              {empleo.etiquetas.map((e, index) => (
                <span key={index} className="etiqueta">
                  <svg
                    xmlns="http://www.w3.org/2000/svg"
                    fill="none"
                    viewBox="0 0 24 24"
                    strokeWidth="1.5"
                    stroke="currentColor"
                    className="w-6 h-6"
                  >
                    <path
                      strokeLinecap="round"
                      strokeLinejoin="round"
                      d="M9.568 3H5.25A2.25 2.25 0 0 0 3 5.25v4.318c0 .597.237 1.17.659 1.591l9.581 9.581c.699.699 1.78.872 2.607.33a18.095 18.095 0 0 0 5.223-5.223c.542-.827.369-1.908-.33-2.607L11.16 3.66A2.25 2.25 0 0 0 9.568 3Z"
                    />
                    <path
                      strokeLinecap="round"
                      strokeLinejoin="round"
                      d="M6 6h.008v.008H6V6Z"
                    />
                  </svg>
                  {e.nombre}
                </span>
              ))}
            </div>
            <div className="contenedor">
              <div className="detalle">
                <div className="row">
                  <div className="col-50">
                    <label>Ubicación: </label>
                    {empleo.ubicacion}
                  </div>
                  <div className="col-50">
                    <label>Modalidad de trabajo: </label>
                    {empleo.modalidadTrabajo}
                  </div>
                </div>
                <div className="row">
                  <div className="col-50">
                    <label>Categoría: </label>
                    {empleo.perfiles.map((p) => p.nombre).join(" / ")}
                  </div>
                  <div className="col-50">
                    <label>Publicado: </label>
                    {empleo.fechaPublicacion}
                  </div>
                </div>
                <div className="row">
                  <div className="col-50">
                    <label>Horario: </label>
                    {empleo.horariosLaborales}
                  </div>
                  <div className="col-50">
                    <label>Tipo de trabajo: </label>
                    {empleo.tipoTrabajo}
                  </div>
                </div>
                <div className="row">
                  <div className="col">
                    <div className="boton-postulacion">
                      <button
                        className="boton btn-primary"
                        onClick={postulacionClick}
                        id="botonpost"
                      >
                        POSTULARME
                      </button>
                    </div>
                  </div>
                </div>
              </div>
            </div>

            <div
              className="Descripcion"
              dangerouslySetInnerHTML={{ __html: empleo.descripcion }}
            ></div>
          </>
        )}
        {!empleo && (
          <>
            <div className="spinner" role="status">
              <svg
                aria-hidden="true"
                className="w-8 h-8 text-gray-200 animate-spin dark:text-gray-600 fill-blue-600"
                viewBox="0 0 100 101"
                fill="none"
                xmlns="http://www.w3.org/2000/svg"
              >
                <path
                  d="M100 50.5908C100 78.2051 77.6142 100.591 50 100.591C22.3858 100.591 0 78.2051 0 50.5908C0 22.9766 22.3858 0.59082 50 0.59082C77.6142 0.59082 100 22.9766 100 50.5908ZM9.08144 50.5908C9.08144 73.1895 27.4013 91.5094 50 91.5094C72.5987 91.5094 90.9186 73.1895 90.9186 50.5908C90.9186 27.9921 72.5987 9.67226 50 9.67226C27.4013 9.67226 9.08144 27.9921 9.08144 50.5908Z"
                  fill="currentColor"
                />
                <path
                  d="M93.9676 39.0409C96.393 38.4038 97.8624 35.9116 97.0079 33.5539C95.2932 28.8227 92.871 24.3692 89.8167 20.348C85.8452 15.1192 80.8826 10.7238 75.2124 7.41289C69.5422 4.10194 63.2754 1.94025 56.7698 1.05124C51.7666 0.367541 46.6976 0.446843 41.7345 1.27873C39.2613 1.69328 37.813 4.19778 38.4501 6.62326C39.0873 9.04874 41.5694 10.4717 44.0505 10.1071C47.8511 9.54855 51.7191 9.52689 55.5402 10.0491C60.8642 10.7766 65.9928 12.5457 70.6331 15.2552C75.2735 17.9648 79.3347 21.5619 82.5849 25.841C84.9175 28.9121 86.7997 32.2913 88.1811 35.8758C89.083 38.2158 91.5421 39.6781 93.9676 39.0409Z"
                  fill="currentFill"
                />
              </svg>
              <span className="sr-only">Cargando...</span>
            </div>
          </>
        )}
        <div>
          <h1 className="text-2xl font-bold text-gray-800 border-b-2 border-gray-300 pb-2 mb-4">
            Cursos Recomendados
          </h1>

          {!cursos && <Spinner />}
          {cursos && (
            <div className="space-y-4 mb-4">
              {cursos.map((curso, index) => (
                <div key={curso.id} className="border rounded-lg">
                  <button
                    onClick={() => cambiarAcordeon(index)}
                    className="w-full flex justify-between items-center px-4 py-2 font-semibold text-left bg-gray-200 hover:bg-gray-300 focus:outline-none"
                  >
                    <span>{curso.titulo}</span>
                    <svg
                      className={`w-4 h-4 transform transition-transform ${
                        indiceCursoActivo === index ? "rotate-180" : ""
                      }`}
                      xmlns="http://www.w3.org/2000/svg"
                      fill="none"
                      viewBox="0 0 24 24"
                      stroke="currentColor"
                    >
                      <path
                        strokeLinecap="round"
                        strokeLinejoin="round"
                        strokeWidth="2"
                        d="M19 9l-7 7-7-7"
                      />
                    </svg>
                  </button>
                  {indiceCursoActivo === index && (
                    <div
                      className="px-4 py-2 bg-white"
                      dangerouslySetInnerHTML={{ __html: curso.contenido }}
                    ></div>
                  )}
                </div>
              ))}
            </div>
          )}
        </div>
        <button onClick={backButtonClick} type="button" className="boton">
          Volver
        </button>
      </div>
      {mensajeModal && (
        <Modal
          mostrar={!!mensajeModal}
          titulo={tituloModal}
          onCambiarModal={limpiarModal}
        >
          <p>{mensajeModal}</p>
        </Modal>
      )}
    </Layout>
  );
}
