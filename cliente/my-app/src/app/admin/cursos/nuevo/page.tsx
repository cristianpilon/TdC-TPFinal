"use client";
import { useState, useEffect, useRef, useMemo } from "react";
import Layout from "../../../layoutUser";
import { useRouter } from "next/navigation";
import Select from "react-select";
import { fetchPrivado, obtenerRolUsuario } from "@/componentes/compartido";
import Modal from "@/componentes/compartido/modal";
import { mensajeErrorGeneral } from "@/constants";
import { SelectOpcion } from "@/componentes/compartido/select/selectOpcion";
import JoditEditor from "jodit-react";

export default function NuevoCurso() {
  const { push } = useRouter();
  const [mensajeModal, setMensajeModal] = useState<string>();
  const [tituloModal, setTituloModal] = useState<string>("");
  const [titulo, setTitulo] = useState<string>("");
  const [empresa, setEmpresa] = useState<string>("");

  const editor = useRef(null);
  const [contenidoEditor, setContenidoEditor] = useState("");

  const [perfiles, setPerfiles] = useState<readonly SelectOpcion[]>([]);
  const [perfilesSistema, setPerfilesSistema] = useState<SelectOpcion[]>();

  const [etiquetas, setEtiquetas] = useState<readonly SelectOpcion[]>([]);
  const [etiquetasSistema, setEtiquetasSistema] = useState<SelectOpcion[]>();

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
    const inicializarNuevoCurso = async () => {
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

    inicializarNuevoCurso();
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
    push("/admin/cursos");
  };
  const limpiarModal = () => setMensajeModal(undefined);

  return (
    <Layout>
      <div className="cuerpo">
        <h1 className="font-bold mb-2">Nuevo curso</h1>

        <div>
          <div className="uai-shadow p-2 my-4">
            <h2 className="font-bold">Empresa</h2>
            <input
              type="text"
              value={empresa}
              className="border w-full mt-1"
              onChange={(e) => setEmpresa(e.target.value)}
              required
            />
          </div>
          <div className="uai-shadow p-2 my-4">
            <h2 className="font-bold">Título</h2>
            <input
              type="text"
              value={titulo}
              className="border w-full mt-1"
              onChange={(e) => setTitulo(e.target.value)}
              required
            />
          </div>
          <div id="wysiwyg-editor" className="uai-shadow p-2 my-4">
            <JoditEditor
              ref={editor}
              value={contenidoEditor}
              config={configuracionEditor}
              onChange={(nuevoContenido) => setContenidoEditor(nuevoContenido)}
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
