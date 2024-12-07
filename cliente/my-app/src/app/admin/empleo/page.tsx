"use client";
import { useState, useEffect } from "react";
import { EditorProvider } from "@tiptap/react";
import Layout from "../../layoutUser";
import { useRouter } from "next/navigation";
import Select from "react-select";
import {
  fetchPrivado,
  formatearFecha,
  obtenerRolUsuario,
} from "@/componentes/compartido";
import Modal from "@/componentes/compartido/modal";
import { mensajeErrorGeneral } from "@/constants";
import Spinner from "@/componentes/compartido/spinner";
import { SelectOpcion } from "@/componentes/compartido/select/selectOpcion";
import ModalIdioma from "@/componentes/cv/modalIdioma";
import { idiomasSistema } from "@/componentes/compartido/idiomas/idiomas-sistema";
import EditorTexto from "@/componentes/compartido/editorTexto/editorTexto";
import { extensionesEditorTexto } from "@/componentes/compartido/editorTexto/extensiones";

export default function Nuevo() {
  const { push } = useRouter();
  const [rolUsuario, setRolUsuario] = useState<string>();
  const [mensajeModal, setMensajeModal] = useState<string>();
  const [tituloModal, setTituloModal] = useState<string>("");
  const [titulo, setTitulo] = useState<string>("");
  const [ubicacion, setUbicacion] = useState<string>("");
  const [idiomas, setIdiomas] = useState<
    Array<{
      nombre: string;
      nivel: string;
    }>
  >([]);

  const [mostrarModalIdiomas, setMostrarModalIdiomas] =
    useState<boolean>(false);

  const [perfiles, setPerfiles] = useState<readonly SelectOpcion[]>([]);
  const [perfilesSistema, setPerfilesSistema] = useState<SelectOpcion[]>();

  const [etiquetas, setEtiquetas] = useState<readonly SelectOpcion[]>([]);
  const [etiquetasSistema, setEtiquetasSistema] = useState<SelectOpcion[]>();

  useEffect(() => {
    const rol = obtenerRolUsuario();

    if (rol === null) {
      return;
    }

    setRolUsuario(rol);
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  useEffect(() => {
    const inicializarNuevoEmpleo = async () => {
      await fetchPrivado("http://localhost:4000/curriculums/", "GET")
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

    // obtenerDatosCv();
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
    const { curriculum } = datos;

    const datosIdiomas = curriculum.idiomas
      ? JSON.parse(curriculum.idiomas)
      : [];
    setIdiomas(datosIdiomas);

    if (curriculum.titulo) {
      setTitulo(curriculum.titulo);
    }

    if (curriculum.ubicacion) {
      setUbicacion(curriculum.ubicacion);
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
    push("/admin");
  };

  const clickAceptarIdioma = (nombre: string, nivel: string) => {
    const idiomasActualizado = [...idiomas, { nombre, nivel }];
    setIdiomas(idiomasActualizado);
    limpiarModalIdiomas();
  };

  const limpiarModal = () => setMensajeModal(undefined);
  const limpiarModalIdiomas = () => setMostrarModalIdiomas(false);

  return (
    <Layout userLayout={false}>
      <div className="cuerpo">
        <h1 className="font-bold mb-2">Nueva vacante de empleo</h1>

        <div className="grid grid-cols-3 gap-4">
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
          </div>
          <div className="col-span-2">
            <div id="tipTapEditor" className="uai-shadow p-2 my-4">
              <EditorProvider
                slotBefore={<EditorTexto />}
                extensions={extensionesEditorTexto}
                content={""}
              ></EditorProvider>
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
