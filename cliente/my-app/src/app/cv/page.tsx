"use client";
import { useState, useEffect } from "react";
import Layout from "../layoutUser";
import { useRouter } from "next/navigation";
import Select from "react-select";
import { fetchPrivado, formatearFecha } from "@/componentes/compartido";
import Modal from "@/componentes/compartido/modal";
import { mensajeErrorGeneral } from "@/constants";
import Spinner from "@/componentes/compartido/spinner";
import ModalEmpleo from "@/componentes/cv/modalEmpleo";
import ModalEstudio from "@/componentes/cv/modalEstudio";
import ModalCurso from "@/componentes/cv/modalCurso";
import { SelectOpcion } from "@/componentes/compartido/select/selectOpcion";
import ModalIdioma from "@/componentes/cv/modalIdioma";
import { idiomasSistema } from "@/componentes/compartido/idiomas/idiomas-sistema";

export default function Cv() {
  const { push } = useRouter();
  const [mensajeModal, setMensajeModal] = useState<string>();
  const [tituloModal, setTituloModal] = useState<string>("");
  const [titulo, setTitulo] = useState<string>("");
  const [ubicacion, setUbicacion] = useState<string>("");
  const [resumenProfesional, setResumenProfesional] = useState<string>("");
  const [intereses, setIntereses] = useState<string>("");
  const [empleos, setEmpleos] = useState<
    Array<{
      organizacion: string;
      cargo: string;
      ingreso: Date;
      egreso: Date | null;
      area: string;
      funciones: string;
    }>
  >();
  const [empleo, setEmpleo] = useState<{
    idx: number;
    organizacion: string;
    cargo: string;
    ingreso: Date;
    egreso: Date | null;
    area: string;
    funciones: string;
  }>();

  const [estudios, setEstudios] = useState<
    Array<{
      institucion: string;
      nivel: string;
      ingreso: Date;
      egreso: Date | null;
      titulo: string;
    }>
  >();

  const [estudio, setEstudio] = useState<{
    idx: number;
    institucion: string;
    nivel: string;
    ingreso: Date;
    egreso: Date | null;
    titulo: string;
  }>();

  const [idiomas, setIdiomas] = useState<
    Array<{
      nombre: string;
      nivel: string;
    }>
  >([]);

  const [mostrarModalIdiomas, setMostrarModalIdiomas] =
    useState<boolean>(false);

  const [cursos, setCursos] = useState<
    Array<{
      institucion: string;
      titulo: string;
      fecha: Date;
    }>
  >();

  const [curso, setCurso] = useState<{
    idx: number;
    institucion: string;
    titulo: string;
    fecha: Date;
  }>();

  const [perfiles, setPerfiles] = useState<readonly SelectOpcion[]>([]);
  const [perfilesSistema, setPerfilesSistema] = useState<SelectOpcion[]>();

  const [etiquetas, setEtiquetas] = useState<readonly SelectOpcion[]>([]);
  const [etiquetasSistema, setEtiquetasSistema] = useState<SelectOpcion[]>();

  useEffect(() => {
    const obtenerDatosCv = async () => {
      await fetchPrivado("http://localhost:4000/curriculums/", "GET")
        .then(async (data) => {
          if (data.ok) {
            const respuesta = await data.json();
            cargarDatosCv(respuesta);

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

    obtenerDatosCv();
  }, []);

  const guardarClick = async () => {
    const curriculum = {
      titulo,
      ubicacion,
      resumenProfesional,
      experienciaLaboral: JSON.stringify(empleos),
      educacion: JSON.stringify(estudios),
      idiomas: JSON.stringify(idiomas),
      certificados: JSON.stringify(cursos),
      intereses,
      etiquetas: etiquetas.map((x) => ({ id: x.value, nombre: x.label })),
      perfiles: perfiles.map((x) => ({ id: x.value, nombre: x.label })),
    };

    await fetchPrivado(
      "http://localhost:4000/curriculums/",
      "PUT",
      JSON.stringify({ curriculum })
    )
      .then(async (data) => {
        if (data.ok) {
          setTituloModal("Curriculum");
          setMensajeModal(
            "Se han guardado correctamente los cambios en el curriculum"
          );
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

  const cargarDatosCv = (datos: any) => {
    const { curriculum } = datos;
    const datosEmpleos = curriculum.experienciaLaboral
      ? JSON.parse(curriculum.experienciaLaboral)
      : [];
    setEmpleos(datosEmpleos);

    const datosEstudios = curriculum.educacion
      ? JSON.parse(curriculum.educacion)
      : [];
    setEstudios(datosEstudios);

    const datosIdiomas = curriculum.idiomas
      ? JSON.parse(curriculum.idiomas)
      : [];
    setIdiomas(datosIdiomas);

    const datosCursos = curriculum.certificados
      ? JSON.parse(curriculum.certificados)
      : [];
    setCursos(datosCursos);

    if (curriculum.titulo) {
      setTitulo(curriculum.titulo);
    }

    if (curriculum.ubicacion) {
      setUbicacion(curriculum.ubicacion);
    }

    if (curriculum.resumenProfesional) {
      setResumenProfesional(curriculum.resumenProfesional);
    }

    if (curriculum.intereses) {
      setIntereses(curriculum.intereses);
    }

    var opcionesEtiquetas = datos.etiquetas.map(
      (x: any) => new Option(x.nombre, x.id)
    );
    var opcionesPerfiles = datos.perfiles.map(
      (x: any) => new Option(x.nombre, x.id)
    );

    setEtiquetasSistema(opcionesEtiquetas);
    setPerfilesSistema(opcionesPerfiles);

    if (curriculum.perfiles) {
      const datosPerfiles = curriculum.perfiles.map(
        (x: any) => new Option(x.nombre, x.id)
      );
      setPerfiles(datosPerfiles);
    }

    if (curriculum.etiquetas) {
      const datosEtiquetas = curriculum.etiquetas.map(
        (x: any) => new Option(x.nombre, x.id)
      );
      setEtiquetas(datosEtiquetas);
    }
  };

  const backButtonClick = async () => {
    push("/empleos");
  };

  const clickAceptarEmpleo = (nuevoEmpleo: {
    organizacion: string;
    cargo: string;
    ingreso: Date;
    egreso: Date | null;
    area: string;
    funciones: string;
  }) => {
    if (!empleos) {
      return;
    }

    let empleosActualizado;

    if (empleo && empleo.idx !== -1) {
      empleosActualizado = [...empleos];
      empleosActualizado[empleo.idx] = nuevoEmpleo;
    } else {
      empleosActualizado = [...empleos, nuevoEmpleo];
    }

    setEmpleos(empleosActualizado);

    limpiarModalEmpleo();
  };

  const clickAceptarEstudio = (nuevoEstudio: {
    institucion: string;
    nivel: string;
    ingreso: Date;
    egreso: Date | null;
    titulo: string;
  }) => {
    if (!estudios) {
      return;
    }

    let estudiosActualizado;

    if (estudio && estudio.idx !== -1) {
      estudiosActualizado = [...estudios];
      estudiosActualizado[estudio.idx] = nuevoEstudio;
    } else {
      estudiosActualizado = [...estudios, nuevoEstudio];
    }

    setEstudios(estudiosActualizado);

    limpiarModalEstudio();
  };

  const clickAceptarCurso = (nuevoCurso: {
    institucion: string;
    titulo: string;
    fecha: Date;
  }) => {
    if (!cursos) {
      return;
    }

    let cursosActualizado;

    if (curso && curso.idx !== -1) {
      cursosActualizado = [...cursos];
      cursosActualizado[curso.idx] = nuevoCurso;
    } else {
      cursosActualizado = [...cursos, nuevoCurso];
    }

    setCursos(cursosActualizado);

    limpiarModalCurso();
  };

  const clickAceptarIdioma = (nombre: string, nivel: string) => {
    const idiomasActualizado = [...idiomas, { nombre, nivel }];
    setIdiomas(idiomasActualizado);
    limpiarModalIdiomas();
  };

  const clickAgregarEmpleo = () => {
    setEmpleo({
      idx: -1,
      organizacion: "",
      area: "",
      cargo: "",
      ingreso: new Date(),
      egreso: null,
      funciones: "",
    });
  };

  const clickEliminarEmpleo = (idx: number) => {
    const empleosActualizado = empleos?.filter((_, index) => index !== idx);
    setEmpleos(empleosActualizado);
  };

  const clickAgregarEstudio = () => {
    setEstudio({
      idx: -1,
      institucion: "",
      nivel: "",
      ingreso: new Date(),
      egreso: null,
      titulo: "",
    });
  };

  const clickEliminarEstudio = (idx: number) => {
    const estudiosActualizado = estudios?.filter((_, index) => index !== idx);
    setEstudios(estudiosActualizado);
  };

  const clickAgregarCurso = () => {
    setCurso({
      idx: -1,
      institucion: "",
      fecha: new Date(),
      titulo: "",
    });
  };

  const clickEliminarCurso = (idx: number) => {
    const cursosActualizado = cursos?.filter((_, index) => index !== idx);
    setCursos(cursosActualizado);
  };

  const clickAgregarIdioma = () => {
    setMostrarModalIdiomas(true);
  };

  const clickEliminarIdioma = (nombre: string) => {
    const idiomasActualizado = idiomas.filter((x) => x.nombre !== nombre);
    setIdiomas(idiomasActualizado);
  };

  const clickEditarEmpleo = (idx: number) => {
    if (!empleos) {
      return;
    }

    setEmpleo({ ...empleos[idx], idx });
  };

  const clickEditarEstudio = (idx: number) => {
    if (!estudios) {
      return;
    }

    setEstudio({ ...estudios[idx], idx });
  };

  const clickEditarCurso = (idx: number) => {
    if (!cursos) {
      return;
    }

    setCurso({ ...cursos[idx], idx });
  };

  const limpiarModal = () => setMensajeModal(undefined);
  const limpiarModalEmpleo = () => setEmpleo(undefined);
  const limpiarModalEstudio = () => setEstudio(undefined);
  const limpiarModalCurso = () => setCurso(undefined);
  const limpiarModalIdiomas = () => setMostrarModalIdiomas(false);

  return (
    <Layout>
      <div className="cuerpo">
        <h1 className="font-bold mb-2">Mi CV</h1>

        <div className="uai-shadow p-2 my-4">
          <h2 className="font-bold">Título</h2>
          <input
            type="text"
            id="titulo"
            name="titulo"
            value={titulo}
            className="border w-full mt-1"
            onChange={(e) => setTitulo(e.target.value)}
            required
          />
        </div>

        <div className="uai-shadow p-2 my-4">
          <h2 className="font-bold">Ubicación</h2>
          <input
            type="text"
            id="ubicacion"
            name="ubicacion"
            value={ubicacion}
            className="border w-full mt-1"
            onChange={(e) => setUbicacion(e.target.value)}
            required
          />
        </div>

        <div className="uai-shadow p-2 my-4">
          <h2 className="font-bold">Resumen profesional</h2>
          <textarea
            className="w-full border border-solid p-2"
            name="resumenProfesional"
            id="resumenProfesional"
            cols={30}
            rows={3}
            onChange={(e) => setResumenProfesional(e.target.value)}
            value={resumenProfesional}
          ></textarea>
        </div>

        <div className="uai-shadow p-2 my-4">
          <h2 className="font-bold mb-2 inline">Empleos</h2>
          <button
            className="boton ml-2"
            style={{ minWidth: "auto", minHeight: "auto", padding: "5px" }}
            onClick={clickAgregarEmpleo}
          >
            <svg
              xmlns="http://www.w3.org/2000/svg"
              fill="none"
              viewBox="0 0 24 24"
              strokeWidth="1.5"
              stroke="currentColor"
              className="w-4 h-4"
            >
              <path
                strokeLinecap="round"
                strokeLinejoin="round"
                d="M12 4.5v15m7.5-7.5h-15"
              />
            </svg>
          </button>

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
                {empleos &&
                  empleos.map((e, index) => {
                    return (
                      <tr
                        key={index}
                        className="bg-white dark:bg-white dark:border-gray-700"
                      >
                        <td className="px-6 py-4 flex">
                          <button
                            className="boton mr-2"
                            style={{
                              minWidth: "auto",
                              minHeight: "auto",
                              padding: "5px",
                            }}
                            onClick={() => {
                              clickEliminarEmpleo(index);
                            }}
                          >
                            <svg
                              xmlns="http://www.w3.org/2000/svg"
                              fill="none"
                              viewBox="0 0 24 24"
                              strokeWidth="1.5"
                              stroke="currentColor"
                              className="w-3 h-3"
                            >
                              <path
                                strokeLinecap="round"
                                strokeLinejoin="round"
                                d="M6 18 18 6M6 6l12 12"
                              />
                            </svg>
                          </button>
                          <a
                            href="#"
                            onClick={() => {
                              clickEditarEmpleo(index);
                            }}
                            className="font-bold"
                          >
                            {e.organizacion}
                          </a>
                        </td>
                        <td className="px-6 py-4">{e.cargo}</td>
                        <td className="px-6 py-4">
                          {formatearFecha(e.ingreso)}
                        </td>
                        <td className="px-6 py-4">
                          {e.egreso ? formatearFecha(e.egreso) : "Actualmente"}
                        </td>
                        <td className="px-6 py-4">{e.area}</td>
                        <td className="px-6 py-4">{e.funciones}</td>
                      </tr>
                    );
                  })}
                {!empleos && (
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
          <button
            className="boton ml-2"
            style={{ minWidth: "auto", minHeight: "auto", padding: "5px" }}
            onClick={clickAgregarEstudio}
          >
            <svg
              xmlns="http://www.w3.org/2000/svg"
              fill="none"
              viewBox="0 0 24 24"
              strokeWidth="1.5"
              stroke="currentColor"
              className="w-4 h-4"
            >
              <path
                strokeLinecap="round"
                strokeLinejoin="round"
                d="M12 4.5v15m7.5-7.5h-15"
              />
            </svg>
          </button>

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
                {estudios &&
                  estudios.map((e, index) => (
                    <tr
                      key={index}
                      className="bg-white dark:bg-white dark:border-gray-700"
                    >
                      <td className="px-6 py-4 flex">
                        <button
                          className="boton mr-2"
                          style={{
                            minWidth: "auto",
                            minHeight: "auto",
                            padding: "5px",
                          }}
                          onClick={() => {
                            clickEliminarEstudio(index);
                          }}
                        >
                          <svg
                            xmlns="http://www.w3.org/2000/svg"
                            fill="none"
                            viewBox="0 0 24 24"
                            strokeWidth="1.5"
                            stroke="currentColor"
                            className="w-3 h-3"
                          >
                            <path
                              strokeLinecap="round"
                              strokeLinejoin="round"
                              d="M6 18 18 6M6 6l12 12"
                            />
                          </svg>
                        </button>
                        <a
                          href="#"
                          onClick={() => {
                            clickEditarEstudio(index);
                          }}
                          className="font-bold"
                        >
                          {e.institucion}
                        </a>
                      </td>
                      <td className="px-6 py-4">{e.nivel}</td>
                      <td className="px-6 py-4">{formatearFecha(e.ingreso)}</td>
                      <td className="px-6 py-4">
                        {e.egreso ? formatearFecha(e.egreso) : "En curso"}
                      </td>
                      <td className="px-6 py-4">{e.titulo}</td>
                    </tr>
                  ))}
                {!estudios && (
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
          <button
            className="boton ml-2"
            style={{ minWidth: "auto", minHeight: "auto", padding: "5px" }}
            onClick={clickAgregarIdioma}
            disabled={idiomasSistema.length === idiomas.length}
          >
            <svg
              xmlns="http://www.w3.org/2000/svg"
              fill="none"
              viewBox="0 0 24 24"
              strokeWidth="1.5"
              stroke="currentColor"
              className="w-4 h-4"
            >
              <path
                strokeLinecap="round"
                strokeLinejoin="round"
                d="M12 4.5v15m7.5-7.5h-15"
              />
            </svg>
          </button>

          <div>
            {idiomas &&
              idiomas.map((e, index) => (
                <div key={index} className="border-b p-2 flex mt-2">
                  <button
                    className="boton mr-2"
                    style={{
                      minWidth: "auto",
                      minHeight: "auto",
                      padding: "5px",
                    }}
                    onClick={() => {
                      clickEliminarIdioma(e.nombre);
                    }}
                  >
                    <svg
                      xmlns="http://www.w3.org/2000/svg"
                      fill="none"
                      viewBox="0 0 24 24"
                      strokeWidth="1.5"
                      stroke="currentColor"
                      className="w-3 h-3"
                    >
                      <path
                        strokeLinecap="round"
                        strokeLinejoin="round"
                        d="M6 18 18 6M6 6l12 12"
                      />
                    </svg>
                  </button>
                  <h2 className="text-sm font-bold inline">{e.nombre}:</h2>
                  <span className="text-sm ml-2">{e.nivel}</span>
                </div>
              ))}
            {!idiomas && <Spinner />}
          </div>
        </div>

        <div className="uai-shadow p-2 my-4">
          <h2 className="font-bold mb-2 inline">Cursos y certificaciones</h2>
          <button
            className="boton ml-2"
            style={{ minWidth: "auto", minHeight: "auto", padding: "5px" }}
            onClick={clickAgregarCurso}
          >
            <svg
              xmlns="http://www.w3.org/2000/svg"
              fill="none"
              viewBox="0 0 24 24"
              strokeWidth="1.5"
              stroke="currentColor"
              className="w-4 h-4"
            >
              <path
                strokeLinecap="round"
                strokeLinejoin="round"
                d="M12 4.5v15m7.5-7.5h-15"
              />
            </svg>
          </button>

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
                {cursos &&
                  cursos.map((e, index) => (
                    <tr
                      key={index}
                      className="bg-white dark:bg-white dark:border-gray-700"
                    >
                      <td className="px-6 py-4 flex">
                        <button
                          className="boton mr-2"
                          style={{
                            minWidth: "auto",
                            minHeight: "auto",
                            padding: "5px",
                          }}
                          onClick={() => {
                            clickEliminarCurso(index);
                          }}
                        >
                          <svg
                            xmlns="http://www.w3.org/2000/svg"
                            fill="none"
                            viewBox="0 0 24 24"
                            strokeWidth="1.5"
                            stroke="currentColor"
                            className="w-3 h-3"
                          >
                            <path
                              strokeLinecap="round"
                              strokeLinejoin="round"
                              d="M6 18 18 6M6 6l12 12"
                            />
                          </svg>
                        </button>
                        <a
                          href="#"
                          onClick={() => {
                            clickEditarCurso(index);
                          }}
                          className="font-bold"
                        >
                          {e.institucion}
                        </a>
                      </td>
                      <td className="px-6 py-4">{e.titulo}</td>
                      <td className="px-6 py-4">{formatearFecha(e.fecha)}</td>
                    </tr>
                  ))}
                {!cursos && (
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
            name="intereses"
            id="intereses"
            cols={30}
            rows={3}
            onChange={(e) => setIntereses(e.target.value)}
            value={intereses}
          ></textarea>
        </div>

        <div className="uai-shadow p-2 my-4">
          <h2 className="font-bold mb-2">Perfiles</h2>
          <Select
            id="perfiles"
            className="mt-2"
            placeholder=""
            isLoading={!perfilesSistema}
            isMulti={true}
            onChange={(opcionSeleccionada: readonly SelectOpcion[]) => {
              setPerfiles(opcionSeleccionada);
            }}
            options={perfilesSistema}
            value={perfiles}
          />
          <span className="text-xs">
            Seleccione perfiles si desea recibir sugerencias
          </span>
        </div>

        <div className="uai-shadow p-2 my-4">
          <h2 className="font-bold mb-2">Etiquetas</h2>
          <Select
            id="etiquetas"
            className="mt-2"
            placeholder=""
            isLoading={!etiquetasSistema}
            isMulti={true}
            onChange={(opcionSeleccionada: readonly SelectOpcion[]) => {
              setEtiquetas(opcionSeleccionada);
            }}
            options={etiquetasSistema}
            value={etiquetas}
          />
          <span className="text-xs">
            Seleccione etiquetas si desea recibir sugerencias
          </span>
        </div>

        <div className="flex mt-2">
          <button onClick={backButtonClick} type="button" className="boton">
            Volver
          </button>

          <button
            onClick={guardarClick}
            type="button"
            className="boton ml-2 btn-primary"
          >
            Guardar
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
      {empleo && (
        <ModalEmpleo
          mostrar={!!empleo}
          empleo={empleo}
          onAceptarAccion={clickAceptarEmpleo}
          onCambiarModal={limpiarModalEmpleo}
        />
      )}
      {estudio && (
        <ModalEstudio
          mostrar={!!estudio}
          estudio={estudio}
          onAceptarAccion={clickAceptarEstudio}
          onCambiarModal={limpiarModalEstudio}
        />
      )}
      {curso && (
        <ModalCurso
          mostrar={!!curso}
          curso={curso}
          onAceptarAccion={clickAceptarCurso}
          onCambiarModal={limpiarModalCurso}
        />
      )}
      {mostrarModalIdiomas && (
        <ModalIdioma
          mostrar={mostrarModalIdiomas}
          idiomasCv={idiomas.map((x) => x.nombre)}
          onAceptarAccion={clickAceptarIdioma}
          onCambiarModal={limpiarModalIdiomas}
        />
      )}
    </Layout>
  );
}
