"use client";
import { useState, useEffect, useRef, useMemo } from "react";
import Layout from "../../layoutUser";
import { useRouter } from "next/navigation";
import Select, { SingleValue } from "react-select";
import { fetchPrivado, obtenerRolUsuario } from "@/componentes/compartido";
import Modal from "@/componentes/compartido/modal";
import { mensajeErrorGeneral } from "@/constants";
import { SelectOpcion } from "@/componentes/compartido/select/selectOpcion";
import JoditEditor from "jodit-react";

export default function Nuevo() {
  const { push } = useRouter();
  const [rolUsuario, setRolUsuario] = useState<string>();
  const [mensajeModal, setMensajeModal] = useState<string>();
  const [tituloModal, setTituloModal] = useState<string>("");
  const [titulo, setTitulo] = useState<string>("");
  const [ubicacion, setUbicacion] = useState<string>("");
  const [remuneracion, setRemuneracion] = useState<string>("");
  const [horarios, setHorarios] = useState<string>("");
  const [destacado, setDestacado] = useState<boolean>(false);

  const editor = useRef(null);
  const [contenidoEditor, setContenidoEditor] = useState("");

  const [perfiles, setPerfiles] = useState<readonly SelectOpcion[]>([]);
  const [perfilesSistema, setPerfilesSistema] = useState<SelectOpcion[]>();

  const [etiquetas, setEtiquetas] = useState<readonly SelectOpcion[]>([]);
  const [etiquetasSistema, setEtiquetasSistema] = useState<SelectOpcion[]>();

  const [modalidadTrabajo, setModalidadTrabajo] = useState<string>();
  const modalidadesTrabajoSistema: SelectOpcion[] = [
    { label: "Full-time", value: "Full-time" },
    { label: "Part-time", value: "Part-time" },
    { label: "Freelance", value: "Freelance" },
  ];

  const [tipoTrabajo, setTipoTrabajo] = useState<string>();
  const tiposTrabajoSistema: SelectOpcion[] = [
    { label: "Presencial", value: "Presencial" },
    { label: "Remoto", value: "Remoto" },
    { label: "Híbrida", value: "Híbrida" },
  ];

  const configuracionEditor = useMemo(
    () => ({
      readonly: false, // Habilita/deshabilita solo lectura
      language: "es", // Configura el idioma a español
      minHeight: 285, // Establece el alto mínimo en píxeles
      toolbarSticky: true,
    }),
    []
  );

  useEffect(() => {
    const inicializarNuevoEmpleo = async () => {
      await fetchPrivado("http://localhost:4000/empleos/0", "GET")
        .then(async (data) => {
          if (data.ok) {
            const respuesta = await data.json();
            cargarDatosIniciales(respuesta);

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

    const rol = obtenerRolUsuario();

    if (rol === null) {
      return;
    }

    setRolUsuario(rol);

    inicializarNuevoEmpleo();
  }, []);

  const guardarClick = async () => {
    // const curriculum = {
    //   titulo,
    //   ubicacion,
    //   resumenProfesional,
    //   experienciaLaboral: JSON.stringify(empleos),
    //   educacion: JSON.stringify(estudios),
    //   idiomas: JSON.stringify(idiomas),
    //   certificados: JSON.stringify(cursos),
    //   intereses,
    //   etiquetas: etiquetas.map((x) => ({ id: x.value, nombre: x.label })),
    //   perfiles: perfiles.map((x) => ({ id: x.value, nombre: x.label })),
    // };
    // await fetchPrivado(
    //   "http://localhost:4000/curriculums/",
    //   "PUT",
    //   JSON.stringify({ curriculum })
    // )
    //   .then(async (data) => {
    //     if (data.ok) {
    //       setTituloModal("Curriculum");
    //       setMensajeModal(
    //         "Se han guardado correctamente los cambios en el curriculum"
    //       );
    //       return;
    //     }
    //     setTituloModal("Error");
    //     setMensajeModal(mensajeErrorGeneral);
    //   })
    //   .catch(() => {
    //     setTituloModal("Error");
    //     setMensajeModal(mensajeErrorGeneral);
    //   });
  };

  const cargarDatosIniciales = (datos: any) => {
    var opcionesEtiquetas = datos.etiquetas.map(
      (x: any) => new Option(x.nombre, x.id)
    );
    var opcionesPerfiles = datos.perfiles.map(
      (x: any) => new Option(x.nombre, x.id)
    );

    setEtiquetasSistema(opcionesEtiquetas);
    setPerfilesSistema(opcionesPerfiles);
  };

  const backButtonClick = async () => {
    push("/admin");
  };
  const limpiarModal = () => setMensajeModal(undefined);

  return (
    <Layout>
      <div className="cuerpo">
        <h1 className="font-bold mb-2">Nueva vacante de empleo</h1>

        <div className="grid grid-cols-3 gap-4">
          {/* Panel izquierdo */}
          <div>
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
              <h2 className="font-bold mb-2">Modalidad de Trabajo</h2>
              <Select
                className="mt-2"
                placeholder=""
                isLoading={!modalidadesTrabajoSistema}
                isMulti={false}
                onChange={(opcionSeleccionada: SingleValue<SelectOpcion>) => {
                  opcionSeleccionada &&
                    setModalidadTrabajo(opcionSeleccionada.value);
                }}
                options={modalidadesTrabajoSistema}
                value={modalidadesTrabajoSistema.filter(function (opcion) {
                  return opcion.value === modalidadTrabajo;
                })}
                isClearable={false}
              />
            </div>

            <div className="uai-shadow p-2 my-4">
              <h2 className="font-bold mb-2">Tipo de Trabajo</h2>
              <Select
                className="mt-2"
                placeholder=""
                isLoading={!tiposTrabajoSistema}
                isMulti={false}
                onChange={(opcionSeleccionada: SingleValue<SelectOpcion>) => {
                  opcionSeleccionada &&
                    setTipoTrabajo(opcionSeleccionada.value);
                }}
                options={tiposTrabajoSistema}
                value={tiposTrabajoSistema.filter(function (opcion) {
                  return opcion.value === tipoTrabajo;
                })}
                isClearable={false}
              />
            </div>
            <div className="uai-shadow p-2 my-4">
              <h2 className="font-bold">Horarios laborales</h2>
              <input
                type="text"
                name="horarios"
                value={horarios}
                className="border w-full mt-1"
                onChange={(e) => setHorarios(e.target.value)}
                required
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
                  name="remuneracion"
                  value={remuneracion}
                  className="border w-full mt-1"
                  onChange={(e) => setRemuneracion(e.target.value)}
                  required
                />
              </div>
              {rolUsuario === "Administrador" && (
                <div className="uai-shadow p-2">
                  <h2 className="font-bold">Destacado</h2>
                  <label className="flex items-center mt-1 text-right">
                    <input
                      type="checkbox"
                      name="destacado"
                      checked={destacado}
                      className="border w-6 h-6"
                      onChange={(e) => setDestacado(e.target.checked)}
                    />
                    Marcar empleo como destacado
                  </label>
                </div>
              )}
            </div>
          </div>

          {/* Panel derecho */}
          <div className="col-span-2">
            <div id="wysiwyg-editor" className="uai-shadow p-2 my-4">
              <JoditEditor
                ref={editor}
                value={contenidoEditor}
                config={configuracionEditor}
                onChange={(nuevoContenido) =>
                  setContenidoEditor(nuevoContenido)
                }
              />
            </div>
            <div className="uai-shadow p-2 my-4">
              <h2 className="font-bold mb-2">Perfiles</h2>
              <Select
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
          </div>
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
    </Layout>
  );
}
