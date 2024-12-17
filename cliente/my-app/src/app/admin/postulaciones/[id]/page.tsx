"use client";
import { useState, useEffect, useMemo } from "react";
import { useRouter } from "next/navigation";
import Select from "react-select";
import Layout from "../../../layoutUser";
import Modal from "@/componentes/compartido/modal";
import Spinner from "@/componentes/compartido/spinner";
import { SelectOpcion } from "@/componentes/compartido/select/selectOpcion";
import {
  fetchPrivado,
  formatearFecha,
  obtenerRolUsuario,
} from "@/componentes/compartido";
import {
  EstadoDescartado,
  EstadoEntrevista,
  EstadoFinalizado,
  EstadoPendiente,
  EstadoRevisado,
  mensajeErrorGeneral,
} from "@/constants";

export default function Postulacion({ params }: { params: { id: string } }) {
  const { push } = useRouter();
  const [rolUsuario, setRolUsuario] = useState<string>();
  const [mensajeModal, setMensajeModal] = useState<string>();
  const [tituloModal, setTituloModal] = useState<string>("");
  const [fecha, setFecha] = useState<string>("");
  const [estado, setEstado] = useState<string>("");
  const [usuario, setUsuario] = useState<string>();
  const [mostrarCalificaciones, setMostrarCalificaciones] =
    useState<boolean>(false);
  const [empleo, setEmpleo] = useState<{
    titulo: string;
    ubicacion: string;
    modalidadTrabajo: string;
    tipoTrabajo: string;
    horarios: string;
    remuneracion: number;
    destacado: boolean;
    perfiles: readonly SelectOpcion[];
    etiquetas: readonly SelectOpcion[];
  }>({
    titulo: "",
    ubicacion: "",
    modalidadTrabajo: "",
    tipoTrabajo: "",
    horarios: "",
    remuneracion: 0,
    destacado: false,
    perfiles: [],
    etiquetas: [],
  });
  const [calificaciones, setCalificaciones] = useState<
    {
      idUsuarioCalificador: number;
      usuario: string;
      puntuacion: number;
      observaciones: string;
    }[]
  >();

  const [curriculum, setCurriculum] = useState<{
    titulo: string;
    ubicacion: string;
    resumenProfesional: string;
    intereses: string;
    empleos: Array<{
      organizacion: string;
      cargo: string;
      ingreso: Date;
      egreso: Date | null;
      area: string;
      funciones: string;
    }>;
    estudios: Array<{
      institucion: string;
      nivel: string;
      ingreso: Date;
      egreso: Date | null;
      titulo: string;
    }>;
    idiomas: Array<{
      nombre: string;
      nivel: string;
    }>;
    cursos: Array<{
      institucion: string;
      titulo: string;
      fecha: Date;
    }>;
    perfiles: readonly SelectOpcion[];
    etiquetas: readonly SelectOpcion[];
  }>({
    titulo: "",
    ubicacion: "",
    resumenProfesional: "",
    intereses: "",
    empleos: [],
    estudios: [],
    idiomas: [],
    cursos: [],
    perfiles: [],
    etiquetas: [],
  });

  useEffect(() => {
    const obtenerDatosPostulacion = async () => {
      await fetchPrivado(
        `http://localhost:4000/postulaciones/${params.id}`,
        "GET"
      )
        .then(async (data) => {
          if (data.ok) {
            const respuesta = await data.json();

            setFecha(formatearFecha(respuesta.fecha));
            setEstado(respuesta.estado);
            cargarDatosUsuario(respuesta);
            cargarDatosCv(respuesta);
            cargarDatosEmpleo(respuesta);
            setCalificaciones([
              {
                idUsuarioCalificador: 5,
                usuario: "Ignacio Riveira",
                puntuacion: 5,
                observaciones:
                  "¡Excelente perfil y predisposición! Lamentablemente, no cubríamos su expectativa salarial.",
              },
              {
                idUsuarioCalificador: 6,
                usuario: "Martín Ballester",
                puntuacion: 3,
                observaciones: "Mas o menos",
              },
            ]);

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

    obtenerDatosPostulacion();

    const rol = obtenerRolUsuario();

    if (rol === null) {
      return;
    }

    setRolUsuario(rol);
  }, []);

  const promedioCalificaciones = useMemo(() => {
    if (!calificaciones) {
      return;
    }

    if (!calificaciones || calificaciones.length === 0) {
      return 0;
    }

    const suma = calificaciones.reduce(
      (total, calificacion) => total + calificacion.puntuacion,
      0
    );
    const promedio = suma / calificaciones.length;

    return Math.round(promedio);
  }, [calificaciones]);

  const cargarDatosUsuario = (datos: any) => {
    const { usuario } = datos;

    setUsuario(usuario);
  };

  const cargarDatosEmpleo = (datos: any) => {
    const { empleo: datosEmpleo } = datos;

    let nuevoEmpleo: {
      titulo: string;
      ubicacion: string;
      modalidadTrabajo: string;
      tipoTrabajo: string;
      horarios: string;
      remuneracion: number;
      destacado: boolean;
      perfiles: readonly SelectOpcion[];
      etiquetas: readonly SelectOpcion[];
    } = {
      titulo: "",
      ubicacion: "",
      modalidadTrabajo: "",
      tipoTrabajo: "",
      horarios: "",
      remuneracion: 0,
      destacado: false,
      perfiles: [],
      etiquetas: [],
    };

    nuevoEmpleo.titulo = datosEmpleo.titulo;
    nuevoEmpleo.ubicacion = datosEmpleo.ubicacion;
    nuevoEmpleo.modalidadTrabajo = datosEmpleo.modalidadTrabajo;
    nuevoEmpleo.tipoTrabajo = datosEmpleo.tipoTrabajo;
    nuevoEmpleo.horarios = datosEmpleo.horarioLaboral;
    nuevoEmpleo.remuneracion = datosEmpleo.remuneracion;
    nuevoEmpleo.destacado = datosEmpleo.destacado;

    if (datosEmpleo.perfiles) {
      const datosPerfiles = datosEmpleo.perfiles.map(
        (x: any) => new Option(x.nombre, x.id)
      );
      nuevoEmpleo.perfiles = datosPerfiles;
    }

    if (datosEmpleo.etiquetas) {
      const datosEtiquetas = datosEmpleo.etiquetas.map(
        (x: any) => new Option(x.nombre, x.id)
      );
      nuevoEmpleo.etiquetas = datosEtiquetas;
    }

    setEmpleo(nuevoEmpleo);
  };

  const cargarDatosCv = (datos: any) => {
    const { curriculum: datosCurriculum } = datos;

    let nuevoCurriculum: {
      titulo: string;
      ubicacion: string;
      resumenProfesional: string;
      intereses: string;
      empleos: Array<{
        organizacion: string;
        cargo: string;
        ingreso: Date;
        egreso: Date | null;
        area: string;
        funciones: string;
      }>;
      estudios: Array<{
        institucion: string;
        nivel: string;
        ingreso: Date;
        egreso: Date | null;
        titulo: string;
      }>;
      idiomas: Array<{
        nombre: string;
        nivel: string;
      }>;
      cursos: Array<{
        institucion: string;
        titulo: string;
        fecha: Date;
      }>;
      perfiles: readonly SelectOpcion[];
      etiquetas: readonly SelectOpcion[];
    } = {
      titulo: "",
      ubicacion: "",
      resumenProfesional: "",
      intereses: "",
      empleos: [],
      estudios: [],
      idiomas: [],
      cursos: [],
      perfiles: [],
      etiquetas: [],
    };

    const datosEmpleos = datosCurriculum.experienciaLaboral
      ? JSON.parse(datosCurriculum.experienciaLaboral)
      : [];

    nuevoCurriculum["empleos"] = datosEmpleos;

    const datosEstudios = datosCurriculum.educacion
      ? JSON.parse(datosCurriculum.educacion)
      : [];

    nuevoCurriculum["estudios"] = datosEstudios;

    const datosIdiomas = datosCurriculum.idiomas
      ? JSON.parse(datosCurriculum.idiomas)
      : [];

    nuevoCurriculum["idiomas"] = datosIdiomas;

    const datosCursos = datosCurriculum.certificados
      ? JSON.parse(datosCurriculum.certificados)
      : [];

    nuevoCurriculum["cursos"] = datosCursos;

    nuevoCurriculum.titulo = datosCurriculum.titulo;
    nuevoCurriculum.ubicacion = datosCurriculum.ubicacion;
    nuevoCurriculum.resumenProfesional = datosCurriculum.resumenProfesional;
    nuevoCurriculum.intereses = datosCurriculum.intereses;

    if (datosCurriculum.perfiles) {
      const datosPerfiles = datosCurriculum.perfiles.map(
        (x: any) => new Option(x.nombre, x.id)
      );
      nuevoCurriculum.perfiles = datosPerfiles;
    }

    if (datosCurriculum.etiquetas) {
      const datosEtiquetas = datosCurriculum.etiquetas.map(
        (x: any) => new Option(x.nombre, x.id)
      );
      nuevoCurriculum.etiquetas = datosEtiquetas;
    }

    setCurriculum(nuevoCurriculum);
  };

  const botonEstadoClick = async (nuevoEstado: string) => {
    if (nuevoEstado === EstadoPendiente || estado === nuevoEstado) {
      return;
    }

    const estadoAnterior = estado;
    setEstado("");

    await fetchPrivado(
      `http://localhost:4000/postulaciones/${params.id}`,
      "PUT",
      JSON.stringify({ estado: nuevoEstado })
    )
      .then(async (data) => {
        if (data.ok) {
          setEstado(nuevoEstado);
          setTituloModal("Postulación actualizada");
          setMensajeModal(
            "Se ha realizado la actualización del estado de la postulación. El candidato será notificado a la brevedad."
          );
          return;
        }
        setTituloModal("Error");
        setMensajeModal(mensajeErrorGeneral);
        setEstado(estadoAnterior);
      })
      .catch(() => {
        setTituloModal("Error");
        setMensajeModal(mensajeErrorGeneral);
        setEstado(estadoAnterior);
      });
  };

  const backButtonClick = async () => {
    push("/admin/postulaciones");
  };

  const limpiarModal = () => setMensajeModal(undefined);
  const limpiarModalCalificaciones = () => setMostrarCalificaciones(false);

  return (
    <Layout>
      <div className="cuerpo">
        <div className="grid grid-cols-3 gap-4">
          <div>
            <div className="flex items-center mb-2">
              <h1 className="font-bold">Candidato</h1>
              {calificaciones && (
                <div className="flex items-center ml-2">
                  {Array.from({ length: 5 }, (_, index) =>
                    index < promedioCalificaciones! ? (
                      <svg
                        key={index}
                        className="w-4 h-4 text-yellow-300 ms-1"
                        aria-hidden="true"
                        xmlns="http://www.w3.org/2000/svg"
                        fill="currentColor"
                        viewBox="0 0 22 20"
                      >
                        <path d="M20.924 7.625a1.523 1.523 0 0 0-1.238-1.044l-5.051-.734-2.259-4.577a1.534 1.534 0 0 0-2.752 0L7.365 5.847l-5.051.734A1.535 1.535 0 0 0 1.463 9.2l3.656 3.563-.863 5.031a1.532 1.532 0 0 0 2.226 1.616L11 17.033l4.518 2.375a1.534 1.534 0 0 0 2.226-1.617l-.863-5.03L20.537 9.2a1.523 1.523 0 0 0 .387-1.575Z" />
                      </svg>
                    ) : (
                      <svg
                        key={index}
                        className="w-4 h-4 ms-1 text-gray-300 dark:text-gray-500"
                        aria-hidden="true"
                        xmlns="http://www.w3.org/2000/svg"
                        fill="currentColor"
                        viewBox="0 0 22 20"
                      >
                        <path d="M20.924 7.625a1.523 1.523 0 0 0-1.238-1.044l-5.051-.734-2.259-4.577a1.534 1.534 0 0 0-2.752 0L7.365 5.847l-5.051.734A1.535 1.535 0 0 0 1.463 9.2l3.656 3.563-.863 5.031a1.532 1.532 0 0 0 2.226 1.616L11 17.033l4.518 2.375a1.534 1.534 0 0 0 2.226-1.617l-.863-5.03L20.537 9.2a1.523 1.523 0 0 0 .387-1.575Z" />
                      </svg>
                    )
                  )}

                  {!calificaciones.length && (
                    <span className="cursor-pointer ml-2 font-bold text-blue-900 text-sm">
                      No hay calificaciones
                    </span>
                  )}
                  {!!calificaciones.length && (
                    <span
                      onClick={() => {
                        setMostrarCalificaciones(true);
                      }}
                      className="cursor-pointer ml-2 font-bold text-blue-900 text-sm"
                    >
                      Ver calificaciones ({calificaciones.length})
                    </span>
                  )}
                </div>
              )}
            </div>
            <label className="text-lg">{usuario}</label>
          </div>
          <div className="col-span-2 flex">
            <div className="mr-10">
              <h1 className="font-bold mb-2">Estado</h1>
              <button
                onClick={() => {
                  botonEstadoClick(EstadoPendiente);
                }}
                type="button"
                disabled={estado !== EstadoPendiente}
                className={`boton mr-2 ${
                  estado === EstadoPendiente
                    ? "btn-primary !cursor-default"
                    : ""
                }`}
              >
                Pendiente
              </button>
              <button
                onClick={() => {
                  botonEstadoClick(EstadoDescartado);
                }}
                disabled={
                  estado !== EstadoPendiente && estado !== EstadoDescartado
                }
                type="button"
                className={`boton mr-2 ${
                  estado === EstadoDescartado
                    ? "btn-primary !cursor-default"
                    : estado === EstadoPendiente
                    ? "!bg-red-500 !text-white"
                    : ""
                }`}
              >
                Descartado
              </button>
              <button
                onClick={() => {
                  botonEstadoClick(EstadoRevisado);
                }}
                type="button"
                disabled={
                  estado !== EstadoPendiente && estado !== EstadoRevisado
                }
                className={`boton mr-2 ${
                  estado === EstadoRevisado
                    ? "btn-primary !cursor-default"
                    : estado === EstadoPendiente
                    ? "!bg-blue-500 !text-white"
                    : ""
                }`}
              >
                Revisado
              </button>
              <button
                onClick={() => {
                  botonEstadoClick(EstadoEntrevista);
                }}
                disabled={
                  estado !== EstadoRevisado && estado !== EstadoEntrevista
                }
                type="button"
                className={`boton mr-2 ${
                  estado === EstadoEntrevista
                    ? "btn-primary !cursor-default"
                    : estado === EstadoRevisado
                    ? "!bg-violet-500 !text-white"
                    : ""
                }`}
              >
                Entrevista
              </button>
              <button
                onClick={() => {
                  botonEstadoClick(EstadoFinalizado);
                }}
                type="button"
                disabled={
                  estado !== EstadoEntrevista && estado !== EstadoFinalizado
                }
                className={`boton ${
                  estado === "Finalizado"
                    ? "btn-primary !cursor-default"
                    : estado === EstadoEntrevista
                    ? "!bg-green-500 !text-white"
                    : ""
                }`}
              >
                Finalizado
              </button>
            </div>
            <div>
              <h1 className="font-bold mb-2">Fecha de postulación</h1>
              <label>{fecha}</label>
            </div>
            <div className="flex ml-4 justify-center">
              {calificaciones && (
                // (estado === EstadoFinalizado ||
                //   estado === EstadoDescartado) &&
                <button onClick={() => {}} type="button" className="boton">
                  Calificar
                </button>
              )}
            </div>
          </div>
        </div>

        <div className="grid grid-cols-3 gap-4 mt-4">
          {/* Panel izquierdo - Info empleo */}
          <div>
            <h1 className="font-bold mb-2 underline">Datos de vacante</h1>
            <div className="uai-shadow p-2 my-4">
              <h2 className="font-bold">Título</h2>
              <input
                type="text"
                value={empleo?.titulo}
                className="border w-full mt-1"
                disabled
              />
            </div>

            <div className="uai-shadow p-2 my-4">
              <h2 className="font-bold">Ubicación</h2>
              <input
                type="text"
                value={empleo?.ubicacion}
                className="border w-full mt-1"
                disabled
              />
            </div>
            <div className="uai-shadow p-2 my-4">
              <h2 className="font-bold mb-2">Modalidad de Trabajo</h2>
              <input
                type="text"
                value={empleo?.modalidadTrabajo}
                className="border w-full mt-1"
                disabled
              />
            </div>

            <div className="uai-shadow p-2 my-4">
              <h2 className="font-bold mb-2">Tipo de Trabajo</h2>
              <input
                type="text"
                value={empleo?.tipoTrabajo}
                className="border w-full mt-1"
                disabled
              />
            </div>
            <div className="uai-shadow p-2 my-4">
              <h2 className="font-bold">Horarios laborales</h2>
              <input
                value={empleo?.horarios}
                className="border w-full mt-1"
                disabled
              />
            </div>

            <div
              className={`grid ${
                rolUsuario === "Administrador" ? "grid-cols-2" : "grid-cols-1"
              } gap-4 mb-2`}
            >
              <div className="uai-shadow p-2">
                <h2 className="font-bold">Remuneración</h2>
                <input
                  type="text"
                  value={empleo?.remuneracion.toLocaleString("en-US", {
                    style: "currency",
                    currency: "USD",
                    minimumFractionDigits: 0,
                    maximumFractionDigits: 0,
                  })}
                  className="border w-full mt-1"
                  disabled
                />
              </div>
              {rolUsuario === "Administrador" && (
                <div className="uai-shadow p-2">
                  <h2 className="font-bold text-center">Destacado</h2>
                  <label className="flex items-center mt-1">
                    <input
                      type="checkbox"
                      name="destacado"
                      checked={empleo?.destacado}
                      className="border m-auto w-6 h-6"
                      disabled
                    />
                  </label>
                </div>
              )}
            </div>

            <div className="uai-shadow p-2 my-4">
              <h2 className="font-bold mb-2">Perfiles</h2>
              <Select
                className="mt-2"
                placeholder=""
                isLoading={!empleo?.perfiles}
                isMulti={true}
                isDisabled={true}
                value={empleo?.perfiles}
              />
            </div>
            <div className="uai-shadow p-2 my-4">
              <h2 className="font-bold mb-2">Etiquetas</h2>
              <Select
                className="mt-2"
                placeholder=""
                isLoading={!empleo?.etiquetas}
                isMulti={true}
                isDisabled={true}
                value={empleo?.etiquetas}
              />
            </div>
          </div>

          {/* Panel derecho - Curriculum candidato */}
          <div className="col-span-2">
            <div>
              <h1 className="font-bold mb-2 underline">Datos de candidato</h1>
            </div>
            <div className="uai-shadow p-2 my-4">
              <h2 className="font-bold">Título</h2>
              <input
                type="text"
                value={curriculum?.titulo}
                className="border w-full mt-1"
                disabled
              />
            </div>

            <div className="uai-shadow p-2 my-4">
              <h2 className="font-bold">Ubicación</h2>
              <input
                type="text"
                value={curriculum?.ubicacion}
                className="border w-full mt-1"
                disabled
              />
            </div>

            <div className="uai-shadow p-2 my-4">
              <h2 className="font-bold">Resumen profesional</h2>
              <textarea
                className="w-full border border-solid p-2"
                cols={30}
                rows={3}
                value={curriculum?.resumenProfesional}
                disabled
              ></textarea>
            </div>

            <div className="uai-shadow p-2 my-4">
              <h2 className="font-bold mb-2 inline">Empleos</h2>

              <div className="uai-shadow relative overflow-x-auto mt-2">
                <table className="w-full text-sm text-left rtl:text-right text-gray-500 dark:text-gray-400">
                  <thead className="text-xs text-gray-700 uppercase bg-gray-50 dark:bg-gray-700 dark:text-gray-400">
                    <tr>
                      <th scope="col" className="px-6 py-3">
                        Organizacion
                      </th>
                      <th scope="col" className="px-6 py-3">
                        Cargo
                      </th>
                      <th scope="col" className="px-6 py-3">
                        Ingreso
                      </th>
                      <th scope="col" className="px-6 py-3">
                        Egreso
                      </th>
                      <th scope="col" className="px-6 py-3">
                        Area
                      </th>
                      <th scope="col" className="px-6 py-3">
                        Funciones
                      </th>
                    </tr>
                  </thead>
                  <tbody>
                    {curriculum?.empleos &&
                      curriculum?.empleos.map((e, index) => {
                        return (
                          <tr
                            key={index}
                            className="bg-white dark:bg-white dark:border-gray-700"
                          >
                            <td className="px-6 py-4 flex">
                              <span className="font-bold">
                                {e.organizacion}
                              </span>
                            </td>
                            <td className="px-6 py-4">{e.cargo}</td>
                            <td className="px-6 py-4">
                              {formatearFecha(e.ingreso)}
                            </td>
                            <td className="px-6 py-4">
                              {e.egreso
                                ? formatearFecha(e.egreso)
                                : "Actualmente"}
                            </td>
                            <td className="px-6 py-4">{e.area}</td>
                            <td className="px-6 py-4">{e.funciones}</td>
                          </tr>
                        );
                      })}
                    {!curriculum?.empleos && (
                      <tr className="bg-white border-b dark:bg-white dark:border-gray-700">
                        <td colSpan={6}>
                          <Spinner />
                        </td>
                      </tr>
                    )}
                  </tbody>
                </table>
              </div>
            </div>

            <div className="uai-shadow p-2 my-4">
              <h2 className="font-bold mb-2 inline">Estudios</h2>

              <div className="uai-shadow relative overflow-x-auto mt-2">
                <table className="w-full text-sm text-left rtl:text-right text-gray-500 dark:text-gray-400">
                  <thead className="text-xs text-gray-700 uppercase bg-gray-50 dark:bg-gray-700 dark:text-gray-400">
                    <tr>
                      <th scope="col" className="px-6 py-3">
                        Institucion
                      </th>
                      <th scope="col" className="px-6 py-3">
                        Nivel
                      </th>
                      <th scope="col" className="px-6 py-3">
                        Ingreso
                      </th>
                      <th scope="col" className="px-6 py-3">
                        Egreso
                      </th>
                      <th scope="col" className="px-6 py-3">
                        Título
                      </th>
                    </tr>
                  </thead>
                  <tbody>
                    {curriculum?.estudios &&
                      curriculum?.estudios.map((e, index) => (
                        <tr
                          key={index}
                          className="bg-white dark:bg-white dark:border-gray-700"
                        >
                          <td className="px-6 py-4 flex">
                            <span className="font-bold">{e.institucion}</span>
                          </td>
                          <td className="px-6 py-4">{e.nivel}</td>
                          <td className="px-6 py-4">
                            {formatearFecha(e.ingreso)}
                          </td>
                          <td className="px-6 py-4">
                            {e.egreso ? formatearFecha(e.egreso) : "En curso"}
                          </td>
                          <td className="px-6 py-4">{e.titulo}</td>
                        </tr>
                      ))}
                    {!curriculum?.estudios && (
                      <tr className="bg-white border-b dark:bg-white dark:border-gray-700">
                        <td colSpan={5}>
                          <Spinner />
                        </td>
                      </tr>
                    )}
                  </tbody>
                </table>
              </div>
            </div>

            <div className="uai-shadow p-2 my-4">
              <h2 className="font-bold mb-2 inline">Idiomas</h2>

              <div>
                {curriculum?.idiomas &&
                  curriculum?.idiomas.map((e, index) => (
                    <div key={index} className="border-b p-2 flex mt-2">
                      <h2 className="text-sm font-bold inline">{e.nombre}:</h2>
                      <span className="text-sm ml-2">{e.nivel}</span>
                    </div>
                  ))}
                {!curriculum?.idiomas && <Spinner />}
              </div>
            </div>

            <div className="uai-shadow p-2 my-4">
              <h2 className="font-bold mb-2 inline">
                Cursos y certificaciones
              </h2>

              <div className="uai-shadow relative overflow-x-auto mt-2">
                <table className="w-full text-sm text-left rtl:text-right text-gray-500 dark:text-gray-400">
                  <thead className="text-xs text-gray-700 uppercase bg-gray-50 dark:bg-gray-700 dark:text-gray-400">
                    <tr>
                      <th scope="col" className="px-6 py-3">
                        Institución emisora
                      </th>
                      <th scope="col" className="px-6 py-3">
                        Título
                      </th>
                      <th scope="col" className="px-6 py-3">
                        Fecha
                      </th>
                    </tr>
                  </thead>
                  <tbody>
                    {curriculum?.cursos &&
                      curriculum?.cursos.map((e, index) => (
                        <tr
                          key={index}
                          className="bg-white dark:bg-white dark:border-gray-700"
                        >
                          <td className="px-6 py-4 flex">
                            <span className="font-bold">{e.institucion}</span>
                          </td>
                          <td className="px-6 py-4">{e.titulo}</td>
                          <td className="px-6 py-4">
                            {formatearFecha(e.fecha)}
                          </td>
                        </tr>
                      ))}
                    {!curriculum?.cursos && (
                      <tr className="bg-white border-b dark:bg-gray-400 dark:border-gray-700">
                        <td colSpan={5}>
                          <Spinner />
                        </td>
                      </tr>
                    )}
                  </tbody>
                </table>
              </div>
            </div>

            <div className="uai-shadow p-2 my-4">
              <h2 className="font-bold mb-2">Intereses</h2>

              <textarea
                className="w-full border border-solid p-2"
                cols={30}
                rows={3}
                disabled
                value={curriculum?.intereses}
              ></textarea>
            </div>

            <div className="uai-shadow p-2 my-4">
              <h2 className="font-bold mb-2">Perfiles</h2>
              <Select
                className="mt-2"
                placeholder=""
                isLoading={!curriculum?.perfiles}
                isMulti={true}
                isDisabled={true}
                value={curriculum?.perfiles}
              />
            </div>

            <div className="uai-shadow p-2 my-4">
              <h2 className="font-bold mb-2">Etiquetas</h2>
              <Select
                className="mt-2"
                placeholder=""
                isLoading={!curriculum?.etiquetas}
                isMulti={true}
                isDisabled={true}
                value={curriculum?.etiquetas}
              />
            </div>
          </div>
        </div>

        <div className="flex mt-2">
          <button onClick={backButtonClick} type="button" className="boton">
            Volver
          </button>
        </div>
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
      <Modal
        mostrar={mostrarCalificaciones}
        titulo={"Calificaciones"}
        onCambiarModal={limpiarModalCalificaciones}
      >
        <div className="min-w-[500px]">
          {calificaciones &&
            calificaciones.map((x, index) => (
              <div key={index}>
                <div className="flex items-center mb-2 mt-2">
                  <h1 className="font-bold">{x.usuario}</h1>

                  <div className="flex items-center ml-2">
                    {Array.from({ length: 5 }, (_, index) =>
                      index < x.puntuacion ? (
                        <svg
                          key={index}
                          className="w-4 h-4 text-yellow-300 ms-1"
                          aria-hidden="true"
                          xmlns="http://www.w3.org/2000/svg"
                          fill="currentColor"
                          viewBox="0 0 22 20"
                        >
                          <path d="M20.924 7.625a1.523 1.523 0 0 0-1.238-1.044l-5.051-.734-2.259-4.577a1.534 1.534 0 0 0-2.752 0L7.365 5.847l-5.051.734A1.535 1.535 0 0 0 1.463 9.2l3.656 3.563-.863 5.031a1.532 1.532 0 0 0 2.226 1.616L11 17.033l4.518 2.375a1.534 1.534 0 0 0 2.226-1.617l-.863-5.03L20.537 9.2a1.523 1.523 0 0 0 .387-1.575Z" />
                        </svg>
                      ) : (
                        <svg
                          key={index}
                          className="w-4 h-4 ms-1 text-gray-300 dark:text-gray-500"
                          aria-hidden="true"
                          xmlns="http://www.w3.org/2000/svg"
                          fill="currentColor"
                          viewBox="0 0 22 20"
                        >
                          <path d="M20.924 7.625a1.523 1.523 0 0 0-1.238-1.044l-5.051-.734-2.259-4.577a1.534 1.534 0 0 0-2.752 0L7.365 5.847l-5.051.734A1.535 1.535 0 0 0 1.463 9.2l3.656 3.563-.863 5.031a1.532 1.532 0 0 0 2.226 1.616L11 17.033l4.518 2.375a1.534 1.534 0 0 0 2.226-1.617l-.863-5.03L20.537 9.2a1.523 1.523 0 0 0 .387-1.575Z" />
                        </svg>
                      )
                    )}
                  </div>
                </div>
                <div className="text-left">
                  <p className="italic">{x.observaciones}</p>
                </div>
                <hr className="mt-2" />
              </div>
            ))}
        </div>
      </Modal>
    </Layout>
  );
}
